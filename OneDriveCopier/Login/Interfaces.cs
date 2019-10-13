using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveCopier.Login
{
    public interface IData
    {
        string Client_ID { get; set; }
        string[] Scope { get; set; }
        string State { get; set; }
        string Tenant { get; set; }
        DateTime Expires { get; set; }
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
        string Code { get; set; }
        string Session_State { get; set; }
    }
}
