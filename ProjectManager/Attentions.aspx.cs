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
        protected static int currpage = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(1);

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
                string Remark = HttpUtility.HtmlEncode(txtRemark.Value.Trim());
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

        private void BindData(int page)
        {
            var list = DAL.AttentionsRule.Get().Where(a => txtSearch.Value == "" || a.Remark.ToLower().IndexOf(txtSearch.Value.ToLower()) >= 0 || a.ProjectName.ToLower().IndexOf(txtSearch.Value.ToLower()) >= 0);
            int cc = (int)Math.Ceiling(list.Count() / 10.0);
            if (page <= 0)
            {
                currpage = 1;
            }
            else if (page > cc)
            {
                currpage = cc;
            }
            else
            {
                currpage = page;
            }
            rpt.DataSource = list.Skip((currpage - 1) * 10).Take(10);
            rpt.DataBind();
            spPage.InnerText = currpage + "/" + cc + " 共" + list.Count() + "条";
        }
        protected void lbFirst_Click(object sender, EventArgs e)
        {
            BindData(1);
        }

        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            BindData(currpage - 1);
        }

        protected void lbNext_Click(object sender, EventArgs e)
        {
            BindData(currpage + 1);
        }

        protected void lbLast_Click(object sender, EventArgs e)
        {
            BindData(1000);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(1);
        }
    }
}