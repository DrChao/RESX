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
namespace Language.Web
{
    public partial class _Default : PageWeb
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ///System.Globalization.CultureInfo.CurrentUICulture
            //CultureInfo _CultureInfo = Thread.CurrentThread.CurrentUICulture;
            //Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CultureType.ZH_CN);
            //ResxContoller.AddResource("F0", "中国");

            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CultureType.ZH_TW);
            //ResxContoller.AddResource("F0", "中國");

            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CultureType.EN_US);
            //ResxContoller.AddResource("F0", "China");

            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CultureType.EN_US);
            //String AA = ResxContoller.GetResource("A120927113755", "aaaa");

        }
    }
}