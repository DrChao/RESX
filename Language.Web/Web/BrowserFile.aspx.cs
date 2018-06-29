using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Language.Web.Web
{
    public partial class BrowserFile : BaseWeb
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String _FileName = base.GetQueryValue("FileName");
            String _SysName = base.GetQueryValue("SysName");
            if (String.IsNullOrEmpty(_FileName) || String.IsNullOrEmpty(_SysName))
            {
                return;
            }
            this.BrowserServerFile(_FileName, _SysName);
        }

        private void BrowserServerFile(String _FileName, String _SysName)
        {
            String _Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\" + _SysName);
            if (!Directory.Exists(_Path))
            {
                return;
            }
            String _File = Path.Combine(_Path, _FileName);
            if (!File.Exists(_File))
            {
                return;
            }

            base.ClientBrowserFile(_File);
        }
    }
}