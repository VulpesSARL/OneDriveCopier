using OneDriveCopier.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveCopier
{
    class DataHolder : IData
    {
        public string Client_ID { get; set; }
        public string[] Scope { get; set; }
        public string State { get; set; }
        public string Tenant { get; set; }
        public DateTime Expires { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Code { get; set; }
        public string Session_State { get; set; }
    }
}
