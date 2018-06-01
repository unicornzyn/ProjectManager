using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WorkPlan
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 工单编号/域名
        /// </summary>
        public string SheepNo { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 所属项目
        /// </summary>
        public Project Project { get; set; }
        /// <summary>
        /// 工作描述
        /// </summary>
        public string WorkRemark { get; set; }
        /// <summary>
        /// 插入(2)/正常(1)
        /// </summary>
        public int PlanType { get; set; }
        public string PlanTypeStr { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime RealStartTime { get; set; }
        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime RealEndTime { get; set; }
        /// <summary>
        ///上线时间
        /// </summary>
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// 任务状态 0：未开始 1：进行中 2：完成
        /// </summary>
        public int State { get; set; }
        public string StateStr { get; set; }
        /// <summary>
        /// 项目负责人id
        /// </summary>
        public int NeederId { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        public User Needer { get; set; }
        /// <summary>
        ///  备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 测试人员
        /// </summary>
        public string Tester { get; set; }
        /// <summary>
        /// 开发人员
        /// </summary>
        public string Dever { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string FilePath { get; set; }
    }
}
