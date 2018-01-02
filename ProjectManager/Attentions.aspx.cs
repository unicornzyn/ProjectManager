using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class Attentions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rpt.DataSource = DAL.AttentionsRule.Get();
                rpt.DataBind();

                rptPorject.DataSource = DAL.ProjectRule.Get();
                rptPorject.DataBind();
            }
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteAttentions")
            {
                if (((User)Session["user"]).RoleType < 3)
                {
                    DAL.AttentionsRule.Delete(Common.St.ToInt32(e.CommandArgument));
                }
                Response.Redirect("Attentions.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (((User)Session["user"]).RoleType < 3)
            {
                int id = Common.St.ToInt32(txtId.Value, 0);
                string Remark = txtRemark.Value.Trim();
                string ProjectId = txtProjectId.Value.Trim();

                if (id > 0)
                {
                    DAL.AttentionsRule.Update(id, Remark, ProjectId);
                }
                else
                {
                    DAL.AttentionsRule.Add(Remark, ProjectId);
                }
                Response.Redirect("Attentions.aspx");

            }
        }
    }
}