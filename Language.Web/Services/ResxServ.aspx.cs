using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.IO;
using Language.Common;
using Language.Public;
using Language.Resource;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
namespace Language.Web.Services
{
    public partial class ResxServ : BaseWeb
    {
        private ClientInfo _ClientInfo = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            String _CallName = base.GetQueryValue("Call");
            if (String.IsNullOrEmpty(_CallName))
            {
                return;
            }
            this.Call(_CallName);

        }

        private void Call(String _CallName)
        {
            if (String.Equals(_CallName, "CreateItem", StringComparison.CurrentCultureIgnoreCase))
            {
                this.CreateItem();
            }
            if (String.Equals(_CallName, "CreateWordItem", StringComparison.CurrentCultureIgnoreCase))
            {
                this.CreateWordItem();
            }
            if (String.Equals(_CallName, "LoadWaitWordInfo", StringComparison.CurrentCultureIgnoreCase))
            {
                String _CultureType = base.GetQueryValue("CultureType");
                if (String.IsNullOrEmpty(_CultureType))
                {
                    _CultureType = "en-US";
                }
                this.LoadWaitWordInfo(_CultureType);
            }
            if (String.Equals(_CallName, "SaveResxInfo", StringComparison.CurrentCultureIgnoreCase))
            {
                String _CultureType = base.GetQueryValue("CultureType");
                this.SaveResxInfo(_CultureType);
            }
            if (String.Equals(_CallName, "GetDetailWordInfo", StringComparison.CurrentCultureIgnoreCase))
            {
                this.GetDetailWordInfo();
            }
            if (String.Equals(_CallName, "CreatResxFile", StringComparison.CurrentCultureIgnoreCase))
            {
                this.CreatResxFile();
            }
            if (String.Equals(_CallName, "CheckItemInfo", StringComparison.CurrentCultureIgnoreCase))
            {
                this.CheckItemInfo();
            }

        }

        private void CheckItemInfo()
        {
            _ClientInfo = new ClientInfo();
            Hashtable _MetaHash = new Hashtable();
            Hashtable _Detail = new Hashtable();

            String _SysName = base.GetQueryValue("SysName");
            String _Word = base.GetQueryValue("Word").Trim();
            DataSet _DS = new ResxManagerInfo().CheckItemInfo(_SysName, _Word);

            _MetaHash.Add("WordInfo", MetaDataInfo.GetListHashData(_DS));

            Hashtable _Hash = new Hashtable();
            _Hash.Add(CONST_COMMON.ClientInfo, _ClientInfo);
            _Hash.Add(CONST_COMMON.Meta, _MetaHash);
            String _Context = MetaDataInfo.GetClientMetaData<Hashtable>(_Hash);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(_Context);
            HttpContext.Current.Response.End();
        }

        private void CreateItem()
        {
            _ClientInfo = new ClientInfo();
            _ClientInfo.Status = true;
            _ClientInfo.Message = "adsfasd";
            Hashtable _Hash = new Hashtable();
            _Hash.Add(CONST_COMMON.ClientInfo, _ClientInfo);
            String _Context = MetaDataInfo.GetClientMetaData<Hashtable>(_Hash);
            Response.Write(_Context);
        }

        private void CreateWordItem()
        {
            _ClientInfo = new ClientInfo();
            Hashtable _MetaHash = new Hashtable();
            Hashtable _Detail = new Hashtable();
            try
            {
                ResxManagerInfo _ResxCtrl = new ResxManagerInfo();

                String _SysName = base.GetQueryValue("sltSysName");
                String _Word = base.GetQueryValue("txtWord");
                Boolean _IsJScript = Convert.ToInt16(base.GetQueryValue("chkJScript")) == 1 ? true : false;
                String _Key = String.Empty;

                Boolean _IsExists = _ResxCtrl.IsExists(_SysName, _Word, out _Key);
                if (_IsExists)
                {
                    _ClientInfo.Status = _IsExists;
                }
                else
                {
                    _ResxCtrl.AddResx(out _Key, _Word, _IsJScript, _SysName, "Admin");
                    _ClientInfo.Status = string.IsNullOrEmpty(_Key) ? false : true;
                    if (_ClientInfo.Status)
                    {
                        String _txtWordCN = ChineseConverter.Convert(_Word, ChineseConversionDirection.TraditionalToSimplified);
                        String _txtWordTW = ChineseConverter.Convert(_Word, ChineseConversionDirection.SimplifiedToTraditional);
                        String _txtWordEN = "";
                        _ResxCtrl.AddResxInfo(_Key, _txtWordCN, _txtWordTW, _txtWordEN, "admin", _SysName, String.Empty);
                    }
                }

                if (_ClientInfo.Status)
                {
                    CreateResx _CreateResx = new CreateResx();

                    _Detail.Add("Key", _Key);
                    _Detail.Add("Value", _Word);
                    _Detail.Add("CSharp", _CreateResx.GetCSharpCode(_Key, _Word));
                    _Detail.Add("JScript", _CreateResx.GetJScriptCode(_Key, _Word));
                    _MetaHash.Add("WordInfo", _Detail);
                    _ClientInfo.Message = "词条添加成功！";
                }
                else
                {
                    _ClientInfo.Message = "词条添加失败,请稍候再试！";
                }
            }
            catch (Exception ex)
            {
                _ClientInfo.Status = false;
                _ClientInfo.Message = "数据访问失败,请稍候再试！";
            }
            Hashtable _Hash = new Hashtable();
            _Hash.Add(CONST_COMMON.ClientInfo, _ClientInfo);
            _Hash.Add(CONST_COMMON.Meta, _MetaHash);
            String _Context = MetaDataInfo.GetClientMetaData<Hashtable>(_Hash);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(_Context);
            HttpContext.Current.Response.End();
        }

