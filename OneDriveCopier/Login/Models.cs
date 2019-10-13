using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveCopier.Login
{

#pragma warning disable 649
    internal class TokenResponse
    {
        public string token_type;
        public string scope;
        public int expires_in;
        public string access_token;
        public string refresh_token;
    }
#pragma warning restore 649


}
