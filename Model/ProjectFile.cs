using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class ProjectFile
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 项目id Project.id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
    }
}
