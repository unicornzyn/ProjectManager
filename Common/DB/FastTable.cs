using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Common.DB
{
    /// <summary>
    /// 作者：王成志
    /// 日期：2013-03-27
    /// 作用：将DataTable转换成字典：键&DataRow形式。
    /// 以通过键能快速查找到DataRow 
    /// </summary>
    public class FastTable
    {
        Dictionary<string, DataRow> fdt = new Dictionary<string, DataRow>();        //数据字典容器
        private object threadLock = new object();                                   //线程锁
        /// <summary>
        /// 作者：王成志
        /// 日期：2013-03-27
        /// 作用：构造器
        /// </summary>
        /// <param name="dt">需要快速访问的表</param>
        /// <param name="keyName">作为键的列名称。注意如果该列有重复值，只会加入重复值的第一个主键所对应的DataRow
        /// </param>
        public FastTable(DataTable dt, string keyName)
        {
            if (dt.Columns.Contains(keyName))
            {
                string key = "";
                foreach (DataRow row in dt.Rows)
                {
                    key = row[keyName].ToString().Trim();
                    if (!fdt.ContainsKey(key))
                    {
                        fdt.Add(key, row);
                    }
                }
            }
        }
        /// <summary>
        /// 作者：王成志
        /// 日期：2013-03-27
        /// 作用：通过主键获取DataRow。
        /// </summary>
        /// <param name="_Id">主键，字符型</param>
        /// <returns></returns>
        public DataRow GetRow(string _Id)
        {
            string Id = _Id.Trim();
            if (!fdt.ContainsKey(Id))
            {
                return null;
            }
            return fdt[Id];
        }
    }
}
