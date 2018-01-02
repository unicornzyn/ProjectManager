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
    public class ServerRule
    {
        public static List<Server> Get()
        {
            List<Server> list = new List<Server>();
            string sql = "SELECT [Id] ,[ServerName],[UserName] ,[Password],[IISVersion],[SqlVersion],[ProjectId],[AddTime],[OSName],[ServerType] FROM [dbo].[Servers]";
           
            DataTable dt = ProjectDB.GetDt(sql);
            List<Project> lp = ProjectRule.Get();
            foreach (DataRow row in dt.Rows)
            {
                Server o = new Server();
                o.Id = St.ToInt32(row["Id"], 0);
                o.ServerName = row["ServerName"].ToString();
                o.UserName = row["UserName"].ToString();
                o.Password = row["Password"].ToString();
                o.IISVersion = row["IISVersion"].ToString();
                o.SqlVersion = row["SqlVersion"].ToString();
                o.ProjectId = row["ProjectId"].ToString();
                o.ProjectName = o.ProjectId==""?"":string.Join(",",lp.Where(a=>Array.IndexOf(o.ProjectId.Split(','),a.Id.ToString())>=0).Select(a=>a.Name).ToArray());
                o.AddTime = St.ToDateTime(row["AddTime"].ToString());
                o.OSName = row["OSName"].ToString();
                o.ServerType = row["ServerType"].ToString();
                list.Add(o);
            }
            return list;
        }

        public static void Delete(int id)
        {
            string sql = @"Delete from Servers where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
        }

        public static void Add(string ServerName, string UserName, string Password, string IISVersion, string SqlVersion, string ProjectId, string OSName, string ServerType)
        {
            string sql = @"INSERT INTO [dbo].[Servers] ([ServerName] ,[UserName] ,[Password] ,[IISVersion] ,[SqlVersion] ,[ProjectId] ,[AddTime],[OSName],[ServerType]) VALUES (@ServerName ,@UserName ,@Password ,@IISVersion ,@SqlVersion ,@ProjectId,GETDATE(),@OSName,@ServerType)";
            SqlParam p = new SqlParam();
            p.AddParam("@ServerName", ServerName, SqlDbType.VarChar, 200);
            p.AddParam("@UserName", UserName, SqlDbType.VarChar, 200);
            p.AddParam("@Password", Password, SqlDbType.VarChar, 200);
            p.AddParam("@IISVersion", IISVersion, SqlDbType.VarChar, 50);
            p.AddParam("@SqlVersion", SqlVersion, SqlDbType.VarChar, 50);
            p.AddParam("@ProjectId", ProjectId, SqlDbType.VarChar, 1000);
            p.AddParam("@OSName", OSName, SqlDbType.VarChar, 100);
            p.AddParam("@ServerType", ServerType, SqlDbType.VarChar, 100);
            ProjectDB.SqlExecute(sql, p);
        }
        public static void Update(int id, string ServerName, string UserName, string Password, string IISVersion, string SqlVersion, string ProjectId, string OSName, string ServerType)
        {
            string sql = @"UPDATE [dbo].[Servers] SET [ServerName] = @ServerName,[UserName] = @UserName,[Password] = @Password,[IISVersion] = @IISVersion,[SqlVersion] = @SqlVersion,[ProjectId] = @ProjectId,[OSName]=@OSName,[ServerType]=@ServerType WHERE id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@ServerName", ServerName, SqlDbType.VarChar, 200);
            p.AddParam("@UserName", UserName, SqlDbType.VarChar, 200);
            p.AddParam("@Password", Password, SqlDbType.VarChar, 200);
            p.AddParam("@IISVersion", IISVersion, SqlDbType.VarChar, 50);
            p.AddParam("@SqlVersion", SqlVersion, SqlDbType.VarChar, 50);
            p.AddParam("@ProjectId", ProjectId, SqlDbType.VarChar, 1000);
            p.AddParam("@OSName", OSName, SqlDbType.VarChar, 100);
            p.AddParam("@ServerType", ServerType, SqlDbType.VarChar, 100);
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
        }
    }
}
