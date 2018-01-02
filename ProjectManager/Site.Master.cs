using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

namespace ProjectManager
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public User u;
        protected void Page_Init(object sender, EventArgs e)
        {
            //u = new User(); return;
            if (Session["user"] != null)
            {
                u = (User)Session["user"];
                hidUserRole.Value = u.RoleType.ToString();
            }
            else
            {
                string username = Common.St.GetCookie("user");
                string passwd = Common.St.GetCookie("user2");
                if (username != null)
                {
                    u = DAL.UserRule.Get(username);
                    if (u.Password == passwd)
                    {
                        Session["user"] = u;
                        hidUserRole.Value = u.RoleType.ToString();
                    }
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }


            }
        }
    }
}