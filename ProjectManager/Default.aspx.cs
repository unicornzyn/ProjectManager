using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManager
{
    public partial class Default : System.Web.UI.Page
    {
        protected static int currpage = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptPorject.DataSource = DAL.ProjectRule.Get();
                rptPorject.DataBind();

                var neederlist = DAL.UserRule.Get().Where(a=>a.Status==1);
                NeederId.DataSource = neederlist;
                NeederId.DataTextField = "RealName";
                NeederId.DataValueField = "Id";
                NeederId.DataBind();
                NeederId.Items.Insert(0, new ListItem("", "0"));

                selNeeder.DataSource = neederlist;
                selNeeder.DataTextField = "RealName";
                selNeeder.DataValueField = "Id";
                selNeeder.DataBind();
                selNeeder.Items.Insert(0,new ListItem("全部", "-1"));
                

                List<int> list = new List<int>();
                for (int i = 2015; i <= DateTime.Now.Year; i++)
                {
                    list.Add(i);
                }
                selYear.DataSource = list;
                selYear.DataBind();

                selYear.Value = DateTime.Now.Year.ToString();
                selMonth.Value = DateTime.Now.Month.ToString();

                BindData(1);
            }
        }

        private void BindData(int page)
        {
            var list = GetData();
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

        private List<Model.WorkPlan> GetData()
        {
            int year = Common.St.ToInt32(selYear.Value);
            int month = Common.St.ToInt32(selMonth.Value);
            var list = DAL.WorkPlanRule.Get()
                .Where(a => txtProjectSearch.Value.Trim() == "" || a.Project.Name.IndexOf(txtProjectSearch.Value.Trim()) >= 0)
                .Where(a => (selNeeder.Value == "-1" || a.NeederId == Common.St.ToInt32(selNeeder.Value)) && (selWorkState.Value == "-1" || a.State == Common.St.ToInt32(selWorkState.Value)))
                .Where(a =>
            {
                if (a.State == 0)
                {
                    if (a.AddTime.Year == year)
                    {
                        if (month > 0)
                        {
                            if (a.AddTime.Month <= month)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }

                    }
                    else if (a.AddTime.Year < year)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (a.State == 1)
                {
                    if (a.RealStartTime.Year == year)
                    {
                        if (month > 0)
                        {
                            if (a.RealStartTime.Month <= month)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (a.RealStartTime.Year < year)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (a.State == 2)
                {
                    if (a.RealStartTime.Year == year)
                    {
                        if (month > 0)
                        {
                            if (a.RealStartTime.Month == month)
                            {
                                return true;
                            }
                            else if (a.RealStartTime.Month < month && a.RealEndTime.Month >= month)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (a.RealStartTime.Year < year && a.RealEndTime.Year > year)
                    {
                        return true;
                    }
                    else if (a.RealStartTime.Year < year && a.RealEndTime.Year == year)
                    {
                        if (month > 0)
                        {
                            if (a.RealEndTime.Month >= month)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }).ToList();
            return list;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (((User)Session["user"]).RoleType < 3)
            {
                DAL.WorkPlanRule.Add(Common.St.ToInt32(hId.Value), SheepNo.Value, Common.St.ToInt32(ProjectId.Value), WorkRemark.Value, Common.St.ToInt32(PlanType.Value), Common.St.ToDateTime(StartTime.Value), Common.St.ToDateTime(EndTime.Value), Common.St.ToDateTime(RealStartTime.Value), Common.St.ToDateTime(RealEndTime.Value), Common.St.ToDateTime(PublishTime.Value), Common.St.ToInt32(State.Value), Common.St.ToInt32(NeederId.Value), Remark.Value, ((User)Session["user"]).Id);
            }

            BindData(currpage);
        }

        protected void rpt_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteWorkPlan")
            {
                if (((User)Session["user"]).RoleType < 3)
                {
                    DAL.WorkPlanRule.Delete(Common.St.ToInt32(e.CommandArgument));
                }
                BindData(currpage);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //http://www.itnose.net/detail/476834.html
            //http://blog.csdn.net/gjban/article/details/39030669

            var list = GetData();

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook(new System.IO.FileStream(Server.MapPath("~/template/template_workplan.xls"), System.IO.FileMode.Open, System.IO.FileAccess.Read));
            NPOI.SS.UserModel.ISheet sheet = book.GetSheet("工作计划表");

            NPOI.SS.UserModel.ICellStyle style = book.CreateCellStyle();
            style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style.WrapText = true;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            // 内容
            int i = 3;
            foreach (var o in list)
            {
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i);
                NPOI.SS.UserModel.ICell cell0 = row2.CreateCell(0);
                cell0.CellStyle = style;
                cell0.SetCellValue(i - 2);
                NPOI.SS.UserModel.ICell cell1 = row2.CreateCell(1);
                cell1.CellStyle = style;
                cell1.SetCellValue(o.SheepNo);
                NPOI.SS.UserModel.ICell cell2 = row2.CreateCell(2);
                cell2.CellStyle = style;
                cell2.SetCellValue(o.Project.Name);
                NPOI.SS.UserModel.ICell cell3 = row2.CreateCell(3);
                cell3.CellStyle = style;
                cell3.SetCellValue(o.WorkRemark);
                NPOI.SS.UserModel.ICell cell4 = row2.CreateCell(4);
                cell4.CellStyle = style;
                cell4.SetCellValue(o.PlanTypeStr);
                NPOI.SS.UserModel.ICell cell5 = row2.CreateCell(5);
                cell5.CellStyle = style;
                cell5.SetCellValue(Common.St.ToDateTimeString(o.StartTime, "yyyy-MM-dd"));
                NPOI.SS.UserModel.ICell cell6 = row2.CreateCell(6);
                cell6.CellStyle = style;
                cell6.SetCellValue(Common.St.ToDateTimeString(o.RealStartTime, "yyyy-MM-dd"));
                NPOI.SS.UserModel.ICell cell7 = row2.CreateCell(7);
                cell7.CellStyle = style;
                cell7.SetCellValue(Common.St.ToDateTimeString(o.EndTime, "yyyy-MM-dd"));
                NPOI.SS.UserModel.ICell cell8 = row2.CreateCell(8);
                cell8.CellStyle = style;
                cell8.SetCellValue(Common.St.ToDateTimeString(o.RealEndTime, "yyyy-MM-dd"));
                NPOI.SS.UserModel.ICell cell9 = row2.CreateCell(9);
                cell9.CellStyle = style;
                cell9.SetCellValue(o.StateStr);
                NPOI.SS.UserModel.ICell cell10 = row2.CreateCell(10);
                cell10.CellStyle = style;
                cell10.SetCellValue(o.Needer.RealName);
                NPOI.SS.UserModel.ICell cell11 = row2.CreateCell(11);
                cell11.CellStyle = style;
                cell11.SetCellValue(o.Remark);
                i++;
            }

            // 写入到客户端  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", System.Web.HttpUtility.UrlEncode("工作计划表", System.Text.Encoding.UTF8)));
            Response.BinaryWrite(ms.ToArray());
            book = null;
            ms.Close();
            ms.Dispose();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(1);
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


        protected string GetTrClass(string state)
        {
            if (state=="0")
            {
                return "success";
            }
            else if (state == "1")
            {
                return "info";
            }
            else if (state == "2")
            {
                return "";
            }
            else
            {
                return "";
            }
        }

    }
}