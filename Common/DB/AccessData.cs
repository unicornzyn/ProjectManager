/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：AccessData.cs
// 文件功能描述：数据底层 （从别的项目的移过来的）
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Common.DB
{
    public class BaseData
    {
        public string constr1;
        public BaseData(string constr)
        {
            constr1 = constr;
        }

        #region 返回DataTable对象
        /// <summary>
        /// 传入SQL语句，返回DataTable对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDt(string sql, SqlParam param)
        {
            SqlConnection cn = new SqlConnection(constr1);
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                if (param != null)
                    foreach (SqlParameter p in param.Paramters)
                        cmd.Parameters.Add(p);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message + sql);
            }
            finally
            {
                cn.Close();
            }
        }
        public DataTable GetDt(string sql)
        {
            return GetDt(sql, null);
        }
        #endregion

        #region 执行SQL语句。
        /// <summary>
        /// 执行SQL语句。
        /// </summary>
        /// <param name="sql"></param>
        public int SqlExecute(string sql)
        {
            return SqlExecute(sql, null);
        }
        public int SqlExecute(string sql, SqlParam param)
        {
            SqlConnection cn = new SqlConnection(constr1);
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                if (param != null)
                    foreach (SqlParameter p in param.Paramters)
                        cmd.Parameters.Add(p);
                cmd.CommandTimeout = 300;
                return cmd.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message + sql);
            }
            finally
            {
                cn.Close();
            }
        }
        #endregion

        #region "取出需要查询的第一行第一列"
        /// <summary>
        /// 取出需要查询的第一行第一列
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns></returns>
        public string GetFirst(string sql, SqlParam param)
        {
            SqlConnection myCn = new SqlConnection(constr1);

            string wt = "";
            try
            {
                myCn.Open();
                SqlCommand com = new SqlCommand(sql, myCn);
                if (param != null)
                    foreach (SqlParameter p in param.Paramters)
                        com.Parameters.Add(p);
                object rs = com.ExecuteScalar();
                if (rs != null)
                {
                    wt = rs.ToString();
                    myCn.Close();
                    return wt;
                }
                return wt;
         
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message + sql);
            }
            finally
            {
                myCn.Close();
            }
        }
        public string GetFirst(string sql)
        {
            return GetFirst(sql, null);
        }
 
        #endregion

        #region 执行存储过程
        public DataTable SqlExecuteProcDt(string sql)
        {
            return SqlExecuteProcDt(sql, null);
        }

        public DataTable SqlExecuteProcDt(string sql, SqlParameter[] param)
        {
            SqlConnection cn = new SqlConnection(constr1);
            string wt = String.Empty;
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (param != null)
                    foreach (SqlParameter p in param)
                        cmd.Parameters.Add(p);
                cmd.CommandTimeout = 300;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message + sql);
            }
            finally
            {
                cn.Close();
            }
        }


        public string SqlExecuteProc(string sql)
        {
            return SqlExecuteProc(sql, null);
        }

        public string SqlExecuteProc(string sql, SqlParameter[] param)
        {
            SqlConnection cn = new SqlConnection(constr1);
            string wt = String.Empty;
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (param != null)
                    foreach (SqlParameter p in param)
                        cmd.Parameters.Add(p);
                cmd.CommandTimeout = 300;
                object rs = cmd.ExecuteScalar();
                if (rs != null)
                {
                    wt = rs.ToString();
                    cn.Close();
                    return wt;
                }

                return wt;
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message + sql);
            }
            finally
            {
                cn.Close();
            }
        }

        #endregion
    }


    

}
