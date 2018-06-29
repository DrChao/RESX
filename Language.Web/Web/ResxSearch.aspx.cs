using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Language.Common;
using Language.Resource;
namespace Language.Web.Web
{
    public partial class ResxSearch : PageWeb
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            this.ClearCtrlInfo();

            String _Word = String.Empty;
            String _FKey = String.Empty;

            String _sysName = sltSysName.Value;
            String _sltSearchField = sltSearchField.Value;
            String _txtSearch = txtSearch.Text.Trim();
            ResxManagerInfo _ResxCtrl = new ResxManagerInfo();

            DataSet _DSInfo = _ResxCtrl.GetWordInfo(_sysName,String.Format(" {0}=N'{1}'",
                _sltSearchField, _txtSearch));
            if (_DSInfo != null && _DSInfo.Tables.Count > 0 && _DSInfo.Tables[0].Rows.Count > 0)
            {
                _FKey = _DSInfo.Tables[0].Rows[0]["FKey"].ToString().Trim();
                _Word = _DSInfo.Tables[0].Rows[0]["FValue"].ToString().Trim();
            }

            if (String.IsNullOrEmpty(_FKey))
            {
                return;
            }


            txtFkey.Value = _FKey;
            txtWord.Value = _Word;


            DataSet _DS = _ResxCtrl.GetDetailWordInfo(_FKey,"");
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

            CreateResx _CreateResx = new CreateResx();
            txtCSharp.Value = _CreateResx.GetCSharpCode(_FKey, _Word);
            txtJScript.Value = _CreateResx.GetJScriptCode(_FKey, _Word);
            sltSysName.Value = _sysName;
        }

        private void ClearCtrlInfo()
        {
            txtCN.Value = "";
            txtTW.Value = "";
            txtEN.Value = "";
            txtFkey.Value = "";
            txtWord.Value = "";
            txtJScript.Value = "";
            txtCSharp.Value = "";
        }
    }
}