        private void LoadWaitWordInfo(String _CultureType)
        {
            _ClientInfo = new ClientInfo();
            Hashtable _MetaHash = new Hashtable();
            Hashtable _Detail = new Hashtable();
            try
            {
                String _SysName = base.GetQueryValue("SysName");
                String _strPage = base.GetQueryValue("PageNum");
                String _strPageSize = base.GetQueryValue("PageSize");
                String _strTrans = base.GetQueryValue("IsTrans");
                int _Page = 1;
                int _PageSize = 10;

                int.TryParse(_strPage, out _Page);
                int.TryParse(_strPageSize, out _PageSize);

                Boolean _IsTrans = true;
                if (String.IsNullOrEmpty(_strTrans) || _strTrans == "0")
                {
                    _IsTrans = false;
                }

                ResxManagerInfo _ResxCtrl = new ResxManagerInfo();
                DataSet _DS = null;


                _DS = _ResxCtrl.GetWaitWordInfo(_SysName, _Page, _PageSize, _CultureType, _IsTrans);

                _MetaHash.Add("WordInfo", MetaDataInfo.GetListHashData(_DS));
                _ClientInfo.Status = true;
            }
            catch (Exception ex)
            {
                _ClientInfo.Status = false;
                _ClientInfo.Message = "数据访问失败,请稍候再试！";
            }
            Hashtable _Hash = new Hashtable();
            _Hash.Add(CONST_COMMON.ClientInfo, _ClientInfo);
            _Hash.Add(CONST_COMMON.Meta, _MetaHash);
            String _Context = MetaDataInfo.GetClientMetaData<Hashtable>(_Hash);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(_Context);
            HttpContext.Current.Response.End();
        }

        private void GetDetailWordInfo()
        {
            _ClientInfo = new ClientInfo();
            Hashtable _MetaHash = new Hashtable();
            Hashtable _Detail = new Hashtable();
            try
            {
                String _FKey = base.GetQueryValue("FKey");
                String _ResxType = base.GetQueryValue("CultureType");

                ResxManagerInfo _ResxCtrl = new ResxManagerInfo();
                DataSet _DS = _ResxCtrl.GetDetailWordInfo(_FKey, _ResxType);
                _MetaHash.Add("DetailInfo", MetaDataInfo.GetListHashData(_DS));
                _ClientInfo.Status = true;
            }
            catch (Exception ex)
            {
                _ClientInfo.Status = false;
                _ClientInfo.Message = "数据访问失败,请稍候再试！";
            }
            Hashtable _Hash = new Hashtable();
            _Hash.Add(CONST_COMMON.ClientInfo, _ClientInfo);
            _Hash.Add(CONST_COMMON.Meta, _MetaHash);
            String _Context = MetaDataInfo.GetClientMetaData<Hashtable>(_Hash);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(_Context);
            HttpContext.Current.Response.End();
        }


