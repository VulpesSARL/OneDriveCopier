using Microsoft.Graph;
using OneDriveCopier.Login;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveCopier
{
    class FileSystemInfos
    {
        public string OneDriveID;
        public string Name;
        public DateTime Modified;
        public DateTime Created;
        public Int64 SZ;
        public bool IsDir;
        public bool Processed = false;
    }

    class Program
    {
        public static GraphServiceClient graphClient = null;

        public static IEnumerable<T> QuerySync<T>(Task<IQueryable<T>> t)
        {
            t.Wait();
            return (t.Result);
        }

        public static T QuerySync<T>(Task<T> t)
        {
            t.Wait();
            return (t.Result);
        }

        public static void QuerySync(Task t)
        {
            t.Wait();
        }

        static List<FileSystemInfos> ListFiles(string RemotePath)
        {
            List<FileSystemInfos> fsis = new List<FileSystemInfos>();

            DriveItem root;
            try
            {
                if (string.IsNullOrWhiteSpace(RemotePath) == true)
                    root = QuerySync<DriveItem>(graphClient.Drive.Root.Request().Expand("children").GetAsync());
                else
                    root = QuerySync<DriveItem>(graphClient.Drive.Root.ItemWithPath(RemotePath).Request().Expand("children").GetAsync());

                if (root == null)
                {
                    return (null);
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.ToString());
                return (null);
            }

            IDriveItemChildrenCollectionPage currentlist = root.Children;
            do
            {
                foreach (DriveItem item in currentlist.CurrentPage)
                {
                    FileSystemInfos fsie = new FileSystemInfos();
                    fsie.Created = item.FileSystemInfo.CreatedDateTime.Value.DateTime;
                    fsie.Modified = item.FileSystemInfo.LastModifiedDateTime.Value.DateTime;
                    fsie.OneDriveID = item.Id;
                    fsie.Name = item.Name;
                    if (item.Folder == null)
                    {
                        fsie.SZ = item.Size.Value;
                        fsie.IsDir = false;
                    }
                    else
                    {
                        fsie.SZ = 0;
                        fsie.IsDir = true;
                    }
                    fsis.Add(fsie);
                }
                if (currentlist.NextPageRequest == null)
                    break;
                currentlist = QuerySync<IDriveItemChildrenCollectionPage>(currentlist.NextPageRequest.GetAsync());
            } while (true);
            return (fsis);
        }

        static void ListFiles(IDriveItemChildrenCollectionPage dir, Quota quota)
        {
            IDriveItemChildrenCollectionPage currentlist = dir;
            int NumFiles = 0;
            int NumDirs = 0;
            Int64 NumSZ = 0;
            do
            {
                foreach (DriveItem item in currentlist.CurrentPage)
                {
                    string DT = "";
                    if (item.FileSystemInfo != null && item.FileSystemInfo.LastModifiedDateTime != null)
                        DT = item.FileSystemInfo.LastModifiedDateTime.ToString();

                    string SZ = "";
                    if (item.Folder == null)
                    {
                        if (item.Size != null)
                        {
                            SZ = item.Size.ToString().PadLeft(10);
                            NumSZ += item.Size.Value;
                        }
                        else
                        {
                            SZ = "".PadLeft(10);
                        }
                        NumFiles++;
                    }
                    else
                    {
                        SZ = " <DIR>".PadRight(10);
                        NumDirs++;
                    }

                    Console.WriteLine(DT.PadRight(30) + " " + SZ + " " + item.Name);
                }
                if (currentlist.NextPageRequest == null)
                    break;
                currentlist = QuerySync<IDriveItemChildrenCollectionPage>(currentlist.NextPageRequest.GetAsync());
            } while (true);

            Console.WriteLine("");
            Console.WriteLine("  " + NumDirs.ToString() + " director" + (NumDirs == 1 ? "y" : "ies"));
            Console.Write("  " + NumFiles.ToString() + " file" + (NumFiles == 1 ? "" : "s"));
            Console.WriteLine("     " + NumSZ.ToString() + " byte" + (NumSZ == 1 ? "" : "s"));

            Console.WriteLine("");
            if (quota != null && quota.Used != null && quota.Total != null)
            {
                Console.WriteLine("   " + NiceSize(quota.Used.Value) + " used of " + NiceSize(quota.Total.Value));
                Console.WriteLine("");
            }
        }

        static string NiceSize(Int64 SZ)
        {
            string unit = "byte" + (SZ == 1 ? "" : "s");
            double un = SZ;
            if (un > 1024)
            {
                un /= 1024f;
                unit = "KiB";
            }
            if (un > 1024)
            {
                un /= 1024f;
                unit = "MiB";
            }
            if (un > 1024)
            {
                un /= 1024f;
                unit = "GiB";
            }
            if (un > 1024)
            {
                un /= 1024f;
                unit = "TiB";
            }
            return (un.ToString("0.##") + " " + unit);
        }

        static void MarkAsProcessed(List<FileSystemInfos> fsi, string Name, bool IsDir)
        {
            foreach (FileSystemInfos f in fsi)
            {
                if (Name.ToLower() == f.Name.ToLower() && IsDir == f.IsDir)
                {
                    f.Processed = true;
                    return;
                }
            }
        }

        static FileSystemInfos GetFSI(List<FileSystemInfos> fsi, string Name, bool IsDir)
        {
            foreach (FileSystemInfos f in fsi)
            {
                if (Name.ToLower() == f.Name.ToLower() && IsDir == f.IsDir)
                {
                    return (f);
                }
            }
            return (null);
        }

        static bool DTLightTest(DateTime DT1, DateTime DT2)
        {
            return (
                DT1.Year == DT2.Year &&
                DT1.Month == DT2.Month &&
                DT1.Day == DT2.Day &&
                DT1.Hour == DT2.Hour &&
                DT1.Minute == DT2.Minute &&
                DT1.Second == DT2.Second &&
                DT1.Millisecond == DT2.Millisecond);
        }

        static bool RecurseUploadData(string LocalPath, string RemotePath, bool SkipFailed)
        {
            List<FileSystemInfos> RemoteFiles = ListFiles(RemotePath);
            if (RemoteFiles == null)
                return (false);

            foreach (string LPath in System.IO.Directory.EnumerateDirectories(LocalPath, "*.*", System.IO.SearchOption.TopDirectoryOnly))
            {
                string RP = RemotePath;
                RP += "/";
                string PathOnly = LPath.Substring(LPath.LastIndexOf("\\") + 1);
                RP += Uri.EscapeUriString(PathOnly);

                MarkAsProcessed(RemoteFiles, PathOnly, true);

                //Create dir in AZ
                string RemotePathDotDot = RP.Substring(0, RP.LastIndexOf("/"));
                Drive drv = QuerySync<Drive>(graphClient.Drive.Request().GetAsync());
                DriveItem newcreateddir;
                DriveItem newfolder = new DriveItem() { Name = RP.Substring(RP.LastIndexOf("/") + 1), Folder = new Folder() };
                if (string.IsNullOrWhiteSpace(RemotePathDotDot) == true)
                    newcreateddir = QuerySync<DriveItem>(graphClient.Drive.Root.Children.Request().AddAsync(newfolder));
                else
                    newcreateddir = QuerySync<DriveItem>(graphClient.Drive.Root.ItemWithPath(RemotePathDotDot).Children.Request().AddAsync(newfolder));

                if (newcreateddir == null)
                {
                    Console.WriteLine("Cannot create directory");
                    return (false);
                }

                if (RecurseUploadData(LPath, RP, SkipFailed) == false)
                    return (false);
            }

            foreach (string Filename in System.IO.Directory.EnumerateFiles(LocalPath, "*.*", System.IO.SearchOption.TopDirectoryOnly))
            {
                string RemoteFullName = RemotePath + "/" + Uri.EscapeUriString(System.IO.Path.GetFileName(Filename));

                Console.Write(Filename + " -> " + RemoteFullName + " ...      ");

                System.IO.FileInfo fi = new System.IO.FileInfo(Filename);

                DateTime DTCreated = fi.CreationTimeUtc;
                DateTime DTModified = fi.LastWriteTimeUtc;
                Int64 FSZ = fi.Length;

                MarkAsProcessed(RemoteFiles, System.IO.Path.GetFileName(Filename), false);

                FileSystemInfos fsitest = GetFSI(RemoteFiles, System.IO.Path.GetFileName(Filename), false);
                if (fsitest != null)
                {
                    if (DTLightTest(fsitest.Modified, DTModified) && DTLightTest(fsitest.Created, DTCreated) && fsitest.SZ == FSZ)
                    {
                        Console.WriteLine("\b\b\b\b\bSkipped");
                        continue;
                    }
                    else
                    {
                        QuerySync(graphClient.Drive.Items[fsitest.OneDriveID].Request().DeleteAsync());
                    }
                }

                using (System.IO.FileStream fss = System.IO.File.Open(Filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                {
                    FileSystemInfo fsi = new Microsoft.Graph.FileSystemInfo();
                    fsi.CreatedDateTime = new DateTimeOffset(DTCreated);
                    fsi.LastModifiedDateTime = new DateTimeOffset(DTModified);
                    DriveItemUploadableProperties uplprop = new DriveItemUploadableProperties();
                    uplprop.FileSystemInfo = fsi;
                    UploadSession UploadSess = QuerySync<UploadSession>(graphClient.Drive.Root.ItemWithPath(RemoteFullName).CreateUploadSession(uplprop).Request().PostAsync());
                    const int MaxChunk = 320 * 10 * 1024;
                    ChunkedUploadProvider provider = new ChunkedUploadProvider(UploadSess, graphClient, fss, MaxChunk);
                    IEnumerable<UploadChunkRequest> chunckRequests = provider.GetUploadChunkRequests();
                    byte[] readBuffer = new byte[MaxChunk];
                    List<Exception> exceptions = new List<Exception>();
                    bool Res = false;
                    foreach (UploadChunkRequest request in chunckRequests)
                    {
                        Int64 Perc = (Int64)(((decimal)request.RangeBegin / (decimal)request.TotalSessionLength) * 100m);
                        Console.Write("\b\b\b\b\b" + Perc.ToString() + "%");

                        UploadChunkResult result = QuerySync<UploadChunkResult>(provider.GetChunkRequestResponseAsync(request, readBuffer, exceptions));

                        if (result.UploadSucceeded)
                        {
                            Res = true;
                        }
                    }
                    if (Res == false)
                    {
                        Console.WriteLine("\b\b\b\b\bFAILED");
                        if (SkipFailed == false)
                            return (false);
                        else
                            continue;
                    }
                }
                Console.WriteLine("\b\b\b\b\bOK   ");
            }

            foreach (FileSystemInfos fsis in RemoteFiles)
            {
                if (fsis.Processed == true)
                    continue;

                string RemoteFullName = RemotePath + "/" + fsis.Name + (fsis.IsDir == true ? " (DIR)" : ""); //deco only

                Console.Write("Deleting " + RemoteFullName + " ... ");
                QuerySync(graphClient.Drive.Items[fsis.OneDriveID].Request().DeleteAsync());
                Console.WriteLine("OK");
            }
            return (true);
        }


        [STAThread]
        static int Main(string[] args)
        {
            Console.WriteLine("OneDrive Copier - copies files to OneDrive");
            Console.WriteLine("");
            Console.WriteLine("Vulpes SARL, Luxembourg - https://vulpes.lu");
            Console.WriteLine("");

            DataHolder dh = new DataHolder();
            dh.Tenant = Tenants.Consumers;
            dh.Scope = new string[] { Scopes.FilesReadWriteAll, Scopes.OfflineAccess };
            dh.State = Guid.NewGuid().ToString();
            bool Auth = false;
            string RemotePath = "/";
            string NewDir = "";
            string UploadLocalDir = "";
            bool SkipFailed = false;
            int Command = 0;

            for (int i = 0; i < args.Length; i++)
            {
                string thiscmd = args[i].ToLower();
                string nextcmd = "";
                if (i + 1 < args.Length)
                {
                    if (args[i + 1].StartsWith("/") == false)
                        nextcmd = args[i + 1];
                }

                switch (thiscmd)
                {
                    case "/auth":
                        if (nextcmd == "")
                        {
                            Console.WriteLine("Missing Client ID");
                            return (1);
                        }
                        dh.Client_ID = nextcmd;
                        Auth = true;
                        i++;
                        break;
                    case "/skipfailed":
                        SkipFailed = true;
                        break;
                    case "/tennant":
                        if (nextcmd == "")
                        {
                            Console.WriteLine("Missing Tennant");
                            return (1);
                        }
                        dh.Tenant = nextcmd;
                        i++;
                        break;
                    case "/rpath":
                        if (nextcmd == "")
                        {
                            Console.WriteLine("Missing Remote path");
                            return (1);
                        }
                        RemotePath = nextcmd;
                        if (RemotePath.StartsWith("/") == false)
                            RemotePath = "/" + RemotePath;
                        i++;
                        break;
                    case "/newdir":
                        if (nextcmd == "")
                        {
                            Console.WriteLine("Missing new directory");
                            return (1);
                        }
                        NewDir = nextcmd;
                        if (NewDir.StartsWith("/") == true)
                            NewDir = NewDir.Substring(1);
                        i++;
                        break;
                    case "/uploadpath":
                        if (nextcmd == "")
                        {
                            Console.WriteLine("Missing local path to upload");
                            return (1);
                        }
                        UploadLocalDir = nextcmd;
                        i++;
                        break;
                    case "/command":
                        if (nextcmd == "")
                        {
                            Console.WriteLine("Missing commnand");
                            return (1);
                        }
                        switch (nextcmd.ToLower())
                        {
                            case "list":
                                Command = 0;
                                break;
                            case "mkdir":
                                Command = 1;
                                break;
                            case "uploaddir":
                                Command = 2;
                                break;
                            default:
                                Console.WriteLine("Unknown commnand");
                                return (1);
                        }
                        i++;
                        break;
                    case "/?":
                    default:
                        Console.WriteLine("Usage:");
                        Console.WriteLine(" /auth <client id>            Creates a new authentication");
                        Console.WriteLine(" /tennant <tennant>           Selects tennant");
                        Console.WriteLine("                                 consumers  <-- default");
                        Console.WriteLine("                                 common");
                        Console.WriteLine("                                 organizations");
                        Console.WriteLine("                                 GUID");
                        Console.WriteLine(" /command <command>           Performs some action on OneDrive");
                        Console.WriteLine("                                 List       <-- default");
                        Console.WriteLine("                                 MkDir");
                        Console.WriteLine("                                 UploadDir");
                        Console.WriteLine(" /rpath <path>                Set remote path");
                        Console.WriteLine(" /newdir <path>               New directory");
                        Console.WriteLine(" /uploadpath <path>           Local path to upload");
                        Console.WriteLine(" /skipfailed                  Skips failed copies, continues the process");
                        Console.WriteLine("");
                        Console.WriteLine("Register app link:");
                        Console.WriteLine("    https://go.microsoft.com/fwlink/?linkid=2083908");
                        Console.WriteLine("");
                        Console.WriteLine("Set Redirect URI to Public/native (mobile & desktop) and set the URI to");
                        Console.WriteLine("    https://login.microsoftonline.com/common/oauth2/nativeclient");
                        Console.WriteLine("");
                        return (1);
                }
            }

            if (Auth == true)
            {
                Console.WriteLine("Running Authentication ...");
                CGraphLogin aglogin = new CGraphLogin(dh);
                graphClient = aglogin.AutomaticGetGraphClient(true);
                if (graphClient == null)
                {
                    Console.WriteLine("Cannot get access to graph API");
                    return (5);
                }

                Reg.WriteValue("AccessToken", dh.AccessToken);
                Reg.WriteValue("Client_ID", dh.Client_ID);
                Reg.WriteValue("Code", dh.Code);
                Reg.WriteValue("RefreshToken", dh.RefreshToken);
                Reg.WriteValue("State", dh.State);
                Reg.WriteValue("Tenant", dh.Tenant);
                Console.WriteLine("Data stored in registry");
                return (0);
            }

            dh.AccessToken = Reg.ReadValue("AccessToken");
            dh.Client_ID = Reg.ReadValue("Client_ID");
            dh.Code = Reg.ReadValue("Code");
            dh.RefreshToken = Reg.ReadValue("RefreshToken");
            dh.State = Reg.ReadValue("State");
            dh.Tenant = Reg.ReadValue("Tenant");

            if (dh.AccessToken == "" || dh.Client_ID == "" || dh.Code == "" || dh.RefreshToken == "" || dh.State == "" || dh.Tenant == "")
            {
                Console.WriteLine("No data in registry - rerun /auth parameter");
                return (5);
            }

            Console.Write("Authenticating ... ");

            CGraphLogin glogin = new CGraphLogin(dh);
            graphClient = glogin.AutomaticGetGraphClient(false);

            if (graphClient == null)
            {
                Console.WriteLine("Cannot get access to graph API - rerun /auth parameter");
                return (5);
            }

            Console.WriteLine("OK");

            switch (Command)
            {
                case 0: //list / dir
                    {
                        Drive drv = QuerySync<Drive>(graphClient.Drive.Request().GetAsync());
                        DriveItem root;
                        if (string.IsNullOrWhiteSpace(RemotePath) == true)
                            root = QuerySync<DriveItem>(graphClient.Drive.Root.Request().Expand("children").GetAsync());
                        else
                            root = QuerySync<DriveItem>(graphClient.Drive.Root.ItemWithPath(RemotePath).Request().Expand("children").GetAsync());

                        if (root == null)
                        {
                            Console.WriteLine("Cannot get data");
                            return (5);
                        }

                        IDriveItemChildrenCollectionPage currentlist = root.Children;
                        ListFiles(root.Children, drv.Quota);
                        break;
                    }
                case 1: //new dir
                    {
                        if (string.IsNullOrWhiteSpace(NewDir) == true)
                        {
                            Console.WriteLine("No directoy to be created specified");
                            return (5);
                        }

                        Drive drv = QuerySync<Drive>(graphClient.Drive.Request().GetAsync());
                        DriveItem newcreateddir;
                        DriveItem newfolder = new DriveItem() { Name = NewDir, Folder = new Folder() };
                        if (string.IsNullOrWhiteSpace(RemotePath) == true)
                            newcreateddir = QuerySync<DriveItem>(graphClient.Drive.Root.Children.Request().AddAsync(newfolder));
                        else
                            newcreateddir = QuerySync<DriveItem>(graphClient.Drive.Root.ItemWithPath(RemotePath).Children.Request().AddAsync(newfolder));

                        if (newcreateddir == null)
                        {
                            Console.WriteLine("Cannot create directory");
                            return (5);
                        }
                        Console.WriteLine("Directory " + NewDir + " created");
                        break;
                    }
                case 2: //upload dir
                    {
                        if (string.IsNullOrWhiteSpace(UploadLocalDir) == true)
                        {
                            Console.WriteLine("No local directory to upload specified");
                            return (5);
                        }

                        RecurseUploadData(UploadLocalDir, RemotePath, SkipFailed);
                        break;
                    }
            }

#if DEBUG
            Console.WriteLine("");
            Console.WriteLine("Press any key . . . ");
            Console.ReadKey(true);
#endif

            return (0);
        }
    }
}
