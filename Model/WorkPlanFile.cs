
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class WorkPlanFile
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 工作计划id WorkPlan.id
        /// </summary>
        public int WorkPlanId { get; set; }
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
