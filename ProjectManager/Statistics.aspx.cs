using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class Statistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {           
            var user=(Model.User)Session["user"];
            if (user.UserName!="youjing")
            {
                Response.Write("Default.aspx");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GridView1.DataSource = Common.DB.MySqlHelper.GetDataSet(System.Data.CommandType.Text,TextBox1.Text);
            GridView1.DataBind();
        }
    }
}