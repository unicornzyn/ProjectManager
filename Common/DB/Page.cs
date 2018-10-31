/*----------------------------------------------------------------
// Copyright (C) 2010 ʢ�ش�ý 
// �ļ�����Page.cs
// �ļ��������������ݵײ� ����ҳ�����ӱ����Ŀ���ƹ����ģ�
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Common.DB;
using Common;
namespace Common.DB
{
    public class Page
    {
        private BaseData AccessData;
        private string newstr = string.Empty;
        public Page(string str)
        {
            newstr = str;
            AccessData = new BaseData(newstr);
        }
        public Page()
        {
            newstr = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ProductDBSensis"]); ;
            AccessData = new BaseData(newstr);
        }


        /// <summary>
        /// ��ȡ��������Χ�ڵ����м�¼��
        /// </summary>
        /// <param name="TableStr"></param>
        /// <param name="WhereStr"></param>
        /// <returns></returns>
        private int getAll(string TableStr, string WhereStr, SqlParam param)
        {
            return St.ToInt32(AccessData.GetFirst("select count(1) from " + TableStr + (string.IsNullOrEmpty(WhereStr) ? "" : " where ") + WhereStr, param), 0);
        }
        /// <summary>
        /// ������������ķ�ҳ��
        /// Ӧ�Ժ������ݷ�ҳ����Դ������ҳ�������޹ء�
        /// </summary>
        /// <param name="SelectCols">Ҫѡ����ֶΣ���������������</param>
        /// <param name="TableStr"></param>
        /// <param name="WhereStr"></param>
        /// <param name="Key"></param>
        /// <param name="Index"></param>
        /// <param name="PageSize"></param>
        /// <param name="IsDesc"></param>
        /// <returns></returns>
        public PageInfo GetPage(string SelectCols, string TableStr, string WhereStr, string Key, Int32 Index, Int16 PageSize, bool IsDesc, SqlParam param)
        {
            int all = getAll(TableStr, WhereStr, param.Copy());
            string saftwhere = (string.IsNullOrEmpty(WhereStr) ? "" : " where " + WhereStr);
            string saftandwhere = (string.IsNullOrEmpty(WhereStr) ? "" : " and " + WhereStr);
            int _index = getIndex(all, Index, PageSize);

            if (PageSize < 1)
                return new PageInfo(all, _index, new DataTable(), PageSize);
            StringBuilder sql = new StringBuilder();
            if (_index == 1)
            {
                sql.Append("select top " + PageSize + " " + SelectCols + " from " + TableStr + saftwhere + " order by " + Key + " " + (IsDesc ? "desc" : ""));
            }
            else
            {
                string tempkey = getTempKey(Key, "temptable");
                if (IsDesc)
                {
                    //����
                    sql.Append("select top " + PageSize + " " + SelectCols + " \r\n");
                    sql.Append("from " + TableStr + " \r\n");
                    sql.Append("where " + Key + "< \r\n");
                    sql.Append("      (select min (" + tempkey + ") from \r\n");
                    sql.Append("      (select top ((" + _index + "-1)*" + PageSize + ") " + Key + " from " + TableStr + saftwhere + " order by " + Key + " desc) as temptable \r\n");
                    sql.Append("       )   " + saftandwhere + "  \r\n");
                    sql.Append("order by " + Key + " desc\r\n");
                }
                else
                {
                    //����
                    sql.Append("select top " + PageSize + " " + SelectCols + " \r\n");
                    sql.Append("from " + TableStr + " \r\n");
                    sql.Append("where " + Key + "> \r\n");
                    sql.Append("      (select max (" + tempkey + ") from \r\n");
                    sql.Append("      (select top ((" + _index + "-1)*" + PageSize + ") " + Key + " from " + TableStr + saftwhere + " order by " + Key + ") as temptable \r\n");
                    sql.Append("       )  " + saftandwhere + "   \r\n");
                    sql.Append("order by " + Key + " \r\n");
                }
            }
            //Console.WriteLine(sql.ToString());
            return new PageInfo(all, _index, AccessData.GetDt(sql.ToString(), param), PageSize);
        }
        public PageInfo GetPage(string SelectCols, string TableStr, string WhereStr, string Key, Int32 Index, Int16 PageSize, bool IsDesc)
        {
            return GetPage(SelectCols, TableStr, WhereStr, Key, Index, PageSize, IsDesc, new SqlParam());
        }
        /// <summary>
        /// ���Զ��������ֶεķ�ҳ
        /// ��Դ������ҳ������������
        /// </summary>
        /// <param name="SelectCols">Ҫѡ����ֶ�</param>
        /// <param name="TableStr">Ҫ��ѯ�ı�������</param>
        /// <param name="WhereStr">��ѯ����</param>
        /// <param name="Index">ҳ��</param>
        /// <param name="PageSize">ҳ��С</param>
        /// <param name="orderbycol">�����ֶ�</param>
        /// <param name="Isdesc">�Ƿ���</param>
        /// <returns></returns>
        public PageInfo GetPage(string SelectCols, string TableStr, string WhereStr, Int32 Index, Int16 PageSize, string orderbycol, Boolean Isdesc, SqlParam param)
        {
            int all = getAll(TableStr, WhereStr, param.Copy());
            int _index = getIndex(all, Index, PageSize);
            if (PageSize < 1)
                return new PageInfo(all, _index, new DataTable(), PageSize);
            string sc = SelectCols;
            if (SelectCols.Contains("," + orderbycol))
                sc += "," + orderbycol;
            int ps = (all - (_index - 1) * PageSize) >= PageSize ? PageSize : all - (_index - 1) * PageSize;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from \r\n");
            sql.Append("	(\r\n");
            sql.Append("		select top " + ps + " * from \r\n");
            sql.Append("		(\r\n");
            sql.Append("			select top " + (_index) * PageSize + " " + SelectCols + " from " + TableStr + (string.IsNullOrEmpty(WhereStr) ? "" : " where ") + WhereStr + " order by " + orderbycol + " " + (Isdesc ? "desc" : "asc"));
            sql.Append("		) as t \r\n");
            sql.Append("		order by " + getTempKey(orderbycol, "t") + " " + (!Isdesc ? "desc" : "asc") + "\r\n");
            sql.Append("	) as s \r\n");
            sql.Append("	order by " + getTempKey(orderbycol, "s") + " " + (Isdesc ? "desc" : "asc") + "\r\n");
            return new PageInfo(all, _index, AccessData.GetDt(sql.ToString(), param), PageSize);
        }


        /// <summary>
        /// ���Զ��������ֶεķ�ҳ
        /// ��Դ������ҳ������������
        /// </summary>
        /// <param name="SelectCols">Ҫѡ����ֶ�</param>
        /// <param name="TableStr">Ҫ��ѯ�ı�������</param>
        /// <param name="WhereStr">��ѯ����</param>
        /// <param name="Index">ҳ��</param>
        /// <param name="PageSize">ҳ��С</param>
        /// <param name="orderbycol">�����ֶ�</param>
        /// <param name="Isdesc">�Ƿ���</param>
        /// <returns></returns>
        public PageInfo GetPage(string SelectCols, string TableStr, string WhereStr, Int32 Index,
            Int16 PageSize, string orderbycol, string orderbycolSec, Boolean Isdesc, SqlParam param)
        {
            int all = getAll(TableStr, WhereStr, param.Copy());
            int _index = getIndex(all, Index, PageSize);
            if (PageSize < 1)
                return new PageInfo(all, _index, new DataTable(), PageSize);
            string sc = SelectCols;
            if (SelectCols.Contains("," + orderbycol))
                sc += "," + orderbycol;
            int ps = (all - (_index - 1) * PageSize) >= PageSize ? PageSize : all - (_index - 1) * PageSize;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from \r\n");
            sql.Append("	(\r\n");
            sql.Append("		select top " + ps + " * from \r\n");
            sql.Append("		(\r\n");
            sql.Append("			select top " + (_index) * PageSize + " " + SelectCols + " from " + TableStr);
            sql.Append((string.IsNullOrEmpty(WhereStr) ? "" : " where ") + WhereStr + " order by " + orderbycol + (Isdesc ? " desc" : " asc"));
            sql.Append(string.IsNullOrEmpty(orderbycolSec)?"":","+orderbycolSec+(Isdesc?" desc":" asc"));
            sql.Append("		) as t \r\n");
            sql.Append("		order by " + getTempKey(orderbycol, "t") + " " + (!Isdesc ? "desc" : "asc"));
            sql.Append(string.IsNullOrEmpty(orderbycolSec) ? "" : "," + getTempKey(orderbycolSec, "t") + (!Isdesc ? " desc" : " asc"));
            sql.Append("\r\n" + "	) as s \r\n");
            sql.Append("	order by " + getTempKey(orderbycol, "s") + " " + (Isdesc ? "desc" : "asc"));
            sql.Append(string.IsNullOrEmpty(orderbycolSec) ? "" : "," + getTempKey(orderbycolSec, "s") + (Isdesc ? " desc" : " asc"));
            return new PageInfo(all, _index, AccessData.GetDt(sql.ToString(), param), PageSize);
        }
        public PageInfo GetPage(string SelectCols, string TableStr, string WhereStr, Int32 Index,
            Int16 PageSize, string orderbycol, string orderbycolSec, bool IsDesc)
        {
            return GetPage(SelectCols, TableStr, WhereStr, Index, PageSize, orderbycol, orderbycolSec, IsDesc, new SqlParam());
        }


        #region  �ķ�ҳֻ����Ʒ���б�ҳ
        /// <summary>
        /// ���Զ��������ֶεķ�ҳ
        /// ��Դ������ҳ������������
        /// </summary>
        /// <param name="SelectCols">Ҫѡ����ֶ�</param>
        /// <param name="TableStr">Ҫ��ѯ�ı�������</param>
        /// <param name="WhereStr">��ѯ����</param>
        /// <param name="Index">ҳ��</param>
        /// <param name="PageSize">ҳ��С</param>
        /// <param name="orderbycol">�����ֶ�</param>
        /// <param name="Isdesc">�Ƿ���</param>
        /// <returns></returns>
        public PageInfo GetBrandPage(string SelectCols, string TableStr, string WhereStr, Int32 Index, Int16 PageSize, string orderbycol, Boolean Isdesc, SqlParam param)
        {

            int all = getAll(TableStr, WhereStr, param.Copy());
            int _index = getIndex(all, Index, PageSize);
            if (PageSize < 1)
                return new PageInfo(all, _index, new DataTable(), PageSize);
            string sc = SelectCols;
            if (SelectCols.Contains("," + orderbycol))
                sc += "," + orderbycol;
            int ps = (all - (_index - 1) * PageSize) >= PageSize ? PageSize : all - (_index - 1) * PageSize;
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from \r\n");
            sql.Append("	(\r\n");
            sql.Append("		select top " + ps + " * from \r\n");
            sql.Append("		(\r\n");
            sql.Append("			select top " + (_index) * PageSize + " " + SelectCols + " from " + TableStr + (string.IsNullOrEmpty(WhereStr) ? "" : " where ") + WhereStr + " order by " + orderbycol + " " + (Isdesc ? "desc" : "asc"));
            sql.Append("		) as t \r\n");
            sql.Append("		order by t. IsCommend asc,t.CommendSort desc ,t.BrandId  desc \r\n");
            sql.Append("	) as s \r\n");
            sql.Append("	order by " + getTempKey(orderbycol, "s") + " " + (Isdesc ? "desc" : "asc") + "\r\n");
            return new PageInfo(all, _index, AccessData.GetDt(sql.ToString(), param), PageSize);
        }

        #endregion
        public PageInfo GetPage(string SelectCols, string TableStr, string WhereStr, Int32 Index, Int16 PageSize, string orderbycol, Boolean Isdesc)
        {
            return GetPage(SelectCols, TableStr, WhereStr, Index, PageSize, orderbycol, Isdesc, new SqlParam());
        }
        /// <summary>
        /// ��ȡ�������ҳ��
        /// </summary>
        /// <param name="all">��ҳ��</param>
        /// <param name="index">����ҳ��</param>
        /// <param name="pagesize">ҳ��С</param>
        /// <returns></returns>
        private int getIndex(int all, int index, Int16 pagesize)
        {
            if (index <= 1)
                return 1;
            int rindex = 1;
            if (pagesize * index <= all)
            {
                rindex = index;
            }
            else
            {
                if (all % pagesize == 0)
                    rindex = all / pagesize;
                else
                    rindex = (all / pagesize + 1);
            }
            if (rindex < 1)
                rindex = 1;
            return rindex;
        }
        /// <summary>
        /// �����������滻����ʱ��
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="TempTable"></param>
        /// <returns></returns>
        private string getTempKey(string Key, string TempTable)
        {
            return TempTable + "." + Key.Split('.')[Key.Split('.').Length - 1];
        }
    }
}
