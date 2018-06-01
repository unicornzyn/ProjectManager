using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class DicManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rpt.DataSource = DAL.DicRule.Get();
                rpt.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (((User)Session["user"]).RoleType == 1)
            {
                int id = Common.St.ToInt32(hid.Value);
                if (id > 0)
                {
                    DAL.DicRule.Update(id,txtCode.Value, txtName.Value, Common.St.ToInt32(selType.Value), txtRemark.Value);
                }
                else
                {
                    DAL.DicRule.Add(txtCode.Value, txtName.Value, Common.St.ToInt32(selType.Value), txtRemark.Value);
                }
                
            }
            Response.Redirect("DicManager.aspx");
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteDic")
            {
                if (((User)Session["user"]).RoleType == 1)
                {
                    DAL.DicRule.Delete(Common.St.ToInt32(e.CommandArgument));
                }
                Response.Redirect("DicManager.aspx");
            }
        }

        protected string GetDicName(string i)
        {
            if (i == "1")
            {
                return "研发人员";
            }           
            else
            {
                return "";
            }
        }
    }
}