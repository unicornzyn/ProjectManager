using Common;
using Common.DB;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ProjectFilesRule
    {
        public static List<ProjectFile> GetList(int projectid)
        {
            string sql = "select Id,ProjectId,FileName,FilePath from ProjectFiles where ProjectId = @id order by Id desc";
            SqlParam p = new SqlParam();
            p.AddParam("@id", projectid, SqlDbType.Int, 0);
            DataTable dt = ProjectDB.GetDt(sql, p);

            List<ProjectFile> list = new List<ProjectFile>();
            foreach (DataRow row in dt.Rows)
            {
                ProjectFile o = new ProjectFile();
                o.Id = St.ToInt32(row["Id"], 0);
                o.ProjectId = St.ToInt32(row["ProjectId"], 0);
                o.FileName = row["FileName"].ToString();
                o.FilePath = row["FilePath"].ToString();
                list.Add(o);
            }
            return list;
        }

        public static ProjectFile Get(int id)
        {
            string sql = "select Id,ProjectId,FileName,FilePath from ProjectFiles where Id = @Id";
            SqlParam p = new SqlParam();
            p.AddParam("@Id", id, SqlDbType.Int, 0);
            DataTable dt = ProjectDB.GetDt(sql, p);

            ProjectFile o = new ProjectFile();

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];

                o.Id = St.ToInt32(row["Id"], 0);
                o.ProjectId = St.ToInt32(row["ProjectId"], 0);
                o.FileName = row["FileName"].ToString();
                o.FilePath = row["FilePath"].ToString();

            }
            return o;
        }

        public static void Add(int ProjectId, string FileName, string FilePath)
        {
            string sql = @"INSERT INTO [dbo].[ProjectFiles] ([ProjectId] ,[FileName], [FilePath], [AddTime]) VALUES (@ProjectId ,@FileName, @FilePath, GetDate())";
            SqlParam p = new SqlParam();
            p.AddParam("@ProjectId", ProjectId, SqlDbType.Int, 0);
            p.AddParam("@FileName", FileName, SqlDbType.VarChar, 200);
            p.AddParam("@FilePath", FilePath, SqlDbType.VarChar, 200);
            ProjectDB.SqlExecute(sql, p);
        }

        public static void Delete(int id)
        {
            string sql = @"Delete from ProjectFiles where Id=@id";
            SqlParam p = new SqlParam();
            p.AddParam("@id", id, SqlDbType.Int, 0);
            ProjectDB.SqlExecute(sql, p);
        }
    }
}
