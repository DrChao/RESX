using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Resources;
using System.IO;
using System.Data;
using System.Data.OleDb;
using Language.Common;
using Language.Public;
using Language.Resource;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
namespace Language.Web.Web
{
    public partial class Import : System.Web.UI.Page
    {
        string strDir = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            new ResxManagerInfo().UpdateResxInfo();
            return;

            strDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
            string strExcelFile = Path.Combine(strDir, "140517-2013DM0038-TAOS1.0多語言词语汇总-V1r00-mingzhu.xls");

            DataSet ds = this.GetExcelData(strExcelFile, "", 1);

            bool _IsJScript = true;
            ResxManagerInfo _ResxCtrl = new ResxManagerInfo();
            string _SysName = "TAOS";

            return;

            //String _txtWordCN = ChineseConverter.Convert(_Word, ChineseConversionDirection.TraditionalToSimplified);
            //String _txtWordTW = ChineseConverter.Convert(_Word, ChineseConversionDirection.SimplifiedToTraditional);
            //String _txtWordEN = "";
            String _txtWordCN = string.Empty;
            string _txtWordTW = string.Empty;
            string _txtWordEN = string.Empty;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string _Word = row["中文繁体"].ToString().Trim();
                //if (string.IsNullOrEmpty(_Word))
                //{
                //    continue;
                //}
                _txtWordCN = ChineseConverter.Convert(_Word, ChineseConversionDirection.TraditionalToSimplified);
                _txtWordTW = ChineseConverter.Convert(_Word, ChineseConversionDirection.SimplifiedToTraditional);
                _txtWordEN = string.Empty;

                string _Key = string.Empty;
                _ResxCtrl.AddResx(out _Key, _Word, _IsJScript, _SysName, "Admin");

                if (!string.IsNullOrEmpty(_Key))
                {
                    bool blnStatus = _ResxCtrl.IsAddResxDetailInfo(_Word, _SysName);
                    if (blnStatus)
                    {
                        _ResxCtrl.AddResxInfo(_Key, _txtWordCN, _txtWordTW, _txtWordEN, "admin", _SysName, String.Empty);
                    }
                }
            }
        }

        //private string CheckKey(string strKey)
        //{

        //    bool blStatus = true;
        //    while (blStatus)
        //    {
        //        blStatus = new ResxManagerInfo().IsExistsFKey(strKey);
        //        if (blStatus)
        //        {
        //            strKey = strKey.Substring(0, 1) + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //        }
        //    }
        //    return strKey;
        //}

        public DataSet GetExcelData(string strExcelFileName, string strName, int iSheetIndex)
        {
            //源的定义
            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";

            DataSet ds = new DataSet();
            OleDbConnection objConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strExcelFileName + ";" + "Extended Properties=Excel 8.0;");
            objConn.Open();
            DataTable schemaTable = objConn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

            string strSheetName = string.Empty;

            string strSql = string.Empty;
            int iCount = 0;
            foreach (DataRow row in schemaTable.Rows)
            {
                ++iCount;
                string strNameA = row["table_name"].ToString().Trim();
                if (string.IsNullOrEmpty(strSheetName))
                {
                    if (iCount == iSheetIndex)
                    {
                        strSheetName = strNameA;
                        break;
                    }
                }
                else
                {
                    if (string.Equals(strNameA, strName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        strSheetName = strName;
                        break;
                    }
                }
            }
            strSql = "select * from [" + strSheetName + "]";
            OleDbCommand objCmd = new OleDbCommand(strSql, objConn);
            OleDbDataAdapter myData = new OleDbDataAdapter(strSql, objConn);
            myData.Fill(ds, strSheetName);//填充数据
            objConn.Close();
            return ds;
        }
    }
}