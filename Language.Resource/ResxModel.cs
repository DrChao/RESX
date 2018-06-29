using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Language.Resource
{
    /// <summary>
    /// 实体类
    /// </summary>
    [Serializable]
    public class ResxModel
    {
        /// <summary>
        /// 语言类型
        /// </summary>
        public String ResxType { get; set; }

        /// <summary>
        ///词条关键字
        /// </summary>
        public String FKey { get; set; }

        /// <summary>
        /// 词条内容
        /// </summary>
        public String ResxText { get; set; }

        /// <summary>
        /// 服务保存路径
        /// </summary>
        public String SavePath { get; set; }

        /// <summary>
        /// 是否生成JavaScript脚本文件
        /// </summary>
        public Boolean IsJScript { get; set; }
    }
}
