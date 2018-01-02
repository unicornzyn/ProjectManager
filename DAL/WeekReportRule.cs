using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Common;
using Common.DB;
using System.Data;


namespace DAL
{
    public class WeekReportRule
    {
        public static List<WeekReport> Get()
        {
            string sql = "SELECT [Id],[ProjectName],[WorkContent],[WorkType],[PlanDateStart],[PlanDateEnd],[PlanDay],[RealDay],[RealWorkContent],[Status],[UserId],[Remark],[AddTime] FROM [dbo].[WeekReports]";
            DataTable dt = ProjectDB.GetDt(sql);

            return GetList(dt);
        }

        private static List<WeekReport> GetList(DataTable dt)
        {
            List<WeekReport> list = new List<WeekReport>();
            List<User> lu = UserRule.Get();
            foreach (DataRow row in dt.Rows)
            {
                WeekReport o = new WeekReport();
                o.Id = St.ToInt32(row["Id"], 0);
                o.ProjectName = row["ProjectName"].ToString();
                o.WorkContent = row["WorkContent"].ToString();
                o.WorkType = St.ToInt32(row["WorkType"], 0);
                o.WorkTypeStr = GetWorkType(o.WorkType);
                o.PlanDateStart = St.ToDateTime(row["PlanDateStart"].ToString());
                o.PlanDateEnd = St.ToDateTime(row["PlanDateEnd"].ToString());
                o.PlanDay = St.ToInt32(row["PlanDay"], 0);
                o.RealDay = St.ToInt32(row["RealDay"], 0);
                o.RealWorkContent = row["RealWorkContent"].ToString();
                o.Status = St.ToInt32(row["Status"], 0);
                o.StatusStr = o.Status == 0 ? "未完成" : "完成";
                o.UserId = St.ToInt32(row["UserId"], 0);
                o.User = o.UserId == 0 ? new User() : lu.FirstOrDefault(a => a.Id == o.UserId);
                o.Remark = row["Remark"].ToString();
                o.AddTime = St.ToDateTime(row["AddTime"].ToString());

                list.Add(o);
            }
            return list;
        }

        private static string GetWorkType(int p)
        {
            string s = "";
            switch (p)
            {
                case 1: s = "会议"; break;
                case 2: s = "测试"; break;
                case 3: s = "编写"; break;
                case 4: s = "跟进"; break;
                case 5: s = "监督"; break;
                default:
                    break;
            }
            return s;
        }
    }
}
