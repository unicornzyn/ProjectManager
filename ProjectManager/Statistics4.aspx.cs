using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class Statistics4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<int> list = new List<int>();
                for (int i = 2015; i <= DateTime.Now.Year; i++)
                {
                    list.Add(i);
                }
                selYear.DataSource = list;
                selYear.DataBind();
                selYear.Value = DateTime.Now.Year.ToString();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int year = Common.St.ToInt32(selYear.Value);
            var list = DAL.WorkPlanRule.Get();
            var m = 12;
            if (year == DateTime.Today.Year)
            {
                m = DateTime.Today.Month;
            }
            rpt.DataSource = list.Where(a=>a.NeederId>0 && a.Needer.Status == 1).GroupBy(a => a.NeederId).Select(a =>
            {
                return new
                {
                    Name = a.First().Needer.RealName,
                    M1 = m < 1 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-2-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-2-1") || b.RealEndTime < DateTime.Parse(year + "-1-1"))))).Count(),
                    M2 = m < 2 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-3-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-3-1") || b.RealEndTime < DateTime.Parse(year + "-2-1"))))).Count(),
                    M3 = m < 3 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-4-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-4-1") || b.RealEndTime < DateTime.Parse(year + "-3-1"))))).Count(),
                    M4 = m < 4 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-5-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-5-1") || b.RealEndTime < DateTime.Parse(year + "-4-1"))))).Count(),
                    M5 = m < 5 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-6-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-6-1") || b.RealEndTime < DateTime.Parse(year + "-5-1"))))).Count(),
                    M6 = m < 6 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-7-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-7-1") || b.RealEndTime < DateTime.Parse(year + "-6-1"))))).Count(),
                    M7 = m < 7 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-8-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-8-1") || b.RealEndTime < DateTime.Parse(year + "-7-1"))))).Count(),
                    M8 = m < 8 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-9-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-9-1") || b.RealEndTime < DateTime.Parse(year + "-8-1"))))).Count(),
                    M9 = m < 9 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-10-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-10-1") || b.RealEndTime < DateTime.Parse(year + "-9-1"))))).Count(),
                    M10 = m < 10 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-11-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-11-1") || b.RealEndTime < DateTime.Parse(year + "-10-1"))))).Count(),
                    M11 = m < 11 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-12-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-12-1") || b.RealEndTime < DateTime.Parse(year + "-11-1"))))).Count(),
                    M12 = m < 12 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse((year + 1) + "-1-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse((year + 1) + "-1-1") || b.RealEndTime < DateTime.Parse(year + "-12-1"))))).Count()
                };
            });
            rpt.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int year = Common.St.ToInt32(selYear.Value);
            var m = 12;
            if (year == DateTime.Today.Year)
            {
                m = DateTime.Today.Month;
            }
            var list = DAL.WorkPlanRule.Get().Where(a => a.NeederId > 0 && a.Needer.Status == 1).GroupBy(a => a.NeederId).Select(a =>
            {
                return new
                {
                    Name = a.First().Needer.RealName,
                    M1 = m < 1 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-2-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-2-1") || b.RealEndTime < DateTime.Parse(year + "-1-1"))))).Count(),
                    M2 = m < 2 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-3-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-3-1") || b.RealEndTime < DateTime.Parse(year + "-2-1"))))).Count(),
                    M3 = m < 3 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-4-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-4-1") || b.RealEndTime < DateTime.Parse(year + "-3-1"))))).Count(),
                    M4 = m < 4 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-5-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-5-1") || b.RealEndTime < DateTime.Parse(year + "-4-1"))))).Count(),
                    M5 = m < 5 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-6-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-6-1") || b.RealEndTime < DateTime.Parse(year + "-5-1"))))).Count(),
                    M6 = m < 6 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-7-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-7-1") || b.RealEndTime < DateTime.Parse(year + "-6-1"))))).Count(),
                    M7 = m < 7 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-8-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-8-1") || b.RealEndTime < DateTime.Parse(year + "-7-1"))))).Count(),
                    M8 = m < 8 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-9-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-9-1") || b.RealEndTime < DateTime.Parse(year + "-8-1"))))).Count(),
                    M9 = m < 9 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-10-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-10-1") || b.RealEndTime < DateTime.Parse(year + "-9-1"))))).Count(),
                    M10 = m < 10 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-11-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-11-1") || b.RealEndTime < DateTime.Parse(year + "-10-1"))))).Count(),
                    M11 = m < 11 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse(year + "-12-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse(year + "-12-1") || b.RealEndTime < DateTime.Parse(year + "-11-1"))))).Count(),
                    M12 = m < 12 ? 0 : a.Where(b => (b.State == 1 && b.RealStartTime < DateTime.Parse((year + 1) + "-1-1")) || (b.State == 2 && (!(b.RealStartTime >= DateTime.Parse((year + 1) + "-1-1") || b.RealEndTime < DateTime.Parse(year + "-12-1"))))).Count()
                };
            });


            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook(new System.IO.FileStream(Server.MapPath("~/template/template_statistics_4.xls"), System.IO.FileMode.Open, System.IO.FileAccess.Read));
            NPOI.SS.UserModel.ISheet sheet = book.GetSheet("测试人员负责项目数统计");

            NPOI.SS.UserModel.ICellStyle style = book.CreateCellStyle();
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.WrapText = true;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            // 内容
            int i = 1;
            foreach (var o in list)
            {
                NPOI.SS.UserModel.IRow row = sheet.CreateRow(i);
                NPOI.SS.UserModel.ICell cell0 = row.CreateCell(0);
                cell0.CellStyle = style;
                cell0.SetCellValue(o.Name);
                NPOI.SS.UserModel.ICell cell1 = row.CreateCell(1);
                cell1.CellStyle = style;
                cell1.SetCellValue(o.M1);
                NPOI.SS.UserModel.ICell cell2 = row.CreateCell(2);
                cell2.CellStyle = style;
                cell2.SetCellValue(o.M2);
                NPOI.SS.UserModel.ICell cell3 = row.CreateCell(3);
                cell3.CellStyle = style;
                cell3.SetCellValue(o.M3);
                NPOI.SS.UserModel.ICell cell4 = row.CreateCell(4);
                cell4.CellStyle = style;
                cell4.SetCellValue(o.M4);
                NPOI.SS.UserModel.ICell cell5 = row.CreateCell(5);
                cell5.CellStyle = style;
                cell5.SetCellValue(o.M5);
                NPOI.SS.UserModel.ICell cell6 = row.CreateCell(6);
                cell6.CellStyle = style;
                cell6.SetCellValue(o.M6);
                NPOI.SS.UserModel.ICell cell7 = row.CreateCell(7);
                cell7.CellStyle = style;
                cell7.SetCellValue(o.M7);
                NPOI.SS.UserModel.ICell cell8 = row.CreateCell(8);
                cell8.CellStyle = style;
                cell8.SetCellValue(o.M8);
                NPOI.SS.UserModel.ICell cell9 = row.CreateCell(9);
                cell9.CellStyle = style;
                cell9.SetCellValue(o.M9);
                NPOI.SS.UserModel.ICell cell10 = row.CreateCell(10);
                cell10.CellStyle = style;
                cell10.SetCellValue(o.M10);
                NPOI.SS.UserModel.ICell cell11 = row.CreateCell(11);
                cell11.CellStyle = style;
                cell11.SetCellValue(o.M11);
                NPOI.SS.UserModel.ICell cell12 = row.CreateCell(12);
                cell12.CellStyle = style;
                cell12.SetCellValue(o.M12);
                i++;
            }




            // 写入到客户端  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", System.Web.HttpUtility.UrlEncode("测试人员负责项目数统计", System.Text.Encoding.UTF8)));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }
    }
}