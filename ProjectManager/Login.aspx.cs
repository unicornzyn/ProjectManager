using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

namespace ProjectManager
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = UserName.Value.Trim();
            string password = Password.Value;

            Model.User u = DAL.UserRule.Get(username);
            if (u.Password == St.GetMd5(username + password)&&u.Status==1)
            {
                Session["user"] = u;
                St.SetCookie("user", u.UserName, DateTime.Now.AddYears(1));
                St.SetCookie("user2", u.Password, DateTime.Now.AddYears(1));
                Response.Redirect("Default.aspx");
            }
            else
            {
                divError.Visible = true;
            }

        }


    }
}