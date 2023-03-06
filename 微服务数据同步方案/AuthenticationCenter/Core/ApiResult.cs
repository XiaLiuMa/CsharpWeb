using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationCenter.Core
{
    /// <summary>
    /// API返回结果
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Msg { get; set; } = "";
        /// <summary>
        /// 返回数据类型
        /// </summary>
        public string Type { get; set; } = "";
        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; } = "";
        /// <summary>
        /// 返回的扩展数据
        /// </summary>
        public object DataExt { get; set; } = "";
    }
}
