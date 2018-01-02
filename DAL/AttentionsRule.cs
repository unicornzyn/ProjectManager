using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;
using Common.DB;
using Common;

namespace DAL
{
    public class AttentionsRule
    {
        public static List<Attentions> Get()
        {
            List<Attentions> list = new List<Attentions>();
            string sql = "SELECT [Id] ,[Remark],[ProjectId],[AddTime] FROM [dbo].[Attentions]";

            DataTable dt = ProjectDB.GetDt(sql);
            List<Project> lp = ProjectRule.Get();
            foreach (DataRow row in dt.Rows)
            {
                Attentions o = new Attentions();
                o.Id = St.ToInt32(row["Id"], 0);
                o.Remark = row["Remark"].ToString();          
                o.ProjectId = row["ProjectId"].ToString();
                o.ProjectName = o.ProjectId == "" ? "" : string.Join(",", lp.Where(a => Array.IndexOf(o.ProjectId.Split(','), a.Id.ToString()) >= 0).Select(a => a.Name).ToArray());
                o.AddTime = St.ToDateTime(row["AddTime"].ToString());               
                list.Add(o);
            }
            return list;
        }

        public static void Delete(int id)
        {
            string sql = @"Delete from Attentions where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
        }

        public static void Add(string Remark, string ProjectId)
        {
            string sql = @"INSERT INTO [dbo].[Attentions] ([Remark] ,[ProjectId]) VALUES (@Remark ,@ProjectId)";
            SqlParam p = new SqlParam();
            p.AddParam("@Remark", Remark, SqlDbType.VarChar, 2000);         
            p.AddParam("@ProjectId", ProjectId, SqlDbType.VarChar, 1000);            
            ProjectDB.SqlExecute(sql, p);
        }
        public static void Update(int id, string Remark, string ProjectId)
        {
            string sql = @"UPDATE [dbo].[Attentions] SET [Remark] = @Remark,[ProjectId] = @ProjectId WHERE id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@Remark", Remark, SqlDbType.VarChar, 2000);
            p.AddParam("@ProjectId", ProjectId, SqlDbType.VarChar, 1000);         
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
        }
    }
}
