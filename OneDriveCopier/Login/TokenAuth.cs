using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OneDriveCopier.Login
{
    public class TokenAuth : IAuthenticationProvider
    {
        string Auth;
        public TokenAuth(string Auth)
        {
            this.Auth = Auth;
        }

        public Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", Auth);
            return (Task.FromResult<object>(null));
        }
    }
}
