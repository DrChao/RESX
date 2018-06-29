using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Language.Public
{
      /// 系统常量定义
    /// </summary>
    public class CONST_COMMON
    {
        /// <summary>
        /// 返回客户端JSON提示信息
        /// Session是否過期,执行任务是否成功.
        /// </summary>
        public const String ClientInfo = "ClientInfo";

        /// <summary>
        /// 返回客户端元数据包
        /// </summary>
        public const String Meta = "MetaInfo";

        /// <summary>
        ///  返回客户端登陆用户基本信息
        /// </summary>
        public const String UserInfo = "UserInfo";
    }
}
