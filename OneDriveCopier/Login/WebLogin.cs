using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace OneDriveCopier.Login
{
    public partial class frmWebLogin : Form
    {
        IData data;
        public frmWebLogin(IData data)
        {
            InitializeComponent();
            this.data = data;
        }

        private void frmWebLogin_Load(object sender, EventArgs e)
        {
            string URL = "https://login.microsoftonline.com/" +
                            HttpUtility.UrlEncode(data.Tenant) + "/oauth2/v2.0/authorize?" +
                            "client_id=" + HttpUtility.UrlEncode(data.Client_ID) + "&" +
                            "response_type=code&" +
                            "scope=" + string.Join("+", data.Scope) + "&" +
                            "redirect_uri=" + HttpUtility.UrlEncode("https://login.microsoftonline.com/common/oauth2/nativeclient") + "&" +
                            "state=" + HttpUtility.UrlEncode(data.State);
            Debug.WriteLine(">> Login URL: " + URL);
            www.Navigated += Www_Navigated;
            www.Navigate(URL);
        }

        private void Www_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (e.Url.ToString().ToLower().StartsWith("https://login.microsoftonline.com/common/oauth2/nativeclient?") == false)
                return;

            Debug.WriteLine(">> Login    : Got it!");

            foreach (string s in e.Url.Query.Split('&'))
            {
                string q = s;
                if (q.StartsWith("?") == true)
                    q = q.Substring(1);
                switch (q.Split('=')[0].ToLower())
                {
                    case "code":
                        data.Code = HttpUtility.UrlDecode(q.Split('=')[1]);
                        break;
                    case "state":
                        data.State = HttpUtility.UrlDecode(q.Split('=')[1]);
                        break;
                    case "session_state":
                        data.Session_State = HttpUtility.UrlDecode(q.Split('=')[1]);
                        break;
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
