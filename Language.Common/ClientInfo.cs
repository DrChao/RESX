using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Language.Common
{
    [Serializable]
    public class ClientInfo
    {
        public ClientInfo() { }

        private Boolean _IsLogin = true;
        /// <summary>
        /// 是否登陆
        /// 默认已经登陆
        /// </summary>
        public Boolean IsLogin
        {
            get { return _IsLogin; }
            set { _IsLogin = value; }
        }

        private String _RedirectURL = String.Empty;
        /// <summary>
        /// 转向地址
        /// </summary>
        public String RedirectURL
        {
            get { return _RedirectURL; }
            set { _RedirectURL = value; }
        }

        private Boolean _Status = false;
        /// <summary>
        /// 状态
        /// </summary>
        public Boolean Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private String _Message = String.Empty;
        /// <summary>
        /// 返回客户端提示信息
        /// </summary>
        public String Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private Hashtable _OtherInfo = new Hashtable();
        /// <summary>
        /// 用户自定义返回客户端信息
        /// </summary>
        public Hashtable OtherInfo
        {
            get
            {
                return _OtherInfo;
            }
            set { _OtherInfo = value; }
        }

        private Boolean _IsNewWindow = false;
        /// <summary>
        /// 返回RUL,是否弹出新窗口显示
        /// </summary>
        public Boolean IsNewWindow
        {
            get { return _IsNewWindow; }
            set { _IsNewWindow = value; }
        }

        /// <summary>
        /// 访问WEB路迳
        /// </summary>
        public String WebPath
        {
            get;
            set;
        }


    }
}
