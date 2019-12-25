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
    public class WorkPlanRule
    {
        public static List<WorkPlan> Get()
        {
            string sql = "select Id,SheepNo,ProjectId,WorkRemark,PlanType,StartTime,EndTime,RealStartTime,RealEndTime,PublishTime,State,NeederId,Remark,AddTime,Tester,Dever,FilePath,SecretScanTime,SecretScanCount from WorkPlans order by Id desc";
            DataTable dt = ProjectDB.GetDt(sql);

            return GetList(dt);
        }
        /// <summary>
        /// 根据上线时间查询所有已完成计划
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<WorkPlan> Get(DateTime start, DateTime end)
        {
            string sql = "select Id,SheepNo,ProjectId,WorkRemark,PlanType,StartTime,EndTime,RealStartTime,RealEndTime,PublishTime,State,NeederId,Remark,AddTime,Tester,Dever,FilePath,SecretScanTime,SecretScanCount from WorkPlans where State=2 and PublishTime between @dtstart and @dtend order by PublishTime";
            SqlParam p = new SqlParam();
            p.AddParam("@dtstart", start, SqlDbType.DateTime, 0);
            p.AddParam("@dtend", end, SqlDbType.DateTime, 0);
            DataTable dt = ProjectDB.GetDt(sql, p);
            return GetList(dt);
        }
        private static List<WorkPlan> GetList(DataTable dt)
        {
            List<WorkPlan> list = new List<WorkPlan>();
            List<Project> lp = ProjectRule.Get();
            List<User> lu = UserRule.Get();
            foreach (DataRow row in dt.Rows)
            {
                WorkPlan o = new WorkPlan();
                o.Id = St.ToInt32(row["Id"], 0);
                o.SheepNo = row["SheepNo"].ToString();
                o.ProjectId = St.ToInt32(row["ProjectId"], 0);
                o.Project = lp.FirstOrDefault(a => a.Id == o.ProjectId);
                o.WorkRemark = row["WorkRemark"].ToString();
                o.PlanType = St.ToInt32(row["PlanType"], 0);
                o.PlanTypeStr = GetPlanType(o.PlanType);
                o.StartTime = St.ToDateTime(row["StartTime"].ToString());
                o.EndTime = St.ToDateTime(row["EndTime"].ToString());
                o.RealStartTime = St.ToDateTime(row["RealStartTime"].ToString());
                o.RealEndTime = St.ToDateTime(row["RealEndTime"].ToString());
                o.PublishTime = St.ToDateTime(row["PublishTime"].ToString());
                o.State = St.ToInt32(row["State"], 0);
                o.StateStr = GetState(o.State);
                o.NeederId = St.ToInt32(row["NeederId"], 0);
                o.Needer = o.NeederId == 0 ? new User() : lu.FirstOrDefault(a => a.Id == o.NeederId);
                o.Remark = row["Remark"].ToString();
                o.AddTime = St.ToDateTime(row["AddTime"].ToString());
                o.Tester = row["Tester"].ToString();
                o.Dever = row["Dever"].ToString();
                o.FilePath = row["FilePath"].ToString();
                o.SecretScanTime = St.ToDateTime(row["SecretScanTime"].ToString());
                o.SecretScanCount = St.ToInt32(row["SecretScanCount"], 0);
                list.Add(o);
            }
            return list;
        }

        public static void Add(int id, string SheepNo, int ProjectId, string WorkRemark, int PlanType, DateTime StartTime, DateTime EndTime, DateTime RealStartTime, DateTime RealEndTime, DateTime PublishTime, int State, int NeederId, string Remark, int userid,string tester,string dever,string FilePath,DateTime SecretScanTime, int SecretScanCount)
        {
            string sql = @" if exists(select Id from WorkPlans where Id=@Id)
	                            update WorkPlans set SheepNo=@SheepNo,ProjectId=@ProjectId,WorkRemark=@WorkRemark,PlanType=@PlanType,StartTime=@StartTime,EndTime=@EndTime,RealStartTime=@RealStartTime,RealEndTime=@RealEndTime,PublishTime=@PublishTime,State=@State,NeederId=@NeederId,Remark=@Remark,LastModifyUser=@userid,LastModifyTime=getdate(),Tester=@Tester,Dever=@Dever,FilePath=@FilePath,SecretScanTime=@SecretScanTime,SecretScanCount=@SecretScanCount where Id=@Id
                            else
	                            insert into WorkPlans(SheepNo,ProjectId,WorkRemark,PlanType,StartTime,EndTime,RealStartTime,RealEndTime,PublishTime,State,NeederId,Remark,LastModifyUser,LastModifyTime,Tester,Dever,FilePath,SecretScanTime,SecretScanCount) values(@SheepNo,@ProjectId,@WorkRemark,@PlanType,@StartTime,@EndTime,@RealStartTime,@RealEndTime,@PublishTime,@State,@NeederId,@Remark,@userid,getdate(),@Tester,@Dever,@FilePath,@SecretScanTime,@SecretScanCount)";
            SqlParam p = new SqlParam();
            p.AddParam("@Id", id, SqlDbType.Int, 0);
            p.AddParam("@SheepNo", SheepNo, SqlDbType.VarChar, 200);
            p.AddParam("@ProjectId", ProjectId, SqlDbType.Int, 0);
            p.AddParam("@WorkRemark", WorkRemark, SqlDbType.VarChar, 1000);
            p.AddParam("@PlanType", PlanType, SqlDbType.Int, 0);
            p.AddParam("@StartTime", StartTime, SqlDbType.DateTime, 0);
            p.AddParam("@EndTime", EndTime, SqlDbType.DateTime, 0);
            p.AddParam("@RealStartTime", RealStartTime, SqlDbType.DateTime, 0);
            p.AddParam("@RealEndTime", RealEndTime, SqlDbType.DateTime, 0);
            p.AddParam("@PublishTime", PublishTime, SqlDbType.DateTime, 0);
            p.AddParam("@State", State, SqlDbType.Int, 0);
            p.AddParam("@NeederId", NeederId, SqlDbType.Int, 0);
            p.AddParam("@Remark", Remark, SqlDbType.NVarChar, 4000);
            p.AddParam("@userid", userid, SqlDbType.Int, 0);
            p.AddParam("@Tester", tester, SqlDbType.VarChar, 500);
            p.AddParam("@Dever", dever, SqlDbType.VarChar, 500);
            p.AddParam("@FilePath", FilePath, SqlDbType.VarChar, 200);
            p.AddParam("@SecretScanTime", RealEndTime, SqlDbType.DateTime, 0);
            p.AddParam("@SecretScanCount", SecretScanCount, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
        }

        public static void Delete(int id)
        {
            string sql = @"Delete from WorkPlans where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
        }

        private static string GetPlanType(int i)
        {
            if (i == 1)
            {
                return "正常";
            }
            else if (i == 2)
            {
                return "插入";
            }
            else
            {
                return "";
            }
        }

        private static string GetState(int i)
        {
            if (i == 0)
            {
                return "未开始";
            }
            else if (i == 1)
            {
                return "进行中";
            }
            else if (i == 2)
            {
                return "完成";
            }
            else if (i == 3)
            {
                return "延期";
            }
            else if (i == 4)
            {
                return "测试完成";
            }
            else
            {
                return "";
            }
        }
    }
}
