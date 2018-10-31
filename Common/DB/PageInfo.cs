/*----------------------------------------------------------------
// Copyright (C) 2010 盛拓传媒 
// 文件名：Page.cs
// 文件功能描述：数据底层 （分页）（从别的项目的移过来的）
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Common.DB
{
    public class PageInfo
    {
        private int _Count;
        private int _Index;
        private DataTable _dt;
        private short _pageSize;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="Count">总记录数</param>
        /// <param name="dt">当前页的记录</param>
        public PageInfo(int Count, int index, DataTable dt,short pageSize)
        {
            _dt = dt;
            if (_dt == null)
                _dt = new DataTable();
            _Count = Count;
            _Index = index;
            _pageSize = pageSize;
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int All
        {
            get
            {
                return _Count;
            }
        }
        /// <summary>
        /// 当前真实页号。
        /// </summary>
        public int Index
        {
            get
            {
                return _Index;
            }
        }
        /// <summary>
        /// 当前页的记录
        /// </summary>
        public DataTable Records
        {
            get
            {
                return _dt;
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return _Count % _pageSize == 0 ? _Count / _pageSize : (_Count / _pageSize) + 1;
            }
        }
    }
}
