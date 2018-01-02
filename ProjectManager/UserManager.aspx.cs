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
                rpt.DataSource = DAL.UserRule.Get();
                rpt.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (((User)Session["user"]).RoleType == 1)
            {
                DateTime leavetime = txtLeaveTime.Value != "" ? Common.St.ToDateTime(txtLeaveTime.Value) : DateTime.Parse("1900-01-01");
                DAL.UserRule.Add(txtUserName.Value, txtRealName.Value, Common.St.ToInt32(selRole.Value), leavetime);
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
    }
}