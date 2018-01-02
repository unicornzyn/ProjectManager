using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class ServerManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rpt.DataSource = DAL.ServerRule.Get();
                rpt.DataBind();

                rptPorject.DataSource = DAL.ProjectRule.Get();
                rptPorject.DataBind();
            }
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteServer")
            {
                if (((User)Session["user"]).UserName == "youjing")
                {
                    DAL.ServerRule.Delete(Common.St.ToInt32(e.CommandArgument));
                }
                Response.Redirect("ServerManager.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (((User)Session["user"]).UserName == "youjing")
            {
                int id = Common.St.ToInt32(txtId.Value,0);
                string ServerName = txtServerName.Value.Trim();
                string UserName = txtUserName.Value.Trim();
                string Password = txtPassword.Value.Trim();
                string IISVersion = txtIISVersion.Value.Trim();
                string SqlVersion = txtSqlVersion.Value.Trim();
                string ProjectId = txtProjectId.Value.Trim();
                string OSName = txtOSName.Value.Trim();
                string ServerType = txtServerType.Value.Trim();
                if (id > 0)
                {
                    DAL.ServerRule.Update(id, ServerName, UserName, Password, IISVersion, SqlVersion, ProjectId, OSName, ServerType);
                }
                else
                {
                    DAL.ServerRule.Add(ServerName, UserName, Password, IISVersion, SqlVersion, ProjectId, OSName, ServerType);
                }
                Response.Redirect("ServerManager.aspx");

            }
        }
    }
}