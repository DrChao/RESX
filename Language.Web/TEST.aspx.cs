using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Resources;
using Language.Resource;
using System.Data;
using Language.Common;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
namespace Language.Web
{
    public partial class TEST : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //System.Globalization.CultureInfo.CurrentUICulture
            //CultureInfo _CultureInfo = Thread.CurrentThread.CurrentUICulture;
            //Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CultureType.ZH_CN);
            //String AA = ResxContoller.GetResource("S131202113956", "技術資產營運系統");
            //TextBox1.Text = DateTime.Now.ToString() + "  " + AA;

            //this.UpdateResx();
        }

        private void UpdateResx()
        {
            //            ResxManagerInfo CtrlInfo = new ResxManagerInfo();
            //            DataSet ds = CtrlInfo.GetWaitUpdateWords();
            //            foreach (DataRow row in ds.Tables[0].Rows)
            //            {
            //                string strTW = row["CN"].ToString().Trim();
            //                string strCN = ChineseConverter.Convert(strTW, ChineseConversionDirection.TraditionalToSimplified);
            //                string strEN = row["Final"].ToString().Trim();
            //                string strKey = row["Fkey"].ToString().Trim();

            //                string strSql = string.Format(@" 
            //                                insert into Resx_DetailInfo(fkey,resx_type,resx_text,sys_name,cuser,ctime) values('{0}','zh-CN',N'{1}','TAOS','admin',getdate())
            //                                insert into Resx_DetailInfo(fkey,resx_type,resx_text,sys_name,cuser,ctime) values('{0}','zh-TW',N'{2}','TAOS','admin',getdate())
            //                                insert into Resx_DetailInfo(fkey,resx_type,resx_text,sys_name,cuser,ctime) values('{0}','en-US',N'{3}','TAOS','admin',getdate())
            //                                update yyyyy set is_import=1 where fkey='{0}' ",
            //                    strKey, strCN, strTW, strEN);

            //                try
            //                {
            //                    CtrlInfo.RunSql(strSql);

            //                }
            //                catch(Exception ex)
            //                {
            //                    string aaa = "adsadsF";
            //                    break;
            //                }
            //            }
        }
    }
}