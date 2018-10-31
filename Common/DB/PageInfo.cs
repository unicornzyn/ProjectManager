/*----------------------------------------------------------------
// Copyright (C) 2010 ʢ�ش�ý 
// �ļ�����Page.cs
// �ļ��������������ݵײ� ����ҳ�����ӱ����Ŀ���ƹ����ģ�
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
        /// ������
        /// </summary>
        /// <param name="Count">�ܼ�¼��</param>
        /// <param name="dt">��ǰҳ�ļ�¼</param>
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
        /// �ܼ�¼��
        /// </summary>
        public int All
        {
            get
            {
                return _Count;
            }
        }
        /// <summary>
        /// ��ǰ��ʵҳ�š�
        /// </summary>
        public int Index
        {
            get
            {
                return _Index;
            }
        }
        /// <summary>
        /// ��ǰҳ�ļ�¼
        /// </summary>
        public DataTable Records
        {
            get
            {
                return _dt;
            }
        }
        /// <summary>
        /// ��ҳ��
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
