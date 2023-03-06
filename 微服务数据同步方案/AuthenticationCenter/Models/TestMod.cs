using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationCenter.Models
{
    /// <summary>
    /// 测试实体
    /// </summary>
    public class TestMod
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { set; get; }
        /// <summary>
        /// 是否是男孩
        /// </summary>
        public bool IsBoy { set; get; }
        /// <summary>
        /// 各科成绩
        /// </summary>
        public List<Examination> Exas { set; get; }
    }

    /// <summary>
    /// 考试成绩
    /// </summary>
    public class Examination
    {
        /// <summary>
        /// 科目名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 分数
        /// </summary>
        public double Score { set; get; }
    }
}
