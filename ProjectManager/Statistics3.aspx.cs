using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class Statistics3 : System.Web.UI.Page
    {
        protected string html = "";
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

                selYear.Value = DateTime.Today.Year.ToString();

                BindData();
            }
        }

        private void BindData()
        {
            int year = Common.St.ToInt32(selYear.Value);
            var list = DAL.WorkPlanRule.Get();

            var userlist = DAL.UserRule.Get(); //测试用户列表 





            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 1; i <= 12; i++)
            {
                int c = list.Where(a => a.State == 2 && a.PublishTime.Year == year && a.PublishTime.Month == i).Count(); //上线次数
                int d = list.Where(a => a.State == 2 && a.PublishTime.Year == year && a.PublishTime.Month == i).GroupBy(a => a.ProjectId).Count(); //上线项目数
                string t = year + "-" + (i + 1) + "-1";
                if (i == 12)
                {
                    t = (year + 1) + "-1-1";
                }
                int e = list.Where(a => (a.State == 1 && a.RealStartTime < DateTime.Parse(t)) || (a.State == 2 && (!(a.RealStartTime >= DateTime.Parse(t) || a.RealEndTime < DateTime.Parse(year + "-" + i + "-1"))))).Count(); //工单数 项目数
                int userscount = 0;
                if (year == DateTime.Today.Year && i > DateTime.Today.Month)
                {
                    e = 0;
                    userscount = 0;
                }
                else
                {
                    userscount = userlist.Where(x => x.Status == 1 || (x.Status == 2 && x.LeaveTime.Year * 12 + x.LeaveTime.Month > year * 12 + i)).Count();
                }
                sb.Append("<tr><td>")
                    .Append(GetMonthName(i)).Append("</td><td>")
                    .Append(e).Append("</td><td>")
                    .Append(e).Append("</td><td>")
                    .Append(userscount).Append("</td><td>")
                    .Append(c).Append("</td><td>")
                    .Append(d).Append("</td></tr>");
            }
            html = sb.ToString();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            int year = Common.St.ToInt32(selYear.Value);
            var list = DAL.WorkPlanRule.Get();


            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook(new System.IO.FileStream(Server.MapPath("~/template/template_statistics_3.xls"), System.IO.FileMode.Open, System.IO.FileAccess.Read));
            NPOI.SS.UserModel.ISheet sheet = book.GetSheet("项目月测试次数");

            NPOI.SS.UserModel.ICellStyle style = book.CreateCellStyle();
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.WrapText = true;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.BlueGrey.Index;
            style.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.BlueGrey.Index;
            style.FillPattern = NPOI.SS.UserModel.FillPattern.AltBars;

            NPOI.SS.UserModel.ICellStyle style2 = book.CreateCellStyle();
            style2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style2.WrapText = true;
            style2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            // 内容
            int i = 0;
            int a1, a2, a3, a4, a11 = 0, a22 = 0, a33 = 0, a44 = 0;
            var userlist = DAL.UserRule.Get(); 
            for (int j = 1; j <= 12; j++)
            {
                
                a3 = list.Where(a => a.State == 2 && a.PublishTime.Year == year && a.PublishTime.Month == j).Count(); //上线次数               
                a4 = list.Where(a => a.State == 2 && a.PublishTime.Year == year && a.PublishTime.Month == j).GroupBy(a => a.ProjectId).Count(); //上线项目数
                string t = year + "-" + (j + 1) + "-1";
                if (j == 12)
                {
                    t = (year + 1) + "-1-1";
                }
                a1 = list.Where(a => (a.State == 1 && a.RealStartTime < DateTime.Parse(t)) || (a.State == 2 && (!(a.RealStartTime >= DateTime.Parse(t) || a.RealEndTime < DateTime.Parse(year + "-" + j + "-1"))))).Count(); //工单数 项目数

                if (year == DateTime.Today.Year && j > DateTime.Today.Month)
                {
                    a1 = 0;
                    a2 = 0;
                }
                else 
                {
                    a2 = userlist.Where(a => a.Status == 1 || a.LeaveTime.Year * 12 + a.LeaveTime.Month > year * 12 + j).Count(); //测试用户数
                }

                //第一行
                NPOI.SS.UserModel.IRow row0 = sheet.CreateRow(i++);
                NPOI.SS.UserModel.ICell cell00 = row0.CreateCell(0);
                cell00.CellStyle = style;
                cell00.SetCellValue(GetMonthName(j));
                NPOI.SS.UserModel.ICell cell01 = row0.CreateCell(1);
                cell01.CellStyle = style;
                NPOI.SS.UserModel.ICell cell02 = row0.CreateCell(2);
                cell02.CellStyle = style;
                NPOI.SS.UserModel.ICell cell03 = row0.CreateCell(3);
                cell03.CellStyle = style;
                NPOI.SS.UserModel.ICell cell04 = row0.CreateCell(4);
                cell04.CellStyle = style;
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(i - 1, i - 1, 0, 4));
                //第二行
                NPOI.SS.UserModel.IRow row1 = sheet.CreateRow(i++);
                NPOI.SS.UserModel.ICell cell10 = row1.CreateCell(0);
                cell10.CellStyle = style;
                cell10.SetCellValue("工单数");
                NPOI.SS.UserModel.ICell cell11 = row1.CreateCell(1);
                cell11.CellStyle = style;
                cell11.SetCellValue("项目数");
                NPOI.SS.UserModel.ICell cell12 = row1.CreateCell(2);
                cell12.CellStyle = style;
                cell12.SetCellValue("测试人数");
                NPOI.SS.UserModel.ICell cell13 = row1.CreateCell(3);
                cell13.CellStyle = style;
                cell13.SetCellValue("上线次数");
                NPOI.SS.UserModel.ICell cell14 = row1.CreateCell(4);
                cell14.CellStyle = style;
                cell14.SetCellValue("上线项目数");
                //第三行
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i++);
                NPOI.SS.UserModel.ICell cell20 = row2.CreateCell(0);
                cell20.CellStyle = style2;
                cell20.SetCellValue(a1);
                NPOI.SS.UserModel.ICell cell21 = row2.CreateCell(1);
                cell21.CellStyle = style2;
                cell21.SetCellValue(a1);
                NPOI.SS.UserModel.ICell cell22 = row2.CreateCell(2);
                cell22.CellStyle = style2;
                cell22.SetCellValue(a2);
                NPOI.SS.UserModel.ICell cell23 = row2.CreateCell(3);
                cell23.CellStyle = style2;
                cell23.SetCellValue(a3);
                NPOI.SS.UserModel.ICell cell24 = row2.CreateCell(4);
                cell24.CellStyle = style2;
                cell24.SetCellValue(a4);
                i++;
                a11 += a1; a22 += a2; a33 += a3; a44 += a4;
            }

            //第一行
            NPOI.SS.UserModel.IRow row4 = sheet.CreateRow(i++);
            NPOI.SS.UserModel.ICell cell40 = row4.CreateCell(0);
            cell40.CellStyle = style;
            cell40.SetCellValue(year + "年总和统计");
            NPOI.SS.UserModel.ICell cell41 = row4.CreateCell(1);
            cell41.CellStyle = style;
            NPOI.SS.UserModel.ICell cell42 = row4.CreateCell(2);
            cell42.CellStyle = style;
            NPOI.SS.UserModel.ICell cell43 = row4.CreateCell(3);
            cell43.CellStyle = style;
            NPOI.SS.UserModel.ICell cell44 = row4.CreateCell(4);
            cell44.CellStyle = style;
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(i - 1, i - 1, 0, 4));
            //第二行
            NPOI.SS.UserModel.IRow row5 = sheet.CreateRow(i++);
            NPOI.SS.UserModel.ICell cell50 = row5.CreateCell(0);
            cell50.CellStyle = style;
            cell50.SetCellValue("工单数");
            NPOI.SS.UserModel.ICell cell51 = row5.CreateCell(1);
            cell51.CellStyle = style;
            cell51.SetCellValue("项目数");
            NPOI.SS.UserModel.ICell cell52 = row5.CreateCell(2);
            cell52.CellStyle = style;
            cell52.SetCellValue("测试人数");
            NPOI.SS.UserModel.ICell cell53 = row5.CreateCell(3);
            cell53.CellStyle = style;
            cell53.SetCellValue("上线次数");
            NPOI.SS.UserModel.ICell cell54 = row5.CreateCell(4);
            cell54.CellStyle = style;
            cell54.SetCellValue("上线项目数");
            //第三行

            NPOI.SS.UserModel.IRow row6 = sheet.CreateRow(i++);
            NPOI.SS.UserModel.ICell cell60 = row6.CreateCell(0);
            cell60.CellStyle = style2;
            cell60.SetCellValue(a11);
            NPOI.SS.UserModel.ICell cell61 = row6.CreateCell(1);
            cell61.CellStyle = style2;
            cell61.SetCellValue(a11);
            NPOI.SS.UserModel.ICell cell62 = row6.CreateCell(2);
            cell62.CellStyle = style2;
            cell62.SetCellValue(a22);
            NPOI.SS.UserModel.ICell cell63 = row6.CreateCell(3);
            cell63.CellStyle = style2;
            cell63.SetCellValue(a33);
            NPOI.SS.UserModel.ICell cell64 = row6.CreateCell(4);
            cell64.CellStyle = style2;
            cell64.SetCellValue(a44);


            // 写入到客户端  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", System.Web.HttpUtility.UrlEncode("项目月测试次数", System.Text.Encoding.UTF8)));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }

        private string GetMonthName(int i)
        {
            string name = "";
            switch (i)
            {
                case 1: name = "一月"; break;
                case 2: name = "二月"; break;
                case 3: name = "三月"; break;
                case 4: name = "四月"; break;
                case 5: name = "五月"; break;
                case 6: name = "六月"; break;
                case 7: name = "七月"; break;
                case 8: name = "八月"; break;
                case 9: name = "九月"; break;
                case 10: name = "十月"; break;
                case 11: name = "十一月"; break;
                case 12: name = "十二月"; break;
                default:
                    break;
            }
            return name;
        }
    }
}