using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

namespace ProjectManager
{
    public partial class Statistics5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int week = (int)DateTime.Today.DayOfWeek;
                if (week == 0)
                {
                    week = 7;
                }
                week--;
                DateTime start = DateTime.Today.AddDays(-1 * week);
                txtStart.Value = start.ToString("yyyy-MM-dd");
                txtEnd.Value = start.AddDays(6).ToString("yyyy-MM-dd");
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var bugsurl = System.Configuration.ConfigurationManager.AppSettings["BugzillaUrl"];
            string start = St.ToDateTime(txtStart.Value).ToString("yyyy-MM-dd");
            string end = St.ToDateTime(txtEnd.Value).ToString("yyyy-MM-dd");
            var urlformat = bugsurl+ "/buglist.cgi?chfield=%5BBug%20creation%5D&chfieldfrom="+ start + "&chfieldto="+end+ "&f1=reporter_realname&o1=equals&query_format=advanced&resolution=FIXED&resolution=WONTFIX&resolution=LATER&resolution=REMIND&resolution=DUPLICATE&resolution=WORKSFORME&resolution=MOVED&v1=";
            rpt.DataSource = GetList().AsEnumerable().Select(a => new { Name = GetBugzillaUserName(a.Field<int>("reporter")), CC = a.Field<Int64>("cc"),Link= urlformat+GetBugzillaUserName(a.Field<int>("reporter")) }).Where(a => a.Name != "");
            rpt.DataBind();
        }



        protected void btnExport_Click(object sender, EventArgs e)
        {
            var list = GetList().AsEnumerable().Select(a => new { Name = GetBugzillaUserName(a.Field<int>("reporter")), CC = a.Field<Int64>("cc") }).Where(a => a.Name != "");

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook(new System.IO.FileStream(Server.MapPath("~/template/template_statistics_5.xls"), System.IO.FileMode.Open, System.IO.FileAccess.Read));
            NPOI.SS.UserModel.ISheet sheet = book.GetSheet("bug统计");

            NPOI.SS.UserModel.ICellStyle style = book.CreateCellStyle();
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.WrapText = true;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            // 内容
            int i = 2;
            foreach (var o in list)
            {
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i);
                NPOI.SS.UserModel.ICell cell0 = row2.CreateCell(0);
                cell0.CellStyle = style;
                cell0.SetCellValue(o.Name);
                NPOI.SS.UserModel.ICell cell1 = row2.CreateCell(1);
                cell1.CellStyle = style;
                cell1.SetCellValue(o.CC.ToString());                
                i++;
            }

            // 写入到客户端  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", System.Web.HttpUtility.UrlEncode("Bug数统计", System.Text.Encoding.UTF8)));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }

        protected DataTable GetList()
        {
            string start = St.ToDateTime(txtStart.Value).ToString("yyyy-MM-dd 00:00:00");
            string end = St.ToDateTime(txtEnd.Value).ToString("yyyy-MM-dd 23:59:59");
            string sql = "select reporter,count(bug_id) as cc from bugs where creation_ts between '" + start + "' and '" + end + "' and resolution in('DUPLICATE','FIXED','LATER','MOVED','REMIND','WONTFIX','WORKSFORME') group by reporter";
            var dt = Common.DB.MySqlHelper.GetDataSet(System.Data.CommandType.Text, sql).Tables[0];
            return dt;
        }


        protected string GetBugzillaUserName(int id)
        {
            Dictionary<int, string> dic = UnicornCache.Get<Dictionary<int, string>>(CacheKey.BugzillaUsers);
            if (dic == null)
            {
                dic = new Dictionary<int, string>();
                var ll = DAL.UserRule.Get();
                foreach (var o in ll)
                {
                    dic.Add(o.BugzillaId, o.RealName);
                }
                UnicornCache.Add(CacheKey.BugzillaUsers, dic);
            }

            if (dic.ContainsKey(id))
            {
                return dic[id];
            }
            else
            {
                return "";
            }
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl.SelectedValue == "1")
            {
                int week = (int)DateTime.Today.DayOfWeek;
                if (week == 0)
                {
                    week = 7;
                }
                week--;
                DateTime start = DateTime.Today.AddDays(-1 * week);
                txtStart.Value = start.ToString("yyyy-MM-dd");
                txtEnd.Value = start.AddDays(6).ToString("yyyy-MM-dd");
            }
            else if (ddl.SelectedValue == "2")
            {
                DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                txtStart.Value = start.ToString("yyyy-MM-dd");
                txtEnd.Value = start.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
        }
    }
}