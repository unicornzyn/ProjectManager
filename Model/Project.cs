using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Project
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目域名
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否显示 0：否 1：是
        /// </summary>
        public int IsShow { get; set; }
        /// <summary>
        /// 测试地址
        /// </summary>
        public string TestUrl { get; set; }
        /// <summary>
        ///网站文件名
        /// </summary>
        public string SiteFileName { get; set; }
        /// <summary>
        /// 数据库
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// 测试用户名
        /// </summary>
        public string TestUserName { get; set; }
        /// <summary>
        /// 测试密码
        /// </summary>
        public string TestPassword { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public int ParentId { get; set; }
    }
}
