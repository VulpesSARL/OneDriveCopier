using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveCopier
{
    class Reg
    {
        public static string ReadValue(string Name, string Default = "")
        {
            using (RegistryKey k = Registry.CurrentUser.OpenSubKey("Software\\Fox\\OneDriveCopier", false))
            {
                if (k == null)
                    return (Default);
                return (Convert.ToString(k.GetValue(Name, Default)));
            }
        }

        public static void WriteValue(string Name, string Value)
        {
            if (Value == null)
                return;
            using (RegistryKey k = Registry.CurrentUser.CreateSubKey("Software\\Fox\\OneDriveCopier", true))
            {
                if (k == null)
                    return;
                k.SetValue(Name, Value, RegistryValueKind.String);
            }
        }
    }
}
