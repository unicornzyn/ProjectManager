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
    public class UserRule
    {
        public static User Get(string username)
        {
            string sql = "select Id,UserName,Password,RealName,RoleType,Status from Users where UserName=@UserName";
            SqlParam p = new SqlParam();
            p.AddParam("@UserName", username, SqlDbType.VarChar, 100);
            DataTable dt = ProjectDB.GetDt(sql, p);
            if (St.CheckDt(dt))
            {
                User o = new User();
                o.Id = St.ToInt32(dt.Rows[0]["Id"], 0);
                o.UserName = dt.Rows[0]["UserName"].ToString();
                o.Password = dt.Rows[0]["Password"].ToString();
                o.RealName = dt.Rows[0]["RealName"].ToString();
                o.RoleType = St.ToInt32(dt.Rows[0]["RoleType"], 0);
                o.Status = St.ToInt32(dt.Rows[0]["Status"], 0);
                return o;
            }
            else
            {
                return new User();
            }
        }

        public static List<User> Get()
        {
            List<User> list = UnicornCache.Get<List<User>>(CacheKey.User);
            if (list==null)
            {
                list = new List<User>();
                string sql = "select Id,UserName,Password,RealName,RoleType,Status,LeaveTime from Users";
                DataTable dt = ProjectDB.GetDt(sql);
                foreach (DataRow row in dt.Rows)
                {
                    User o = new User();
                    o.Id = St.ToInt32(row["Id"], 0);
                    o.UserName = row["UserName"].ToString();
                    o.Password = row["Password"].ToString();
                    o.RealName = row["RealName"].ToString();
                    o.RoleType = St.ToInt32(row["RoleType"], 0);
                    o.Status = St.ToInt32(row["Status"], 0);
                    o.LeaveTime = St.ToDateTime(row["LeaveTime"].ToString());
                    list.Add(o);
                }
                UnicornCache.Add(CacheKey.User, list);
            }
            
            return list;
        }

        public static void Add(string username, string realname,int userrole,DateTime leavetime)
        {
            string sql = @" if exists(select Id from Users where UserName=@UserName)
	                            update Users set RealName=@RealName,RoleType=@RoleType,LeaveTime=@LeaveTime where UserName=@UserName
                            else
	                            insert into Users(UserName,Password,RealName,RoleType,Status,LeaveTime) values(@UserName,@Password,@RealName,@RoleType,1,@LeaveTime)";
            SqlParam p = new SqlParam();
            p.AddParam("@UserName", username, SqlDbType.VarChar, 100);
            p.AddParam("@RealName", realname, SqlDbType.VarChar, 500);
            p.AddParam("@RoleType", userrole, SqlDbType.Int, 0);
            p.AddParam("@Password", St.GetMd5(username + "123456"), SqlDbType.VarChar, 100);
            p.AddParam("@LeaveTime", leavetime, SqlDbType.DateTime, 0);
            ProjectDB.SqlExecute(sql, p);
            UnicornCache.Remove(CacheKey.User);
        }

        public static void Delete(int id)
        {
            string sql = @"Delete from Users where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);            
            ProjectDB.SqlExecute(sql, p);
            UnicornCache.Remove(CacheKey.User);
        }

        public static void Update(int id)
        {
            string sql = @"update Users set [Status]=[Status]%2+1 where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
            UnicornCache.Remove(CacheKey.User);
        }

        public static void Update(int id, string username,string password)
        {
            string sql = @"update Users set Password=@Password where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            p.AddParam("@Password", St.GetMd5(username + password), SqlDbType.VarChar, 100);
            ProjectDB.SqlExecute(sql, p);
            UnicornCache.Remove(CacheKey.User);
        }
    }
}
