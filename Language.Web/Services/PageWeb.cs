using System;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Language.Web
{
    public class PageWeb:BaseWeb
    {
        protected override void OnInit(EventArgs e)
        {
            LiteralControl _ctrl = new LiteralControl();
            _ctrl.Text = this.WriteClientScripts();
            this.Header.Controls.AddAt(1, _ctrl);
        }

        /// <summary>
        /// 输出客户端公共通用文件
        /// </summary>
        /// <returns></returns>
        private String WriteClientScripts()
        {
            String _WebPath = GetWebApplicantPath();
            String _WebCss = String.Format(@"<link rel='Stylesheet' type='text/css' href='{0}'/>", _WebPath + @"/Style/public.css");

            String _lhgcoreCss = String.Format(@"<link rel='Stylesheet' type='text/css' href='{0}'/>", _WebPath + @"/Style/skins/chrome.css");

            String _Public = String.Format(@"<script type='text/javascript' src='{0}'></script>", _WebPath + @"/JScripts/public.js");

            String _Common = String.Format(@"<script type='text/javascript' src='{0}'></script>", _WebPath + @"/JScripts/jquery.form.js");

            String _Query = String.Format(@"<script type='text/javascript' src='{0}'></script>", _WebPath + "/JScripts/jquery-1.8.1.min.js");

            String _ExtCore = String.Format(@"<script type='text/javascript' src='{0}'></script>", _WebPath + "/JScripts/ext-core.js");

            String _CheckInfo = String.Format(@"<script type='text/javascript' src='{0}'></script>", _WebPath + "/JScripts/CheckInfo.js");

            String _OnReady = String.Format(@"<script type='text/javascript' src='{0}'></script>", _WebPath + "/JScripts/language.js");

            String _lhgcore = String.Format(@"<script type='text/javascript' src='{0}'></script>", _WebPath + "/JScripts/lhgcore.js");

            String _lhgdialog = String.Format(@"<script type='text/javascript' src='" + _WebPath + "/JScripts/lhgdialog.js?skin=chrome'></script>");

            StringBuilder _result = new StringBuilder();
            _result.Append("\r\n");
            _result.Append(_WebCss);

            _result.Append("\r\n");
            _result.Append(_lhgcoreCss);

            _result.Append("\r\n");
            _result.Append(_Query);

            _result.Append("\r\n");
            _result.Append(_ExtCore);

            _result.Append("\r\n");
            _result.Append(_Public);

            _result.Append("\r\n");
            _result.Append(_Common);

            _result.Append("\r\n");
            _result.Append(_CheckInfo);

            _result.Append("\r\n");
            _result.Append(_OnReady);

            _result.Append("\r\n");
            _result.Append(_lhgcore);

            _result.Append("\r\n");
            _result.Append(_lhgdialog);

            _result.Append("\r\n");
            return _result.ToString();
        }


    }
}