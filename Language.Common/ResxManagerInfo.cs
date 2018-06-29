using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Language.Resource;
using CPC.DataAccess;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
namespace Language.Common
{
    public class ResxManagerInfo : BaseRole
    {
        /// <summary>
        /// 获取产品领域初始化数据
        /// </summary>
        /// <param name="_Language"></param>
        /// <returns></returns>
        public DataSet GetProductsDomainInfo(String _Language)
        {
            String _strSql = String.Format(@" Select * from cpc_Initial_Domain");
            dbConnect();
            DataSet _DS = runSql.GetDataBySql(_strSql);
            return _DS;
        }


        public void UpdateResxInfo()
        {
            try
            {
                String _strSql = String.Format(@" select a.FKey,a.FValue from Resx_Info a with(nolock) left join Resx_DetailInfo  b with(nolock) 
                    on a.FKey=b.FKey where a.Sys_Name='TAOS' and isnull(b.FKey,'')='' order by b.FKey ");
                dbConnect();
                DataSet _DS = runSql.GetDataBySql(_strSql);
                foreach (DataRow row in _DS.Tables[0].Rows)
                {
                    _strSql = string.Empty;
                    string strFkey = row["fkey"].ToString().Trim();
                    string strValue = row["FValue"].ToString().Trim();
                    String _txtWordCN = ChineseConverter.Convert(strValue, ChineseConversionDirection.TraditionalToSimplified);
                    String _txtWordTW = ChineseConverter.Convert(strValue, ChineseConversionDirection.SimplifiedToTraditional);
                    _strSql = _strSql + string.Format(@" insert into Resx_DetailInfo (FKey,Resx_Type,Resx_Text,Sys_Name,CUser,CTime) 
                    values(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',GETDATE()) ", strFkey, "zh-CN", _txtWordCN, "TAOS", "admin");
                    _strSql = _strSql + string.Format(@" insert into Resx_DetailInfo (FKey,Resx_Type,Resx_Text,Sys_Name,CUser,CTime) 
                    values(N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',GETDATE()) ", strFkey, "zh-TW", _txtWordTW, "TAOS", "admin");
                    runSql.ExecSql(_strSql, "");
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 新增词条
        /// </summary>
        /// <param name="_Key"></param>
        /// <param name="_Value"></param>
        /// <param name="_IsJScript"></param>
        /// <param name="_SysName"></param>
        /// <param name="_CUser"></param>
        /// <returns></returns>
        public void AddResx(out string _Key, String _Value, Boolean _IsJScript, String _SysName, String _CUser)
        {

            try
            {
                String _txtWordCN = ChineseConverter.Convert(_Value, ChineseConversionDirection.TraditionalToSimplified);
                String _txtWordTW = ChineseConverter.Convert(_Value, ChineseConversionDirection.SimplifiedToTraditional);

                string strSql = string.Format(@" select * from  Resx_Info where (FValue=N'{0}' or FValue=N'{1}') and Sys_Name='{2}' ", _txtWordCN, _txtWordTW, _SysName);
                dbConnect();
                DataSet ds = runSql.GetDataBySql(strSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    _Key = ds.Tables[0].Rows[0]["FKey"].ToString().Trim();
                }
                else
                {

                    _Key = DateTime.Now.ToString("yyMMddHHmmssfff");
                    if (_IsJScript)
                    {
                        _Key = "S" + _Key;
                    }
                    else
                    {
                        _Key = "A" + _Key;
                    }
                    String _strSql = String.Format(@"if not exists( select * from  Resx_Info where (FValue=N'{5}' or FValue=N'{6}')  and Sys_Name='{3}' )
	                Begin 
                        Insert into Resx_Info(FKey, FValue, Is_JScript, Sys_Name, CUser, CTime)
	                    Values(N'{0}',N'{1}',{2},N'{3}',N'{4}',getdate()) 
                    End ", _Key, _Value, _IsJScript ? 1 : 0, _SysName, _CUser, _txtWordCN, _txtWordTW);
                    dbConnect();
                    runSql.ExecSql(_strSql, "");
                }
            }
            catch
            {
                _Key = string.Empty;
            }
        }

        public Boolean IsAddResxDetailInfo(string strKeyWord, string strSysName)
        {
            bool blStatus = true;
            String _txtWordCN = ChineseConverter.Convert(strKeyWord, ChineseConversionDirection.TraditionalToSimplified);
            String _txtWordTW = ChineseConverter.Convert(strKeyWord, ChineseConversionDirection.SimplifiedToTraditional);
            String _strSql = String.Format(@" select * from  Resx_DetailInfo where (Resx_Text=N'{0}' or Resx_Text='{1}')  and Sys_Name='{2}' ", _txtWordCN, _txtWordTW, strSysName);
            dbConnect();
            DataSet ds = runSql.GetDataBySql(_strSql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                blStatus = false;
            }
            return blStatus;
        }

        /// <summary>
        /// 模糊匹配词条信息
        /// </summary>
        /// <param name="_Word"></param>
        public DataSet CheckItemInfo(String _SysName, String _Word)
        {
            String _txtWordCN = ChineseConverter.Convert(_Word, ChineseConversionDirection.TraditionalToSimplified);
            String _txtWordTW = ChineseConverter.Convert(_Word, ChineseConversionDirection.SimplifiedToTraditional);

            String _strSql = String.Format(@" Select *,convert(nvarchar(10),CTime,120 ) as DTime
                from Resx_Info where Sys_Name='{0}' and (FValue like N'%{1}%' or FValue like N'%{2}%' ) ", _SysName, _txtWordCN, _txtWordTW);
            dbConnect();
            DataSet _DS = runSql.GetDataBySql(_strSql);
            if (_DS != null && _DS.Tables.Count > 0 && _DS.Tables[0].Rows.Count > 0)
            {
                CreateResx _CResx = new CreateResx();
                _DS.Tables[0].Columns.Add("Context");
                _DS.Tables[0].Columns.Add("Html");
                int iCount = 0;
                foreach (DataRow _Row in _DS.Tables[0].Rows)
                {
                    ++iCount;
                    string strJs = "<span id='spJs" + iCount.ToString() + "'>" + _CResx.GetJScriptCode(_Row["FKey"].ToString().Trim(), _Row["FValue"].ToString().Trim()) + "</span>";
                    string strCs = "<span id='spCs" + iCount.ToString() + "'>" + _CResx.GetCSharpCode(_Row["FKey"].ToString().Trim(), _Row["FValue"].ToString().Trim()) + "</span>";
                    string strHtml = "<span ><input type='text' id='spHtml" + iCount.ToString() + "' style='border:0px;width:300px;' value='<%=" + _CResx.GetCSharpCode(_Row["FKey"].ToString().Trim(), _Row["FValue"].ToString().Trim()) + "%>'/></span>";
                    _Row["Context"] = "JS替换代码： " + strJs
                            + "  <a  href='javascript:void(0);' onclick='CopyToClipbord(\"spJs" + iCount.ToString() + "\");'>復制</a>"
                            + "<br/>CS后臺替换代码：  " + strCs
                            + "  <a  href='javascript:void(0);' onclick='CopyToClipbord(\"spCs" + iCount.ToString() + "\");'>復制</a>"
                            + "<br/>html替换代码：  " + strHtml
                            + "  <a  href='javascript:void(0);' onclick='CopyToClipbord(\"spHtml" + iCount.ToString() + "\");'>復制</a>";
                }
            }
            return _DS;
        }

        //            public DataSet GetWordItemInfo(String _SysName, String _Key)
        //            {
        //                DataSet ds = new DataSet();
        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("Type", typeof(string));
        //                dt.Columns.Add("F", typeof(string));
        //                dt.Columns.Add("Type", typeof(string));

        //                String _strSql = String.Format(@" Select *,convert(nvarchar(10),CTime,120 ) as DTime
        //                    from Resx_Info where Sys_Name='{0}' and  FKey='{1}' ", _SysName, _Key);
        //                dbConnect();
        //                DataSet _DS = runSql.GetDataBySql(_strSql);
        //                if (_DS != null && _DS.Tables.Count > 0 && _DS.Tables[0].Rows.Count > 0)
        //                {
        //                    CreateResx _CResx = new CreateResx();
        //                    _DS.Tables[0].Columns.Add("Context");
        //                    _DS.Tables[0].Columns.Add("Html");
        //                    foreach (DataRow _Row in _DS.Tables[0].Rows)
        //                    {
        //                        string strJs = "<span id='spJs'>" + _CResx.GetJScriptCode(_Row["FKey"].ToString().Trim(), _Row["FValue"].ToString().Trim()) + "</span>";
        //                        string strCs = "<span id='spCs'>" + _CResx.GetCSharpCode(_Row["FKey"].ToString().Trim(), _Row["FValue"].ToString().Trim()) + "</span>";
        //                        string strHtml = "<span ><input type='text' id='spHtml' style='border:0px;width:300px;' value='<%=" + _CResx.GetCSharpCode(_Row["FKey"].ToString().Trim(), _Row["FValue"].ToString().Trim()) + "%>'/></span>";
        //                        _Row["Context"] = "JS替换代码： " + strJs
        //                                + "  <a  href='javascript:void(0);' onclick='CopyToClipbord(\"spJs\");'>復制</a>"
        //                                + "<br/>CS后臺替换代码：  " + strCs
        //                                + "  <a  href='javascript:void(0);' onclick='CopyToClipbord(\"spCs\");'>復制</a>"
        //                                + "<br/>html替换代码：  " + strHtml
        //                                + "  <a  href='javascript:void(0);' onclick='CopyToClipbord(\"spHtml\");'>復制</a>";
        //                    }
        //                }
        //                return _DS;
        //            }



        /// <summary>
        /// 词条是否存在
        /// </summary>
        /// <param name="_Word"></param>
        /// <returns></returns>
        public bool IsExists(string _SysName, string _Word, out string _Key)
        {
            bool _Status = false;
            _Key = String.Empty;
            try
            {
                string _strSql = String.Format(@" Select * from Resx_Info where Sys_name='{0}' and FValue=N'{1}' ", _SysName, _Word);
                dbConnect();
                DataSet _DS = runSql.GetDataBySql(_strSql);
                if (_DS != null && _DS.Tables.Count > 0 && _DS.Tables[0].Rows.Count > 0)
                {
                    _Key = _DS.Tables[0].Rows[0]["FKey"].ToString();
                    _Status = true;
                }
            }
            catch (Exception ex)
            {
                _Status = true;
            }
            return _Status;
        }

        public bool IsExistsFKey(string _Key)
        {
            bool _Status = false;

            string _strSql = String.Format(@" Select * from Resx_Info where FKey='{0}' ", _Key);
            dbConnect();
            DataSet _DS = runSql.GetDataBySql(_strSql);
            if (_DS != null && _DS.Tables.Count > 0 && _DS.Tables[0].Rows.Count > 0)
            {
                _Key = _DS.Tables[0].Rows[0]["FKey"].ToString();
                _Status = true;
            }
            return _Status;
        }

        public DataSet GetWordInfo(string _SysName, String _Filter)
        {
            String _strSql = String.Format(@" Select * from Resx_Info Where Sys_name='{0}'  and  {1} ", _SysName, _Filter);
            dbConnect();
            return runSql.GetDataBySql(_strSql);
        }

        public DataSet GetWaitWordInfo(String _SysName, int _Page, int _PageSize, String _TransLanguage, Boolean _IsTrans)
        {
            _Page = _Page <= 0 ? 0 : _Page - 1;
            _PageSize = _PageSize <= 0 ? 10 : _PageSize;
            int _Size = _Page * _PageSize;

            String _strSql = String.Format(@" Declare @Counts int
            Select @Counts=Count(*) from Resx_Info A    Where Sys_Name='{0}' 
	            and Fkey {4} in (Select Fkey from Resx_DetailInfo where Resx_Type in('{1}')) 
            Select top {2} *,@Counts as Counts from Resx_Info 
            where FKey not in (select top {3} FKey from Resx_Info Where Sys_Name='{0}'  
            and Fkey {4} in (Select Fkey from Resx_DetailInfo where Resx_Type in('{1}'))  order by CTime desc) 
            and Sys_Name='{0}' and Fkey {4} in (Select Fkey from Resx_DetailInfo where Resx_Type in('{1}')) 
             order by CTime desc  ", _SysName, _TransLanguage, _PageSize, _Size, _IsTrans ? "" : "not");
            dbConnect();
            return runSql.GetDataBySql(_strSql);
        }

        public DataSet GetWaitWordInfo(String _SysName)
        {
            String _strSql = String.Format(@" Select * from Resx_Info   Where Sys_Name='{0}' order by CTime desc ", _SysName);
            dbConnect();
            return runSql.GetDataBySql(_strSql);
        }

        public DataSet GetWaitUpdateWords()
        {
            String _strSql = String.Format(@" Select * from yyyyy where is_import=0 ");
            dbConnect();
            return runSql.GetDataBySql(_strSql);
        }

        public void RunSql(string strSql)
        {

            dbConnect();
            runSql.ExecSql(strSql, "");

        }

        public DataSet GetDetailWordInfo(String _FKey, String _ResxType)
        {
            String _strSql = String.Empty;
            if (String.IsNullOrEmpty(_ResxType))
            {
                _strSql = String.Format(@" Select * from Resx_DetailInfo  Where FKey='{0}' ", _FKey);
            }
            else
            {
                _strSql = String.Format(@" Select * from Resx_DetailInfo  Where FKey='{0}' and Resx_Type='{1}' ", _FKey, _ResxType);
            }
            dbConnect();
            return runSql.GetDataBySql(_strSql);
        }
        public DataSet GetDetailWordInfoBySysName(String _SysName)
        {
            String _strSql = String.Format(@" Select * from Resx_DetailInfo  Where   Sys_Name='{0}' ", _SysName);
            dbConnect();
            return runSql.GetDataBySql(_strSql);
        }

        public Boolean AddResxInfo(String _FKey, String _txtWordCN, String _txtWordTW,
                String _txtWordEN, String _User, String _SysName, String _CultureType)
        {
            Boolean _Status = false;
            try
            {
                String _strSql = String.Empty;

                if (!String.IsNullOrEmpty(_CultureType))
                {
                    _strSql = _strSql + String.Format(@" 
                    if exists(Select * from Resx_DetailInfo Where FKey='{0}' and Resx_Type='{1}')
	                    Begin
		                    Update Resx_DetailInfo Set Resx_Text=N'{2}' Where FKey='{0}' and Resx_Type='{1}' 
	                    End
                    Else
	                    Begin
                            insert into Resx_DetailInfo(FKey, Resx_Type, Resx_Text, 
                                CUser, CTime,Sys_Name) values ('{0}',N'{1}',N'{2}',N'{3}',GetDate(),'{4}')
	                    End ",
                    _FKey, _CultureType, _txtWordEN, _User, _SysName);
                }
                else
                {

                    if (String.IsNullOrEmpty(_txtWordCN))
                    {
                        _strSql = _strSql + String.Format(@" Delete from Resx_DetailInfo Where FKey='{0}' 
                    and Resx_Type='{1}'  ", _FKey, CultureType.ZH_CN);
                    }
                    else
                    {
                        _strSql = _strSql + String.Format(@" 
                    if exists(Select * from Resx_DetailInfo Where FKey='{0}' and Resx_Type='{1}')
	                    Begin
		                    Update Resx_DetailInfo Set Resx_Text=N'{2}' Where FKey='{0}' and Resx_Type='{1}' 
	                    End
                    Else
	                    Begin
                            insert into Resx_DetailInfo(FKey, Resx_Type, Resx_Text, 
                                CUser, CTime,Sys_Name) values ('{0}',N'{1}',N'{2}',N'{3}',GetDate(),'{4}')
	                    End ",
                        _FKey, CultureType.ZH_CN, _txtWordCN, _User, _SysName);
                    }

                    if (String.IsNullOrEmpty(_txtWordTW))
                    {
                        _strSql = _strSql + String.Format(@" Delete from Resx_DetailInfo Where FKey='{0}' 
                    and Resx_Type='{1}'  ", _FKey, CultureType.ZH_TW);
                    }
                    else
                    {
                        _strSql = _strSql + String.Format(@" 
                    if exists(Select * from Resx_DetailInfo Where FKey='{0}' and Resx_Type='{1}')
	                    Begin
		                    Update Resx_DetailInfo Set Resx_Text=N'{2}' Where FKey='{0}' and Resx_Type='{1}' 
	                    End
                    Else
	                    Begin
                            insert into Resx_DetailInfo(FKey, Resx_Type, Resx_Text, 
                                CUser, CTime,Sys_Name) values ('{0}',N'{1}',N'{2}',N'{3}',GetDate(),'{4}')
	                    End ",
                           _FKey, CultureType.ZH_TW, _txtWordTW, _User, _SysName);
                    }
                    if (String.IsNullOrEmpty(_txtWordEN))
                    {
                        _strSql = _strSql + String.Format(@" Delete from Resx_DetailInfo Where FKey='{0}' 
                    and Resx_Type='{1}'  ", _FKey, CultureType.EN_US);
                    }
                    else
                    {
                        _strSql = _strSql + String.Format(@" 
                    if exists(Select * from Resx_DetailInfo Where FKey='{0}' and Resx_Type='{1}')
	                    Begin
		                    Update Resx_DetailInfo Set Resx_Text=N'{2}' Where FKey='{0}' and Resx_Type='{1}' 
	                    End
                    Else
	                    Begin
                            insert into Resx_DetailInfo(FKey, Resx_Type, Resx_Text, 
                                CUser, CTime,Sys_Name) values ('{0}',N'{1}',N'{2}',N'{3}',GetDate(),'{4}')
	                    End ",
                            _FKey, CultureType.EN_US, _txtWordEN, _User, _SysName);
                    }

                }
                dbConnect();
                _Status = runSql.ExecSql(_strSql, String.Empty);
            }
            catch (Exception ex)
            { }
            return _Status;
        }


        public DataSet GetOtherResxInfo(String _SysName, String _CultureType)
        {
            DataSet _DS = null;
            try
            {
                String _strSql = String.Empty;
                if (String.Equals(_CultureType, CultureType.JA_JP, StringComparison.CurrentCultureIgnoreCase))
                {
                    _strSql = String.Format(@"   Declare @Counts int
                     Select  @Counts=Count(*)  from Resx_Info A Where Sys_Name='{0}' and A.FKey not in 
                                        (Select  B.FKey from Resx_DetailInfo B where Sys_Name='{0}' and B.Resx_Type='{1}' ) 
                     Select top 10 *,@Counts as Counts from Resx_Info A Where Sys_Name='{0}' and A.FKey not in 
                                        (Select  B.FKey from Resx_DetailInfo B where Sys_Name='{0}' and B.Resx_Type='{1}' ) 
                                        order by CTime Desc
                    ", _SysName, _CultureType);
                }
                dbConnect();
                _DS = runSql.GetDataBySql(_strSql, DataType.CPC);
            }
            catch (Exception ex)
            {
                _DS = null;
            }
            return _DS;
        }
    }
}
