using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

namespace ProjectManager
{
    public partial class ModifyPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string password = Password2.Value;
            User u = (User)Session["user"];
            DAL.UserRule.Update(u.Id, u.UserName, password);
            Response.Redirect("Default.aspx");
        }
    }
}