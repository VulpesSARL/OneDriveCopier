using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace OneDriveCopier.Login
{
    public class CGraphLogin
    {
        //doc: https://docs.microsoft.com/en-us/graph/auth-v2-user

        IData data;
        public CGraphLogin(IData data)
        {
            this.data = data;

        }

        public bool ProvokeGUILogin()
        {
            return (ProvokeGUILogin(null));
        }

        public bool ProvokeGUILogin(IWin32Window parent)
        {
            frmWebLogin frm = new frmWebLogin(data);
            if (frm.ShowDialog(parent) != DialogResult.OK)
                return (false);
            return (true);
        }

        public bool GetToken()
        {
            if (string.IsNullOrWhiteSpace(data.Code) == true)
                return (false);

            TokenResponse tresponse;
            int httpresponse;

            HTTPClientWrap cli = new HTTPClientWrap();
            cli.SendReq<TokenResponse>(HttpUtility.UrlEncode(data.Tenant) + "/oauth2/v2.0/token",
                new Dictionary<string, string>(){
                    { "client_id", data.Client_ID },
                    { "grant_type","authorization_code" },
                    { "code", data.Code },
                    { "scope", string.Join (" ", data.Scope) },
                    { "redirect_uri", "https://login.microsoftonline.com/common/oauth2/nativeclient" }},
                out tresponse, out httpresponse);

            if (httpresponse != 200)
                return (false);

            data.AccessToken = tresponse.access_token;
            data.RefreshToken = tresponse.refresh_token;
            data.Expires = DateTime.Now.AddSeconds(tresponse.expires_in);

            return (true);
        }

        public bool RefreshToken()
        {
            if (string.IsNullOrWhiteSpace(data.RefreshToken) == true)
                return (false);

            TokenResponse tresponse;
            int httpresponse;

            HTTPClientWrap cli = new HTTPClientWrap();
            cli.SendReq<TokenResponse>(HttpUtility.UrlEncode(data.Tenant) + "/oauth2/v2.0/token",
                new Dictionary<string, string>(){
                    { "client_id", data.Client_ID },
                    { "refresh_token", data.RefreshToken },
                    { "grant_type","refresh_token" },
                    { "scope", string.Join (" ", data.Scope) },
                    { "redirect_uri", "https://login.microsoftonline.com/common/oauth2/nativeclient" }},
                out tresponse, out httpresponse);

            if (httpresponse != 200)
                return (false);

            data.AccessToken = tresponse.access_token;
            data.RefreshToken = tresponse.refresh_token;
            data.Expires = DateTime.Now.AddSeconds(tresponse.expires_in);

            return (true);
        }

        public GraphServiceClient AutomaticGetGraphClient(bool WithGUI = false)
        {
            bool NeedGUI = false;

            if (string.IsNullOrWhiteSpace(data.AccessToken) == false)
            {
                if (data.Expires < DateTime.Now)
                {
                    if (RefreshToken() == false)
                        NeedGUI = true;
                }
                else
                {
                    NeedGUI = true;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(data.Code) == false)
                {
                    if (GetToken() == false)
                        NeedGUI = true;
                }
                else
                {
                    NeedGUI = true;
                }
            }

            if (NeedGUI == true)
            {
                if (WithGUI == false)
                    return (null);
                if (ProvokeGUILogin() == false)
                    return (null);
                if (GetToken() == false)
                    return (null);
            }

            return (GetGraphClient());
        }

        public GraphServiceClient GetGraphClient()
        {
            if (string.IsNullOrWhiteSpace(data.AccessToken) == true)
                return (null);

            GraphServiceClient cli = new GraphServiceClient(new TokenAuth(data.AccessToken));
            return (cli);
        }
    }
}
