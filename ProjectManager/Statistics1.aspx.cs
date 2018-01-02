using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class Statistics1 : System.Web.UI.Page
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


        protected void btnExport_Click(object sender, EventArgs e)
        {
            var list = DAL.WorkPlanRule.Get().Where(a => a.PublishTime.Year == Common.St.ToInt32(selYear.Value)).Select(a =>
            {
                int pid = 0, id = 0;
                string pname = "", name = "";
                GetIDAndName(a.Project, ref pid, ref id, ref pname, ref name);
                return new { ID_0 = pid, ID_1 = id, Name_0 = pname, Name_1 = name, M = a.PublishTime.Month,PublishTime=a.PublishTime,Name_3=a.Project.Name };
            }).GroupBy(a => a.ID_1).Select(a =>
            {
                return new
                {
                    PName = a.First().Name_0,
                    Name = a.First().Name_1,
                    M1 = a.Where(b => b.M == 1).Count(),
                    M2 = a.Where(b => b.M == 2).Count(),
                    M3 = a.Where(b => b.M == 3).Count(),
                    M4 = a.Where(b => b.M == 4).Count(),
                    M5 = a.Where(b => b.M == 5).Count(),
                    M6 = a.Where(b => b.M == 6).Count(),
                    M7 = a.Where(b => b.M == 7).Count(),
                    M8 = a.Where(b => b.M == 8).Count(),
                    M9 = a.Where(b => b.M == 9).Count(),
                    M10 = a.Where(b => b.M == 10).Count(),
                    M11 = a.Where(b => b.M == 11).Count(),
                    M12 = a.Where(b => b.M == 12).Count(),
                    S1 = string.Join("\n", a.Where(b => b.M == 1).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S2 = string.Join("\n", a.Where(b => b.M == 2).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S3 = string.Join("\n", a.Where(b => b.M == 3).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S4 = string.Join("\n", a.Where(b => b.M == 4).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S5 = string.Join("\n", a.Where(b => b.M == 5).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S6 = string.Join("\n", a.Where(b => b.M == 6).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S7 = string.Join("\n", a.Where(b => b.M == 7).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S8 = string.Join("\n", a.Where(b => b.M == 8).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S9 = string.Join("\n", a.Where(b => b.M == 9).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S10 = string.Join("\n", a.Where(b => b.M == 10).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S11 = string.Join("\n", a.Where(b => b.M == 11).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray()),
                    S12 = string.Join("\n", a.Where(b => b.M == 12).Select(b => b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")").ToArray())
                };
            }).OrderBy(a => a.PName).Where(a => (txtProjectParent.Value.Trim() == "" || a.PName.IndexOf(txtProjectParent.Value.Trim()) >= 0) && (txtProject.Value.Trim() == "" || a.Name.IndexOf(txtProject.Value.Trim()) >= 0));

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook(new System.IO.FileStream(Server.MapPath("~/template/template_statistics_project.xls"), System.IO.FileMode.Open,System.IO.FileAccess.Read));
            NPOI.SS.UserModel.ISheet sheet = book.GetSheet("科教组项目");


            // 内容
            int i = 1;
            foreach (var o in list)
            {
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i);
                NPOI.SS.UserModel.ICell cell0 =row2.CreateCell(0);
                cell0.SetCellValue(o.PName);
                NPOI.SS.UserModel.ICell cell1 = row2.CreateCell(1);
                cell1.SetCellValue(o.Name);
                NPOI.SS.UserModel.ICell cell2 = row2.CreateCell(2);
                cell2.SetCellValue(o.M1);
                NPOI.SS.UserModel.ICell cell3 = row2.CreateCell(3);
                cell3.SetCellValue(o.M2);
                NPOI.SS.UserModel.ICell cell4 = row2.CreateCell(4);
                cell4.SetCellValue(o.M3);
                NPOI.SS.UserModel.ICell cell5 = row2.CreateCell(5);
                cell5.SetCellValue(o.M4);
                NPOI.SS.UserModel.ICell cell6 = row2.CreateCell(6);
                cell6.SetCellValue(o.M5);
                NPOI.SS.UserModel.ICell cell7 = row2.CreateCell(7);
                cell7.SetCellValue(o.M6);
                NPOI.SS.UserModel.ICell cell8 = row2.CreateCell(8);
                cell8.SetCellValue(o.M7);
                NPOI.SS.UserModel.ICell cell9 = row2.CreateCell(9);
                cell9.SetCellValue(o.M8);
                NPOI.SS.UserModel.ICell cell10 = row2.CreateCell(10);
                cell10.SetCellValue(o.M9);
                NPOI.SS.UserModel.ICell cell11 = row2.CreateCell(11);
                cell11.SetCellValue(o.M10);
                NPOI.SS.UserModel.ICell cell12 = row2.CreateCell(12);
                cell12.SetCellValue(o.M11);
                NPOI.SS.UserModel.ICell cell13 = row2.CreateCell(13);
                cell13.SetCellValue(o.M12);
                i++;
            }            
            

            
            NPOI.SS.UserModel.IRow row3 = sheet.CreateRow(i);
            row3.CreateCell(0).SetCellValue("总计");
            row3.CreateCell(1).SetCellValue(list.Sum(a => a.M1 + a.M2 + a.M3 + a.M4 + a.M5 + a.M6 + a.M7 + a.M8 + a.M9 + a.M10 + a.M11 + a.M12));
            row3.CreateCell(2).SetCellValue(list.Sum(a => a.M1));
            row3.CreateCell(3).SetCellValue(list.Sum(a => a.M2));
            row3.CreateCell(4).SetCellValue(list.Sum(a => a.M3));
            row3.CreateCell(5).SetCellValue(list.Sum(a => a.M4));
            row3.CreateCell(6).SetCellValue(list.Sum(a => a.M5));
            row3.CreateCell(7).SetCellValue(list.Sum(a => a.M6));
            row3.CreateCell(8).SetCellValue(list.Sum(a => a.M7));
            row3.CreateCell(9).SetCellValue(list.Sum(a => a.M8));
            row3.CreateCell(10).SetCellValue(list.Sum(a => a.M9));
            row3.CreateCell(11).SetCellValue(list.Sum(a => a.M10));
            row3.CreateCell(12).SetCellValue(list.Sum(a => a.M11));
            row3.CreateCell(13).SetCellValue(list.Sum(a => a.M12));
           


            NPOI.SS.UserModel.ISheet sheet2 = book.GetSheet("上线明细表");


            // 内容
            int j = 1;
            foreach (var o in list)
            {
                NPOI.SS.UserModel.IRow row2 = sheet2.CreateRow(j);
                NPOI.SS.UserModel.ICell cell0 = row2.CreateCell(0);
                cell0.SetCellValue(o.Name);
                NPOI.SS.UserModel.ICell cell1 = row2.CreateCell(1);
                cell1.SetCellValue(o.S1);
                NPOI.SS.UserModel.ICell cell2 = row2.CreateCell(2);
                cell2.SetCellValue(o.S2);
                NPOI.SS.UserModel.ICell cell3 = row2.CreateCell(3);
                cell3.SetCellValue(o.S3);
                NPOI.SS.UserModel.ICell cell4 = row2.CreateCell(4);
                cell4.SetCellValue(o.S4);
                NPOI.SS.UserModel.ICell cell5 = row2.CreateCell(5);
                cell5.SetCellValue(o.S5);
                NPOI.SS.UserModel.ICell cell6 = row2.CreateCell(6);
                cell6.SetCellValue(o.S6);
                NPOI.SS.UserModel.ICell cell7 = row2.CreateCell(7);
                cell7.SetCellValue(o.S7);
                NPOI.SS.UserModel.ICell cell8 = row2.CreateCell(8);
                cell8.SetCellValue(o.S8);
                NPOI.SS.UserModel.ICell cell9 = row2.CreateCell(9);
                cell9.SetCellValue(o.S9);
                NPOI.SS.UserModel.ICell cell10 = row2.CreateCell(10);
                cell10.SetCellValue(o.S10);
                NPOI.SS.UserModel.ICell cell11 = row2.CreateCell(11);
                cell11.SetCellValue(o.S11);
                NPOI.SS.UserModel.ICell cell12 = row2.CreateCell(12);
                cell12.SetCellValue(o.S12);
                j++;
            }         

            // 写入到客户端  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", System.Web.HttpUtility.UrlEncode(selYear.Value + "年项目上线频度表"), System.Text.Encoding.UTF8));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var list = DAL.WorkPlanRule.Get().Where(a => a.PublishTime.Year == Common.St.ToInt32(selYear.Value)).Select(a =>
            {
                int pid = 0, id = 0;
                string pname = "", name = "";
                GetIDAndName(a.Project, ref pid, ref id, ref pname, ref name);
                return new { ID_0 = pid, ID_1 = id, Name_0 = pname, Name_1 = name, M = a.PublishTime.Month, PublishTime = a.PublishTime, Name_3 = a.Project.Name };
            }).GroupBy(a => a.ID_1).Select(a =>
            {
                return new
                {
                    PName = a.First().Name_0,
                    Name = a.First().Name_1,
                    M1 = a.Where(b => b.M == 1).Count(),
                    M2 = a.Where(b => b.M == 2).Count(),
                    M3 = a.Where(b => b.M == 3).Count(),
                    M4 = a.Where(b => b.M == 4).Count(),
                    M5 = a.Where(b => b.M == 5).Count(),
                    M6 = a.Where(b => b.M == 6).Count(),
                    M7 = a.Where(b => b.M == 7).Count(),
                    M8 = a.Where(b => b.M == 8).Count(),
                    M9 = a.Where(b => b.M == 9).Count(),
                    M10 = a.Where(b => b.M == 10).Count(),
                    M11 = a.Where(b => b.M == 11).Count(),
                    M12 = a.Where(b => b.M == 12).Count(),
                    S1 = string.Join("", a.Where(b => b.M == 1).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S2 = string.Join("", a.Where(b => b.M == 2).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S3 = string.Join("", a.Where(b => b.M == 3).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S4 = string.Join("", a.Where(b => b.M == 4).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S5 = string.Join("", a.Where(b => b.M == 5).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S6 = string.Join("", a.Where(b => b.M == 6).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S7 = string.Join("", a.Where(b => b.M == 7).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S8 = string.Join("", a.Where(b => b.M == 8).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S9 = string.Join("", a.Where(b => b.M == 9).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S10 = string.Join("", a.Where(b => b.M == 10).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S11 = string.Join("", a.Where(b => b.M == 11).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray()),
                    S12 = string.Join("", a.Where(b => b.M == 12).Select(b => "<p>" + b.PublishTime.ToString("M.d") + "(" + b.Name_3 + ")</p>").ToArray())
                };
            }).OrderBy(a => a.PName).Where(a => (txtProjectParent.Value.Trim() == "" || a.PName.IndexOf(txtProjectParent.Value.Trim()) >= 0) && (txtProject.Value.Trim() == "" || a.Name.IndexOf(txtProject.Value.Trim()) >= 0));

            rpt.DataSource = list;
            rpt.DataBind();
        }
        private void GetIDAndName(Model.Project p, ref int a, ref int b, ref string c, ref string d)
        {
            if (p.ParentId == 0)
            {
                a = 0; c = "无";
                b = p.Id; d = p.Name;
                return;
            }
            Model.Project np = DAL.ProjectRule.Get().First(x => x.Id == p.ParentId);
            if (np.ParentId == 0)
            {
                a = np.Id; c = DAL.ProjectRule.Get().First(x => x.Id == np.Id).Name;
                b = p.Id; d = p.Name;
                return;
            }
            GetIDAndName(np, ref a, ref b, ref c, ref d);
        }

    }
}