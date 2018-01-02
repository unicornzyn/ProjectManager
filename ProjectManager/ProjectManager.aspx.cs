using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class ProjectManager : System.Web.UI.Page
    {
        protected static int currpage = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(1);

                selParent.DataSource = DAL.ProjectRule.Get().Where(a => a.IsShow == 0);
                selParent.DataTextField = "Name";
                selParent.DataValueField = "Id";
                selParent.DataBind();
                selParent.Items.Insert(0, new ListItem("无", "0"));
            }
        }

        private void BindData(int page)
        {
            var list = DAL.ProjectRule.Get().Where(a => txtSearch.Value == "" || a.Name.ToLower().IndexOf(txtSearch.Value.ToLower()) >= 0 || (a.Url.Length > 0 && a.Url.ToLower().IndexOf(txtSearch.Value.ToLower()) >= 0) || (a.TestUrl.Length > 0 && a.TestUrl.ToLower().IndexOf(txtSearch.Value.ToLower()) >= 0));
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

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProject")
            {
                if (((User)Session["user"]).RoleType == 1)
                {
                    DAL.ProjectRule.Delete(Common.St.ToInt32(e.CommandArgument));
                }
                BindData(currpage);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (((User)Session["user"]).RoleType == 1)
            {
                DAL.ProjectRule.Add(Common.St.ToInt32(txtId.Value), txtName.Value, txtUrl.Value, Common.St.ToInt32(selParent.Value), txtTestUrl.Value, txtSiteFileName.Value, txtDatabaseName.Value, txtTestUserName.Value, txtTestPassword.Value, txtRemark.Value, chkIsShow.Checked ? 1 : 0);
            }
            BindData(currpage);
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var list = DAL.ProjectRule.Get().Where(a => txtSearch.Value == "" || a.Name.ToLower().IndexOf(txtSearch.Value.ToLower()) >= 0 || (a.Url.Length > 0 && a.Url.ToLower().IndexOf(txtSearch.Value.ToLower()) >= 0) || (a.TestUrl.Length > 0 && a.TestUrl.ToLower().IndexOf(txtSearch.Value.ToLower()) >= 0));
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("项目列表");

            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            NPOI.SS.UserModel.ICell cell0 = row.CreateCell(0);
            cell0.SetCellValue("名称");
            NPOI.SS.UserModel.ICell cell1 = row.CreateCell(1);
            cell1.SetCellValue("域名");
            NPOI.SS.UserModel.ICell cell2 = row.CreateCell(2);
            cell2.SetCellValue("是否显示");
            NPOI.SS.UserModel.ICell cell3 = row.CreateCell(3);
            cell3.SetCellValue("测试地址");
            NPOI.SS.UserModel.ICell cell4 = row.CreateCell(4);
            cell4.SetCellValue("网站文件名");
            NPOI.SS.UserModel.ICell cell5 = row.CreateCell(5);
            cell5.SetCellValue("数据库");
            NPOI.SS.UserModel.ICell cell6 = row.CreateCell(6);
            cell6.SetCellValue("测试用户名");
            NPOI.SS.UserModel.ICell cell7 = row.CreateCell(7);
            cell7.SetCellValue("密码");
            NPOI.SS.UserModel.ICell cell8 = row.CreateCell(8);
            cell8.SetCellValue("备注");
            // 内容
            int i = 1;
            foreach (var o in list)
            {
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i);

                NPOI.SS.UserModel.ICell cell00 = row2.CreateCell(0);
                cell00.SetCellValue(o.Name);
                NPOI.SS.UserModel.ICell cell01 = row2.CreateCell(1);
                cell01.SetCellValue(o.Url);
                NPOI.SS.UserModel.ICell cell02 = row2.CreateCell(2);
                cell02.SetCellValue(o.IsShow==1?"是":"否");
                NPOI.SS.UserModel.ICell cell03 = row2.CreateCell(3);
                cell03.SetCellValue(o.TestUrl);
                NPOI.SS.UserModel.ICell cell04 = row2.CreateCell(4);
                cell04.SetCellValue(o.SiteFileName);
                NPOI.SS.UserModel.ICell cell05 = row2.CreateCell(5);
                cell05.SetCellValue(o.DatabaseName);
                NPOI.SS.UserModel.ICell cell06 = row2.CreateCell(6);
                cell06.SetCellValue(o.TestUserName);
                NPOI.SS.UserModel.ICell cell07 = row2.CreateCell(7);
                cell07.SetCellValue(o.TestPassword);
                NPOI.SS.UserModel.ICell cell08 = row2.CreateCell(8);
                cell08.SetCellValue(o.Remark);



                i++;
            }

            // 写入到客户端  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", System.Web.HttpUtility.UrlEncode("项目表", System.Text.Encoding.UTF8)));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }
    }
}