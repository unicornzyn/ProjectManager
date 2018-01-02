using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class WeekReport
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 具体工作计划内容
        /// </summary>
        public string WorkContent { get; set; }
        /// <summary>
        /// 工作类型  1 会议、2 测试、3 编写、4 跟进、5 监督
        /// </summary>
        public int WorkType { get; set; }
        public string WorkTypeStr { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanDateStart { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanDateEnd { get; set; }
        public string PlanDateStr { get; set; }
        /// <summary>
        /// 计划工作量（天）
        /// </summary>
        public int PlanDay { get; set; }
        /// <summary>
        /// 实际工作量（工时）
        /// </summary>
        public int RealDay { get; set; }
        /// <summary>
        /// 实际工作内容
        /// </summary>
        public string RealWorkContent { get; set; }
        /// <summary>
        /// 完成情况 0 未完成 1 完成
        /// </summary>
        public int Status { get; set; }
        public string StatusStr { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public int UserId { get; set; }
        public User User { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
