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
    public class DicRule
    {        

        public static List<Dic> Get()
        {
            List<Dic> list = UnicornCache.Get<List<Dic>>(CacheKey.Dic);
            if (list == null)
            {
                list = new List<Dic>();
                string sql = "select Id,Code,Name,Type,Remark,AddTime from Dics";
                DataTable dt = ProjectDB.GetDt(sql);
                foreach (DataRow row in dt.Rows)
                {
                    Dic o = new Dic();
                    o.Id = St.ToInt32(row["Id"], 0);
                    o.Code = row["Code"].ToString();
                    o.Name = row["Name"].ToString();
                    o.Type = St.ToInt32(row["Type"].ToString(), 0);
                    o.Remark = row["Remark"].ToString();
                    o.AddTime = St.ToDateTime(row["AddTime"].ToString());
                    list.Add(o);
                }
                UnicornCache.Add(CacheKey.Dic, list);
            }

            return list;
        }

        public static void Add(string code, string name, int type,string remark)
        {
            string sql = @"insert into Dics(Code,Name,Type,Remark) values(@Code,@Name,@Type,@Remark)";
            SqlParam p = new SqlParam();
            p.AddParam("@Code", code, SqlDbType.VarChar, 50);
            p.AddParam("@Name", name, SqlDbType.VarChar, 50);
            p.AddParam("@Type", type, SqlDbType.Int, 0);
            p.AddParam("@Remark", remark, SqlDbType.VarChar, 50);
            ProjectDB.SqlExecute(sql, p);
            UnicornCache.Remove(CacheKey.Dic);
        }

        public static void Delete(int id)
        {
            string sql = @"Delete from Dics where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
            UnicornCache.Remove(CacheKey.Dic);
        }

        public static void Update(int id, string code, string name, int type, string remark)
        {
            string sql = @"update Dics set Code=@Code,Name=@Name,Type=@Type,Remark=@Remark where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            p.AddParam("@Code", code, SqlDbType.VarChar, 50);
            p.AddParam("@Name", name, SqlDbType.VarChar, 50);
            p.AddParam("@Type", type, SqlDbType.Int, 0);
            p.AddParam("@Remark", remark, SqlDbType.VarChar, 50);
            ProjectDB.SqlExecute(sql, p);
            UnicornCache.Remove(CacheKey.Dic);
        }
    }
}
