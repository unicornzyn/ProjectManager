using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class UserManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var state = int.Parse(selState.SelectedValue);
                rpt.DataSource = DAL.UserRule.Get().Where(a => a.Status == state || state == 0);
                rpt.DataBind();

                //selBugziilaUser.DataSource = Common.DB.MySqlHelper.GetDataSet(System.Data.CommandType.Text, "select userid,realname from profiles where is_enabled=1 order by userid desc").Tables[0]; ;
                //selBugziilaUser.DataValueField = "userid";
                //selBugziilaUser.DataTextField = "realname";
                //selBugziilaUser.DataBind();

                selBugziilaUser.Items.Insert(0,new ListItem() { Value = "0", Text = "请选择" });
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (((User)Session["user"]).RoleType == 1)
            {
                DateTime leavetime = txtLeaveTime.Value != "" ? Common.St.ToDateTime(txtLeaveTime.Value) : DateTime.Parse("1900-01-01");
                DAL.UserRule.Add(txtUserName.Value, txtRealName.Value, Common.St.ToInt32(selRole.Value), leavetime,Common.St.ToInt32(selBugziilaUser.SelectedValue));
            }
            Response.Redirect("UserManager.aspx");
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                if (((User)Session["user"]).RoleType == 1)
                {
                    DAL.UserRule.Delete(Common.St.ToInt32(e.CommandArgument));
                }
                Response.Redirect("UserManager.aspx");
            }
            else if (e.CommandName == "ValidUser")
            {
                if (((User)Session["user"]).RoleType == 1)
                {
                    DAL.UserRule.Update(Common.St.ToInt32(e.CommandArgument));
                }
                Response.Redirect("UserManager.aspx");
            }
        }

        protected string GetRoleName(string i)
        {
            if (i == "1")
            {
                return "管理员";
            }
            else if (i == "2")
            {
                return "录入用户";
            }
            else if (i == "3")
            {
                return "普通用户";
            }
            else
            {
                return "";
            }
        }

        protected void selState_SelectedIndexChanged(object sender, EventArgs e)
        {
            var state = int.Parse(selState.SelectedValue);
            rpt.DataSource = DAL.UserRule.Get().Where(a => a.Status == state || state == 0);
            rpt.DataBind();
        }
    }
}