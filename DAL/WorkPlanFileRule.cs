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
    public class WorkPlanFileRule
    {
        public static List<WorkPlanFile> GetList(int workplanid)
        {
            string sql = "select Id,WorkPlanId,FileName,FilePath from WorkPlanFiles where WorkPlanId = @workplanid order by Id desc";
            SqlParam p = new SqlParam();
            p.AddParam("@workplanid", workplanid, SqlDbType.Int, 0);
            DataTable dt = ProjectDB.GetDt(sql,p);

            List<WorkPlanFile> list = new List<WorkPlanFile>();
            foreach (DataRow row in dt.Rows)
            {
                WorkPlanFile o = new WorkPlanFile();
                o.Id = St.ToInt32(row["Id"], 0);
                o.WorkPlanId = St.ToInt32(row["WorkPlanId"], 0);
                o.FileName = row["FileName"].ToString();
                o.FilePath = row["FilePath"].ToString();
                list.Add(o);
            }
            return list;
        }

        public static WorkPlanFile Get(int id)
        {
            string sql = "select Id,WorkPlanId,FileName,FilePath from WorkPlanFiles where Id = @Id";
            SqlParam p = new SqlParam();
            p.AddParam("@Id", id, SqlDbType.Int, 0);
            DataTable dt = ProjectDB.GetDt(sql, p);

            WorkPlanFile o = new WorkPlanFile();

            if (dt.Rows.Count>0)
            {
                var row = dt.Rows[0];
                
                o.Id = St.ToInt32(row["Id"], 0);
                o.WorkPlanId = St.ToInt32(row["WorkPlanId"], 0);
                o.FileName = row["FileName"].ToString();
                o.FilePath = row["FilePath"].ToString();
                
            }
            return o;
        }

        public static void Add(int WorkPlanId,string FileName,string FilePath)
        {
            string sql = @"INSERT INTO [dbo].[WorkPlanFiles] ([WorkPlanId] ,[FileName], [FilePath], [AddTime]) VALUES (@WorkPlanId ,@FileName, @FilePath, GetDate())";
            SqlParam p = new SqlParam();
            p.AddParam("@WorkPlanId", WorkPlanId, SqlDbType.Int, 0);
            p.AddParam("@FileName", FileName, SqlDbType.VarChar, 200);
            p.AddParam("@FilePath", FilePath, SqlDbType.VarChar, 200);
            ProjectDB.SqlExecute(sql, p);
        }

        public static void Delete(int id)
        {
            string sql = @"Delete from WorkPlanFiles where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
        }
    }
}
