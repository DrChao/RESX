using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Newtonsoft;
using Newtonsoft.Json;
namespace Language.Public
{
    [Serializable]
    public class MetaDataInfo
    {
        public MetaDataInfo() { }

        /// <summary>
        /// 返回客户端元数据
        /// </summary>
        /// <typeparam name="T">抽象类型</typeparam>
        /// <param name="_Info">具体类型</param>
        /// <returns></returns>
        public static String GetClientMetaData<T>(T _Info)
        {
            String _Result = SerializeInfo.SerializeClientObject(_Info);
            return _Result;
        }

        /// <summary>
        /// 表格转JSON数据,客户端JQGrid控件调用
        /// </summary>
        /// <param name="_Table"></param>
        /// <returns></returns>
        public static String TableToJSON(DataTable _Table)
        {
            StringBuilder _StringBuilder = new StringBuilder();
            try
            {
                _StringBuilder.Append("{\"page\":1,\"total\":" + _Table.Rows.Count + ",\"records\":" + _Table.Rows.Count + ",\"rows\"");
                _StringBuilder.Append(":[");
                int _Count = 0;
                for (int i = 0; i < _Table.Rows.Count; i++)
                {
                    ++_Count;
                    _StringBuilder.Append("{\"id\":" + _Count.ToString() + ",\"cell\":[");
                    for (int j = 0; j < _Table.Columns.Count; j++)
                    {
                        _StringBuilder.Append("\"");
                        _StringBuilder.Append(_Table.Rows[i][j].ToString());
                        _StringBuilder.Append("\",");
                    }
                    _StringBuilder.Remove(_StringBuilder.Length - 1, 1);
                    _StringBuilder.Append("]},");
                }
                _StringBuilder.Remove(_StringBuilder.Length - 1, 1);
                _StringBuilder.Append("]");
                _StringBuilder.Append("}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return _StringBuilder.ToString();
        }


        /// <summary>
        /// DataRow数据行转换为HashTable信息
        /// </summary>
        /// <param name="_Row"></param>
        /// <returns></returns>
        public static Hashtable DataSetToHash(DataSet _DS)
        {
            Hashtable _Hash = null;
            try
            {
                if (_DS != null && _DS.Tables.Count > 0)
                {
                    if (_DS.Tables[0].Rows.Count > 0)
                    {
                        DataRow _Row = _DS.Tables[0].Rows[0];
                        _Hash = new Hashtable();
                        object _A = new object();
                        for (int j = 0; j < _Row.Table.Columns.Count; j++)
                        {
                            _A = _Row[j];
                            if (_Row.ItemArray[j].GetType() == typeof(System.DateTime))
                            {
                                _A = Convert.ToDateTime(_Row[j]).ToString("yyyy-MM-dd");
                            }
                            if (_Row.ItemArray[j].GetType() == typeof(System.Boolean))
                            {
                                _A = _Row[j];
                            }
                            _Hash.Add(_Row.Table.Columns[j].ToString(), _A);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return _Hash;
        }

        private static Hashtable DataRowJSON(DataRow _Row)
        {
            if (_Row == null)
            {
                return null;
            }
            Hashtable _Hash = new Hashtable();
            object _A = new object();
            for (int j = 0; j < _Row.Table.Columns.Count; j++)
            {
                _A = _Row[j];
                if (_A.GetType() == typeof(System.DBNull))
                {
                    _A = "";
                }
                else
                {
                    if (_Row.ItemArray[j].GetType() == typeof(System.DateTime))
                    {
                        _A = Convert.ToDateTime(_Row[j]).ToString("yyyy-MM-dd hh:MM:ss");
                    }
                    if (_Row.ItemArray[j].GetType() == typeof(System.Boolean))
                    {
                        _A = _Row[j];
                    }
                }
                _Hash.Add(_Row.Table.Columns[j].ToString(), _A);
            }
            return _Hash;
        }



        /// <summary>
        /// DataSet数据转化为HashTable集合信息
        /// </summary>
        /// <param name="_DS"></param>
        /// <returns></returns>
        public static List<Hashtable> GetListHashData(DataSet _DS)
        {
            List<Hashtable> _lstHash = null;
            try
            {
                if (_DS != null && _DS.Tables.Count > 0 && _DS.Tables[0].Rows.Count > 0)
                {
                    _lstHash = new List<Hashtable>();
                    foreach (DataRow _Row in _DS.Tables[0].Rows)
                    {
                        _lstHash.Add(MetaDataInfo.DataRowJSON(_Row));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return _lstHash;
        }

        /// <summary>
        /// DataTable数据转化为HashTable集合信息
        /// </summary>
        /// <param name="_Table"></param>
        /// <returns></returns>
        public static List<Hashtable> GetListHashData(DataTable _Table)
        {
            List<Hashtable> _lstHash = null;
            try
            {
                if (_Table != null && _Table.Rows.Count > 0)
                {
                    _lstHash = new List<Hashtable>();
                    foreach (DataRow _Row in _Table.Rows)
                    {
                        _lstHash.Add(MetaDataInfo.DataRowJSON(_Row));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return _lstHash;
        }

    }
}
