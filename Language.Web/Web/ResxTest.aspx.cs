using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Language.Common;
using Language.Resource;
namespace Language.Web.Web
{
    public partial class ResxTest : PageWeb
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            this.ClearCtrlInfo();
            String _FKey=txtKey.Text.Trim();
            DataSet _DS = new ResxManagerInfo().GetDetailWordInfo(_FKey,"en-US");
            if (_DS != null && _DS.Tables.Count > 0 && _DS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow _Row in _DS.Tables[0].Rows)
                {
                    String _Type = _Row["Resx_Type"].ToString().Trim();
                    if (String.Equals(_Type, CultureType.ZH_CN, StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtCN.Value = _Row["Resx_Text"].ToString().Trim();
                    }
                    if (String.Equals(_Type, CultureType.ZH_TW, StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtTW.Value = _Row["Resx_Text"].ToString().Trim();
                    }
                    if (String.Equals(_Type, CultureType.EN_US, StringComparison.CurrentCultureIgnoreCase))
                    {
                        txtEN.Value = _Row["Resx_Text"].ToString().Trim();
                    }
                }
            }
        }

        private void ClearCtrlInfo()
        {
            txtCN.Value = "";
            txtTW.Value = "";
            txtEN.Value = "";
        }
    }
}