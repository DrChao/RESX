using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Web;
using System.Web.Caching;
namespace Language.Resource
{

    /// <summary>
    /// 多语言管理帮助类
    /// Create:HuangJie 2012-09-14
    /// </summary>
    public class ResxContoller : ResourceManager
    {

        public String Culture
        {
            get;
            set;
        }

        public ResxContoller()
        {
        }

        private Hashtable _FileHash = new Hashtable();
        public Hashtable FileHash
        {
            get
            {
                return _FileHash;
            }
        }

        /// <summary>
        /// 增加多语言词条
        /// 调用之前设置Thread.CurrentThread.CurrentUICulture 
        /// </summary>
        /// <param name="_Culture">多语言类型</param>
        /// <param name="_Key">词条关键字</param>
        /// <param name="_Value">词条值</param>
        /// <param name="_SavePath">保存路径</param>
        public String AddResource(String _Key, String _Value, String _SavePath)
        {

            CultureInfo _CultureInfo = Thread.CurrentThread.CurrentUICulture;

            String _DestFile = String.Empty;
            try
            {
                if (!Directory.Exists(_SavePath))
                {
                    Directory.CreateDirectory(_SavePath);
                }

                Hashtable _HashFiles = GetHashFiles();
                foreach (DictionaryEntry _Entry in _HashFiles)
                {
                    String _LanguageType = _Entry.Key.ToString().Trim();
                    String _File = _Entry.Value.ToString().Trim();
                    if (String.Equals(_LanguageType, _CultureInfo.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        _DestFile = Path.Combine(_SavePath, Path.GetFileName(_File));
                        if (!File.Exists(_DestFile))
                        {
                            File.Create(_DestFile).Dispose();
                        }
                        break;
                    }
                }

                using (ResourceWriter _ResourceWriter = new ResourceWriter(_DestFile))
                {
                    _ResourceWriter.AddResource(_Key, _Value);
                    _ResourceWriter.Generate();
                }
            }
            catch (Exception ex)
            {
                _DestFile = String.Empty;
                throw new Exception("添加多语言词条失败！", ex);
            }
            return _DestFile;
        }


        public void AddResource(Dictionary<String, List<ResxModel>> _DirCollects)
        {
            Hashtable _HashFiles = GetHashFiles();
            CultureInfo _CultureInfo = null;
            String _DestFile = String.Empty;

            foreach (KeyValuePair<String, List<ResxModel>> item in _DirCollects)
            {
                List<ResxModel> _lstModel = item.Value;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(item.Key);
                _CultureInfo = Thread.CurrentThread.CurrentUICulture;
                IResourceWriter _ResourceWriter = null;
                foreach (ResxModel _Model in _lstModel)
                {
                    if (!Directory.Exists(_Model.SavePath))
                    {
                        Directory.CreateDirectory(_Model.SavePath);
                    }
                    foreach (DictionaryEntry _Entry in _HashFiles)
                    {
                        String _LanguageType = _Entry.Key.ToString().Trim();
                        String _File = _Entry.Value.ToString().Trim();
                        if (String.Equals(_LanguageType, _CultureInfo.Name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            _DestFile = Path.Combine(_Model.SavePath, Path.GetFileName(_File));
                            if (!File.Exists(_DestFile))
                            {
                                File.Create(_DestFile).Dispose();
                            }
                            break;
                        }
                    }
                    if (_ResourceWriter == null)
                    {
                        _ResourceWriter = new ResourceWriter(_DestFile);
                    }
                    _ResourceWriter.AddResource(_Model.FKey, _Model.ResxText);
                }
                _ResourceWriter.Generate();
                _ResourceWriter.Close();
                _ResourceWriter.Dispose();
            }

        }


        /// <summary>
        /// 获取资源文件类型
        ///中文、英文、繁体
        /// </summary>
        /// <returns></returns>
        private static Hashtable GetHashFiles()
        {
            Hashtable _Hash = new Hashtable();
            String _Directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource");
            if (!Directory.Exists(_Directory))
            {
                Directory.CreateDirectory(_Directory);
            }
            _Hash.Add(CultureType.ZH_CN, Path.Combine(_Directory, String.Format("{0}.resources", CultureType.ZH_CN.ToLower())));
            _Hash.Add(CultureType.EN_US, Path.Combine(_Directory, String.Format("{0}.resources", CultureType.EN_US.ToLower())));
            _Hash.Add(CultureType.ZH_TW, Path.Combine(_Directory, String.Format("{0}.resources", CultureType.ZH_TW.ToLower())));
            _Hash.Add(CultureType.JA_JP, Path.Combine(_Directory, String.Format("{0}.resources", CultureType.JA_JP.ToLower())));
            return _Hash;
        }

        protected override ResourceSet InternalGetResourceSet(CultureInfo _Culture, Boolean _IsExists, Boolean _TryParents)
        {
            ResourceSet rs = (ResourceSet)HttpRuntime.Cache["rs" + _Culture.Name];
            if (rs == null)
            {
                string _Directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource");
                string resxFile = Path.Combine(_Directory, String.Format("{0}.resources", _Culture.Name.ToLower()));
                if (File.Exists(resxFile))
                {
                    //rs = new ResourceSet(resxFile);
                    FileStream fileStream = new FileStream(resxFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, bytes.Length);
                    fileStream.Close();
                    Stream stream = new MemoryStream(bytes);
                    ResourceSet myResource = new ResourceSet(stream);
                    HttpRuntime.Cache.Insert("rs" + _Culture.Name, myResource, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
                }
            }
            return rs;
        }


        /// <summary>
        /// 获取多语言词条内容
        /// 调用之前设置Thread.CurrentThread.CurrentUICulture 
        /// </summary>
        /// <param name="_Culture">多语言类型</param>
        /// <param name="_Key">词条关键字</param>
        /// <param name="_Value">词条值</param>
        /// <returns>如果未找到词条,返回值本身</returns>
        public static String GetResource(String _Key, String _Value)
        {
            
            CultureInfo _CultureInfo = Thread.CurrentThread.CurrentUICulture;
            String _Resut = String.Empty;
            try
            {
                ResourceManager resourceManager = new ResxContoller();
                _Resut = resourceManager.GetString(_Key, _CultureInfo);
                if (String.IsNullOrEmpty(_Resut))
                {
                    _Resut = _Value;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("获取多语言词条内容失败！", ex);
            }
            return _Resut;
        }

    }


}
