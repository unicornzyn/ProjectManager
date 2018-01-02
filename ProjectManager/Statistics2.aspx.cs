using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class Statistics2 : System.Web.UI.Page
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

                rpt.DataSource = DAL.WorkPlanRule.Get(Common.St.ToDateTime(txtStart.Value + " 00:00:00"), Common.St.ToDateTime(txtEnd.Value + " 23:59:59"));
                rpt.DataBind();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DateTime start=Common.St.ToDateTime(txtStart.Value + " 00:00:00");
            DateTime end=Common.St.ToDateTime(txtEnd.Value + " 23:59:59");
            var list = DAL.WorkPlanRule.Get(start, end);

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook(new System.IO.FileStream(Server.MapPath("~/template/template_statistics_2.xls"), System.IO.FileMode.Open, System.IO.FileAccess.Read));
            NPOI.SS.UserModel.ISheet sheet = book.GetSheet("周上线记录");

            NPOI.SS.UserModel.ICellStyle style = book.CreateCellStyle();
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.WrapText = true;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            NPOI.SS.UserModel.IRow row = sheet.GetRow(0);
            NPOI.SS.UserModel.ICell cell = row.GetCell(0);
            cell.SetCellValue("技术测试周上线记录（" + start.ToString("yyyy年MM月dd日") + "-" + end.ToString("yyyy年MM月dd日") + "）");
            // 内容
            int i = 2;
            foreach (var o in list)
            {
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i);
                NPOI.SS.UserModel.ICell cell0 = row2.CreateCell(0);
                cell0.CellStyle = style;
                cell0.SetCellValue(o.SheepNo);
                NPOI.SS.UserModel.ICell cell1 = row2.CreateCell(1);
                cell1.CellStyle = style;
                cell1.SetCellValue(o.Project.Name);
                NPOI.SS.UserModel.ICell cell2 = row2.CreateCell(2);
                cell2.CellStyle = style;
                cell2.SetCellValue(Common.St.ToDateTimeString(o.PublishTime,"yyyy-MM-dd"));               
                i++;
            }

            // 写入到客户端  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", System.Web.HttpUtility.UrlEncode("每周项目上线记录", System.Text.Encoding.UTF8)));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            rpt.DataSource = DAL.WorkPlanRule.Get(Common.St.ToDateTime(txtStart.Value + " 00:00:00"), Common.St.ToDateTime(txtEnd.Value + " 23:59:59"));
            rpt.DataBind();
        }
    }
}