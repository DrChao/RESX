using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using CPC.DataAccess;
using System.Web;

namespace Language.Common
{
    public class BaseRole
    {
        protected IRunSql runSql;

        /// <summary>
        /// 创建本地或远程访问数据库对象
        /// </summary>
        /// <returns></returns>
        protected Boolean dbConnect()
        {
            try
            {

                if (runSql != null)
                {
                    try
                    {
                        if (runSql.dbConnect())
                        {
                            return true;
                        }
                    }
                    catch
                    {
                    }
                }

                String Uri = (String)ConfigurationManager.AppSettings["WebServer"];

                if (String.IsNullOrEmpty(Uri))
                {
                    runSql = new RunSqlFCT().CreateInstance();
                }
                else
                {
                    Uri = "http://" + Uri + "/CPCDataAccess/RunSqlFCT.soap";
                    IRunSqlFCT Fct =
                           (IRunSqlFCT)System.Activator.GetObject(typeof(IRunSqlFCT), Uri);

                    HttpClientChannel client = (HttpClientChannel)ChannelServices.RegisteredChannels[0];
                    client.Properties["proxyName"] = null;

                    runSql = Fct.CreateInstance();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: The database connection is failed!");
            }
        }
    }
}
