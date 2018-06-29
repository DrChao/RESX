using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.IO;
using System.Threading;
using System.Globalization;
using Language.Resource;
namespace Language.Common
{
    public class CreateResx
    {

        public String SavePath { get; set; }

        private List<ResxModel> _lstResxModel = null;

        private Dictionary<String, List<ResxModel>> _DirCollects = null;

        public void CreateResxInfo(DataSet _DS, DataSet _Detail)
        {
            if (_DS != null && _DS.Tables.Count > 0 && _DS.Tables[0].Rows.Count > 0)
            {
                _DirCollects = new Dictionary<string, List<ResxModel>>();
                ResxModel _Model = null;
                foreach (DataRow _Row in _DS.Tables[0].Rows)
                {
                    String _FKey = _Row["FKey"].ToString().Trim();
                    foreach (DataRow _subRow in _Detail.Tables[0].Select("FKey='" + _FKey + "'"))
                    {
                        _lstResxModel = new List<ResxModel>();
                        _Model = new ResxModel();

                        String _Key = _subRow["FKey"].ToString().Trim();
                        String _ResxType = _subRow["Resx_Type"].ToString().Trim();
                        String _ResxText = _subRow["Resx_Text"].ToString().Trim();
                        Boolean _IsJScript = Convert.ToBoolean(_Row["Is_JScript"].ToString().Trim());

                        _Model.FKey = _Key;
                        _Model.ResxType = _ResxType;
                        _Model.ResxText = _ResxText;
                        _Model.SavePath = this.SavePath;
                        _Model.IsJScript = _IsJScript;

                        if (_DirCollects.Keys.Contains(_ResxType))
                        {
                            foreach (KeyValuePair<String, List<ResxModel>> item in _DirCollects)
                            {
                                if (String.Equals(item.Key, _ResxType, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    item.Value.Add(_Model);
                                }
                            }
                        }
                        else
                        {
                            _lstResxModel.Add(_Model);
                            _DirCollects.Add(_ResxType, _lstResxModel);
                        }

                    }

                }
            }

            if (_DirCollects == null)
            {
                return;
            }

            new ResxContoller().AddResource(_DirCollects);
            this.CreateJScriptResx(_DirCollects);
        }


        private void CreateJScriptResx(Dictionary<String, List<ResxModel>> _DirCollects)
        {
            StringBuilder _CNBuilder = new StringBuilder();
            StringBuilder _TWBuilder = new StringBuilder();
            StringBuilder _ENBuilder = new StringBuilder();
            StringBuilder _JPBuilder = new StringBuilder();


            foreach (KeyValuePair<String, List<ResxModel>> _item in _DirCollects)
            {
                String _ResxType = _item.Key;

                foreach (ResxModel _Model in _item.Value)
                {
                    String _Key = _Model.FKey;
                    String _ResxText = _Model.ResxText;

                    if (_ResxText.IndexOf("\"") > -1)
                    {
                        _ResxText = _ResxText.Replace("\"", "'");
                    }

                    if (String.Equals(_ResxType, CultureType.ZH_CN, StringComparison.CurrentCultureIgnoreCase))
                    {
                        _CNBuilder.Append(String.Format("'{0}':\"{1}\",", _Key, _ResxText));
                    }
                    if (String.Equals(_ResxType, CultureType.ZH_TW, StringComparison.CurrentCultureIgnoreCase))
                    {
                        _TWBuilder.Append(String.Format("'{0}':\"{1}\",", _Key, _ResxText));
                    }

                    if (String.Equals(_ResxType, CultureType.EN_US, StringComparison.CurrentCultureIgnoreCase))
                    {
                        _ENBuilder.Append(String.Format("'{0}':\"{1}\",", _Key, _ResxText));
                    }

                    if (String.Equals(_ResxType, CultureType.JA_JP, StringComparison.CurrentCultureIgnoreCase))
                    {
                        _JPBuilder.Append(String.Format("'{0}':\"{1}\",", _Key, _ResxText));
                    }
                }

            }


            try
            {
                FileStream _FileStream = new FileStream(Path.Combine(this.SavePath, "language.js"), FileMode.Create);
                StreamWriter _Writer = new StreamWriter(_FileStream);
                String _Temp = String.Empty;
                if (!String.IsNullOrEmpty(_CNBuilder.ToString()))
                {
                    _Temp = _CNBuilder.ToString();
                    if (_Temp.EndsWith(","))
                    {
                        _Temp = _Temp.Substring(0, _Temp.Length - 1);
                    }
                    _Writer.WriteLine("var " + CultureType.ZH_CN.Replace("-", "_").ToLower().Trim()
                        + "={" + _Temp + "}");
                }
                if (!String.IsNullOrEmpty(_TWBuilder.ToString()))
                {
                    _Temp = _TWBuilder.ToString();
                    if (_Temp.EndsWith(","))
                    {
                        _Temp = _Temp.Substring(0, _Temp.Length - 1);
                    }
                    _Writer.WriteLine("var " + CultureType.ZH_TW.Replace("-", "_").ToLower().Trim()
                        + "={" + _Temp + "}");
                }
                if (!String.IsNullOrEmpty(_ENBuilder.ToString()))
                {
                    _Temp = _ENBuilder.ToString();
                    if (_Temp.EndsWith(","))
                    {
                        _Temp = _Temp.Substring(0, _Temp.Length - 1);
                    }
                    _Writer.WriteLine("var " + CultureType.EN_US.Replace("-", "_").ToLower().Trim()
                        + "={" + _Temp + "}");
                }
                if (!String.IsNullOrEmpty(_JPBuilder.ToString()))
                {
                    _Temp = _JPBuilder.ToString();
                    if (_Temp.EndsWith(","))
                    {
                        _Temp = _Temp.Substring(0, _Temp.Length - 1);
                    }
                    _Writer.WriteLine("var " + CultureType.JA_JP.Replace("-", "_").ToLower().Trim()
                        + "={" + _Temp + "}");
                }
                _Writer.Flush();
                _FileStream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public String GetCSharpCode(String _Key, String _Word)
        {
            String _Context = "ResxContoller.GetResource(\"" + _Key + "\", \"" + _Word + "\")";
            return _Context;
        }
        public String GetJScriptCode(String _Key, String _Word)
        {
            String _Context = String.Format("GetResource('{0}','{1}')", _Key, _Word);
            return _Context;
        }
    }
}
