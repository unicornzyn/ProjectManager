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
    public class ProjectRule
    {
        public static List<Project> Get()
        {
            List<Project> list = UnicornCache.Get<List<Project>>(CacheKey.Project);

            if (list==null)
            {
                list = new List<Project>();
                string sql = "select Id,Name,ParentId,Url,TestUrl,SiteFileName,DatabaseName,TestUserName,TestPassword,Remark,IsShow from Projects order by Id desc";
                DataTable dt = ProjectDB.GetDt(sql);

                foreach (DataRow row in dt.Rows)
                {
                    Project o = new Project();
                    o.Id = St.ToInt32(row["Id"], 0);
                    o.Name = row["Name"].ToString();
                    o.Url = row["Url"].ToString();
                    o.ParentId = St.ToInt32(row["ParentId"], 0);
                    o.TestUrl = row["TestUrl"].ToString();
                    o.SiteFileName = row["SiteFileName"].ToString();                   
                    o.DatabaseName = row["DatabaseName"].ToString();
                    o.TestUserName = row["TestUserName"].ToString();
                    o.TestPassword = row["TestPassword"].ToString();
                    o.Remark = row["Remark"].ToString();
                    o.IsShow = St.ToInt32(row["IsShow"], 0);
                    list.Add(o);
                }
                UnicornCache.Add(CacheKey.Project, list);
            }
           
            return list;
        }

        public static void Add(int id, string name, string url, int parentid, string TestUrl, string SiteFileName, string DatabaseName, string TestUserName, string TestPassword, string Remark, int IsShow)
        {
            string sql = @" if exists(select Id from Projects where Id=@Id)
	                            update Projects set Name=@Name,ParentId=@ParentId,Url=@Url,TestUrl=@TestUrl,SiteFileName=@SiteFileName,DatabaseName=@DatabaseName,TestUserName=@TestUserName,TestPassword=@TestPassword,Remark=@Remark,IsShow=@IsShow where Id=@Id
                            else
	                            insert into Projects(Name,ParentId,Url,TestUrl,SiteFileName,DatabaseName,TestUserName,TestPassword,Remark,IsShow) values(@Name,@ParentId,@Url,@TestUrl,@SiteFileName,@DatabaseName,@TestUserName,@TestPassword,@Remark,@IsShow)";
            SqlParam p = new SqlParam();
            p.AddParam("@Id", id, SqlDbType.Int, 0);
            p.AddParam("@Name", name, SqlDbType.VarChar, 500);
            p.AddParam("@ParentId", parentid, SqlDbType.Int, 0);
            p.AddParam("@Url", url, SqlDbType.VarChar, 500);
            p.AddParam("@TestUrl", TestUrl, SqlDbType.VarChar, 500);
            p.AddParam("@SiteFileName", SiteFileName, SqlDbType.VarChar, 500);
            p.AddParam("@DatabaseName", DatabaseName, SqlDbType.VarChar, 500);
            p.AddParam("@TestUserName", TestUserName, SqlDbType.VarChar, 200);
            p.AddParam("@TestPassword", TestPassword, SqlDbType.VarChar, 200);
            p.AddParam("@Remark", Remark, SqlDbType.VarChar, 500);
            p.AddParam("@IsShow", IsShow, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
            UnicornCache.Remove(CacheKey.Project);
        }

        public static void Delete(int id)
        {
            string sql = @"Delete from Projects where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
            UnicornCache.Remove(CacheKey.Project);
        }
    }
}