        private void SaveResxInfo(String _CultureType)
        {
            _ClientInfo = new ClientInfo();
            try
            {
                String _SysName = base.GetQueryValue("txtSysName");
                String _Key = base.GetQueryValue("txtKey");
                String _txtWordCN = base.GetQueryValue("txtWordCN");
                String _txtWordTW = base.GetQueryValue("txtWordTW");
                String _txtWordEN = base.GetQueryValue("txtWordEN");

                _ClientInfo.Status = new ResxManagerInfo().AddResxInfo(_Key, _txtWordCN, _txtWordTW,
                    _txtWordEN, "admin", _SysName, _CultureType);

                if (_ClientInfo.Status)
                {
                    _ClientInfo.Message = "数据保存成功！";
                }
                else
                {
                    _ClientInfo.Message = "数据保存失败，请稍候再试！";
                }

            }
            catch (Exception ex)
            {
                _ClientInfo.Status = false;
                _ClientInfo.Message = "数据访问失败,请稍候再试！";
            }
            Hashtable _Hash = new Hashtable();
            _Hash.Add(CONST_COMMON.ClientInfo, _ClientInfo);
            String _Context = MetaDataInfo.GetClientMetaData<Hashtable>(_Hash);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(_Context);
            HttpContext.Current.Response.End();
        }

        private void CreatResxFile()
        {
            _ClientInfo = new ClientInfo();
            Hashtable _MetaHash = new Hashtable();
            try
            {
                String _SysName = base.GetQueryValue("SysName");
                ResxManagerInfo _ResxCtrl = new ResxManagerInfo();
                DataSet _DS = _ResxCtrl.GetWaitWordInfo(_SysName);
                DataSet _Detail = _ResxCtrl.GetDetailWordInfoBySysName(_SysName);

                String _SavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource\\" + _SysName);
                if (!Directory.Exists(_SavePath))
                {
                    Directory.CreateDirectory(_SavePath);
                }

                foreach (String _item in Directory.GetFiles(_SavePath))
                {
                    if (File.Exists(_item))
                    {
                        File.Delete(_item);
                    }
                }

                CreateResx _CreateResx = new CreateResx();
                _CreateResx.SavePath = _SavePath;

                _CreateResx.CreateResxInfo(_DS, _Detail);


                DataTable _Table = new DataTable();
                _Table.Columns.Add("Number");
                _Table.Columns.Add("FileName");
                _Table.Columns.Add("SysName");
                _Table.Columns.Add("CUser");
                _Table.Columns.Add("CTime");
                _Table.Columns.Add("Size");
                DataRow _NewRow = null;
                int _ICount = 0;
                FileInfo _FileInfo = null;

                foreach (String _File in Directory.GetFiles(_SavePath))
                {
                    _NewRow = _Table.NewRow();
                    _FileInfo = new FileInfo(_File);
                    _NewRow["Number"] = Convert.ToString(++_ICount);
                    _NewRow["FileName"] = _FileInfo.Name;
                    _NewRow["SysName"] = _SysName;
                    _NewRow["Size"] = this.GetSize(_FileInfo.Length);
                    _NewRow["CUser"] = "";
                    _NewRow["CTime"] = _FileInfo.LastWriteTime.ToString();
                    _Table.Rows.Add(_NewRow);
                    _Table.AcceptChanges();
                }

                _MetaHash.Add("FileInfo", MetaDataInfo.GetListHashData(_Table));
                _ClientInfo.Status = true;
            }
            catch (Exception ex)
            {
                _ClientInfo.Status = false;
                _ClientInfo.Message = "数据访问失败,请稍候再试！";
            }
            Hashtable _Hash = new Hashtable();
            _Hash.Add(CONST_COMMON.ClientInfo, _ClientInfo);
            _Hash.Add(CONST_COMMON.Meta, _MetaHash);
            String _Context = MetaDataInfo.GetClientMetaData<Hashtable>(_Hash);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(_Context);
            HttpContext.Current.Response.End();
        }


        /// <summary>
        /// 获取文件大小(KB、MB)
        /// </summary>
        /// <param name="_Size"></param>
        /// <returns></returns>
        public String GetSize(long _Size)
        {
            String _Context = "1KB";
            try
            {
                _Context = GetSizeInfo(_Size, 0);
            }
            catch (Exception ex)
            {

            }
            return _Context;
        }

        private String GetSizeInfo(long _Size, int _Count)
        {
            ++_Count;
            long _Max = _Size / 1024;


            if (_Max > 1024)
            {
                return GetSizeInfo(_Max, _Count);
            }
            String _Context = String.Empty;
            if (_Count >= 2)
            {
                _Context = _Max.ToString() + "MB";
            }
            else
            {
                if (_Max <= 1)
                {
                    return "1KB";
                }
                _Context = _Max.ToString() + "KB";
            }
            return _Context;

        }
    }

}