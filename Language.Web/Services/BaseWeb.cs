using System;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
namespace Language.Web
{
    public class BaseWeb : System.Web.UI.Page
    {
        /// <summary>
        /// 取网站虚拟根目录地址
        /// </summary>
        /// <param name="_HttpContext"></param>
        /// <returns></returns>
        protected String GetWebApplicantPath()
        {
            String _Path = String.Empty;
            String _ApplicationPath = Request.ApplicationPath;
            if (_ApplicationPath.EndsWith(@"/"))
            {
                _Path = "http://" + Request.Url.Authority;
            }
            else
            {
                _Path = _ApplicationPath;
            }
            return _Path;
        }
        /// <summary>
        /// 从QueryString或Form中取值
        /// </summary>
        /// <param name="_Para"></param>
        /// <returns></returns>
        protected String GetQueryValue(String _Para)
        {
            String _Value = (String)Request.QueryString[_Para];
            if (String.IsNullOrEmpty(_Value))
            {
                _Value = (String)Request.Form[_Para];
            }
            return _Value;
        }

        /// <summary>
        /// 服务器文件输出到客户端
        /// </summary>
        /// <param name="_LocalFile">服务器本地文件路径</param>
        protected void ClientBrowserFile(String _LocalFile)
        {
            try
            {
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Buffer = false;
                string pBrowserDefaultName = System.Web.HttpUtility.UrlEncode(Path.GetFileName(_LocalFile));
                System.IO.FileStream MyFileStream;
                long FileSize;
                MyFileStream = new System.IO.FileStream(_LocalFile, System.IO.FileMode.Open);
                FileSize = MyFileStream.Length;
                MyFileStream.Close();

                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";

                System.Web.HttpContext.Current.Response.AppendHeader("content-disposition",
                                    @"attachment;filename=" + pBrowserDefaultName);
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Length", FileSize.ToString());
                System.Web.HttpContext.Current.Response.WriteFile(_LocalFile);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
                //System.IO.File.Delete(localFile);
            }
            catch (Exception ex)
            {
                if (typeof(System.Threading.ThreadAbortException).ToString() != ex.GetType().ToString())
                {
                }
            }
        }
    }
}