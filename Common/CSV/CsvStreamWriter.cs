﻿using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Data;
using System.Collections.Generic;
namespace Common.CSV
{
    #region 类说明信息
    /// <summary>
    ///  写CSV文件类,首先给CSV文件赋值,最后通过Save方法进行保存操作
    ///  <Author>yangzhihong</Author>    
    ///  <CreateDate>2006/01/16</CreateDate>
    ///  <Company></Company>
    ///  <Version>1.0</Version>
    /// </summary>
    #endregion
    public class CsvStreamWriter
    {
        private ArrayList rowAL;        //行链表,CSV文件的每一行就是一个链
        private string fileName;       //文件名
        private Encoding encoding;       //编码
        private string title = "";          //标题字符串,以","分隔 lyl
        public CsvStreamWriter()
        {
            this.rowAL = new ArrayList();
            this.fileName = "";
            this.encoding = Encoding.Default;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        public CsvStreamWriter(string fileName)
        {
            this.rowAL = new ArrayList();
            this.fileName = fileName;
            this.encoding = Encoding.Default;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        /// <param name="encoding">文件编码</param>
        public CsvStreamWriter(string fileName, Encoding encoding)
        {
            this.rowAL = new ArrayList();
            this.fileName = fileName;
            this.encoding = encoding;
        }
        /// <summary>
        /// row:行,row = 1代表第一行
        /// col:列,col = 1代表第一列
        /// </summary>
        public string this[int row, int col]
        {
            set
            {
                //对行进行判断
                if (row <= 0)
                {
                    throw new Exception("行数不能小于0");
                }
                else if (row > this.rowAL.Count) //如果当前列链的行数不够，要补齐
                {
                    for (int i = this.rowAL.Count + 1; i <= row; i++)
                    {
                        this.rowAL.Add(new ArrayList());
                    }
                }
                else
                {
                }
                //对列进行判断
                if (col <= 0)
                {
                    throw new Exception("列数不能小于0");
                }
                else
                {
                    ArrayList colTempAL = (ArrayList)this.rowAL[row - 1];
                    //扩大长度
                    if (col > colTempAL.Count)
                    {
                        for (int i = colTempAL.Count; i <= col; i++)
                        {
                            colTempAL.Add("");

                        }
                    }
                    this.rowAL[row - 1] = colTempAL;
                }
                //赋值
                ArrayList colAL = (ArrayList)this.rowAL[row - 1];
                colAL[col - 1] = value;
                this.rowAL[row - 1] = colAL;
            }
        }
        /// <summary>
        /// 文件名,包括文件路径
        /// </summary>
        public string FileName
        {
            set
            {
                this.fileName = value;
            }
        }
        /// <summary>
        /// 文件编码
        /// </summary>
        public Encoding FileEncoding
        {
            set
            {
                this.encoding = value;
            }
        }
        /// <summary>
        /// 获取当前最大行
        /// </summary>
        public int CurMaxRow
        {
            get
            {
                return this.rowAL.Count;
            }
        }
        /// <summary>
        /// 获取最大列
        /// </summary>
        public int CurMaxCol
        {
            get
            {
                int maxCol;
                maxCol = 0;
                for (int i = 0; i < this.rowAL.Count; i++)
                {
                    ArrayList colAL = (ArrayList)this.rowAL[i];
                    maxCol = (maxCol > colAL.Count) ? maxCol : colAL.Count;
                }
                return maxCol;
            }
        }
        private bool isWriteTitle = false; //是否添加标题 lyl
        /// <summary>
        /// 刘玉磊
        /// 是否添加标题
        /// </summary>
        public bool IsWriteTitle
        {
            get { return isWriteTitle; }
            set { isWriteTitle = value; }
        }
        /// <summary>
        /// 添加表数据到CSV文件中
        /// </summary>
        /// <param name="dataDT">表数据</param>
        /// <param name="beginCol">从第几列开始,beginCol = 1代表第一列</param>
        /// <param name="columnNames">列名称 为空不添加 多个以英文逗号分隔</param>
        public void AddData(DataTable dataDT, int beginCol)
        { 
            if (dataDT == null)
            {
                throw new Exception("需要添加的表数据为空");
            }
            int curMaxRow;
            curMaxRow = this.rowAL.Count;

            #region 是否添加标题 刘玉磊 2012-11-23
            if (this.isWriteTitle)
            {
                this.title = "";
                for (int i = 0; i < dataDT.Columns.Count; i++)
                {
                    this.title += dataDT.Columns[i].ColumnName + ",";
                }
                this.title.TrimEnd(',');
            }
            #endregion

            for (int i = 0; i < dataDT.Rows.Count; i++)
            {
                for (int j = 0; j < dataDT.Columns.Count; j++)
                {
                    this[curMaxRow + i + 1, beginCol + j] = dataDT.Rows[i][j].ToString();
                }
            }
        }
        public void AddListDate(List<DataRow> listData, int beginCol, string columnNames,int columnNum)
        {
            if (listData == null)
            {
                throw new Exception("需要添加的表数据为空");
            }
            int curMaxRow;
            curMaxRow = this.rowAL.Count;
            if (!string.IsNullOrEmpty(columnNames))
            {
                curMaxRow++;
                string[] arrColumns = columnNames.Split(',');
                for (int i = 0; i < arrColumns.Length; i++)
                {
                    this[1, i+1] = arrColumns[i].ToString();
                }
            }
            for (int i = 0; i < listData.Count; i++)
            {
                for (int j = 0; j < columnNum; j++)
                {
                    this[curMaxRow + i + 1, beginCol + j] = listData[i][j].ToString();
                }
            }
        }
        /// <summary>
        /// 保存数据,如果当前硬盘中已经存在文件名一样的文件，将会覆盖
        /// </summary>
        public void Save()
        {
            if (this.fileName == null)
            {
                throw new Exception("缺少文件名");
            }
            else if (File.Exists(this.fileName))
            {
                File.Delete(this.fileName);
            }
            if (this.encoding == null)
            {
                this.encoding = Encoding.Default;
            }
            System.IO.StreamWriter sw = new StreamWriter(this.fileName, false, this.encoding);
            #region 是否添加标题 刘玉磊 2012-11-23
            if (isWriteTitle)
            {
                sw.WriteLine(this.title);
            }
            #endregion
            for (int i = 0; i < this.rowAL.Count; i++)
            {
                sw.WriteLine(ConvertToSaveLine((ArrayList)this.rowAL[i]));
            }
            sw.Close();
        }
        /// <summary>
        /// 保存数据,如果当前硬盘中已经存在文件名一样的文件，将会覆盖
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        public void Save(string fileName)
        {
            this.fileName = fileName;
            Save();
        }
        /// <summary>
        /// 保存数据,如果当前硬盘中已经存在文件名一样的文件，将会覆盖
        /// </summary>
        /// <param name="fileName">文件名,包括文件路径</param>
        /// <param name="encoding">文件编码</param>
        public void Save(string fileName, Encoding encoding)
        {
            this.fileName = fileName;
            this.encoding = encoding;
            Save();
        }
        /// <summary>
        /// 转换成保存行
        /// </summary>
        /// <param name="colAL">一行</param>
        /// <returns></returns>
        private string ConvertToSaveLine(ArrayList colAL)
        {
            string saveLine;
            saveLine = "";
            for (int i = 0; i < colAL.Count; i++)
            {
                saveLine += ConvertToSaveCell(colAL[i].ToString());
                //格子间以逗号分割
                if (i < colAL.Count - 1)
                {
                    saveLine += ",";
                }
            }
            return saveLine;
        }
        /// <summary>
        /// 字符串转换成CSV中的格子
        /// 双引号转换成两个双引号，然后首尾各加一个双引号
        /// 这样就不需要考虑逗号及换行的问题
        /// </summary>
        /// <param name="cell">格子内容</param>
        /// <returns></returns>
        private string ConvertToSaveCell(string cell)
        {
            cell = cell.Replace("\"", "\"\"");
            return "\"" + cell + "\"";
        }
    }
}
