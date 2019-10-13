using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OneDriveCopier.Login
{
    internal class HTTPClientWrap
    {
        const string URL = "https://login.microsoftonline.com/";
        string SessionID = "";

        HttpWebRequest client = null;
        internal enum Verb
        {
            POST,
            GET,
            PUT,
            PATCH,
            DELETE,
            HEAD
        }

        internal bool SendReq<T>(string URLAppend, Dictionary<string,string> FormPosts, out T Output, out int HTTPResponseCode)
        {
            Output = default(T);
            HTTPResponseCode = 500;

            string DataToSend = "";
            foreach(KeyValuePair <string,string> kvp in FormPosts)
            {
                DataToSend += (DataToSend == "" ? "" : "&") + HttpUtility.UrlEncode(kvp.Key) + "=" + HttpUtility.UrlEncode(kvp.Value);
            }

            string SOutput;

            bool res = SendReq(URLAppend, Verb.POST, DataToSend, out SOutput, out HTTPResponseCode);
            if (res == false)
            {
                Debug.WriteLine(SOutput);
                return (false);
            }

            Output = JsonConvert.DeserializeObject<T>(SOutput);
            return (true);
        }

        internal bool SendReq(string URLAppend, Verb verb, string sdata, out string rdata, out int HTTPResponseCode)
        {
            rdata = "";
            HTTPResponseCode = 200;
            Debug.WriteLine(sdata);

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(sdata);
                client = (HttpWebRequest)WebRequest.Create(URL + URLAppend);
                client.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                client.Pipelined = false;
#if !COMPACT
                client.ServicePoint.Expect100Continue = false;
#endif
                client.AllowAutoRedirect = true;
                if (SessionID != "")
                    client.Headers.Add("Authorization", "Bearer " + SessionID);
#if DEBUG
                client.ReadWriteTimeout = 5000;
                client.Timeout = 5000;
#else
                client.ReadWriteTimeout = 60000;
                client.Timeout = 60000;
#endif
                client.UserAgent = "Custom Graph Client";
                switch (verb)
                {
                    case Verb.DELETE: client.Method = "DELETE"; break;
                    case Verb.GET: client.Method = "GET"; break;
                    case Verb.HEAD: client.Method = "HEAD"; break;
                    case Verb.PATCH: client.Method = "PATCH"; break;
                    case Verb.POST: client.Method = "POST"; break;
                    case Verb.PUT: client.Method = "PUT"; break;
                }

                if (verb != Verb.HEAD && verb != Verb.GET)
                {
                    client.ContentLength = data.Length;
                    client.ContentType = "application/x-www-form-urlencoded";
                    Stream send = client.GetRequestStream();
                    send.Write(data, 0, data.Length);
                    send.Close();
                }
                HttpWebResponse resp = (HttpWebResponse)client.GetResponse();
                HTTPResponseCode = (int)resp.StatusCode;
                StreamReader recv = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                rdata = recv.ReadToEnd();
#if COMPACT
                recv.Close();
                send.Close();
#endif
                return (true);
            }
            catch (WebException ee)
            {
                HttpWebResponse resp = (HttpWebResponse)ee.Response;
                if (resp == null)
                {
                    HTTPResponseCode = 500;
                    Debug.WriteLine("HTTP TIMEOUT, or other issues");
                    rdata = "";
                }
                else
                {
                    HTTPResponseCode = (int)resp.StatusCode;
                    Debug.WriteLine("HTTP " + HTTPResponseCode.ToString() + " - " + URLAppend);
                    StreamReader recv = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                    rdata = recv.ReadToEnd();
                }
                return (false);
            }
            catch (Exception ee)
            {
                HTTPResponseCode = 500;
                Debug.WriteLine("HTTP other error");
                rdata = "";
                Debug.WriteLine(ee.ToString());
                return (false);
            }
        }
    }
}