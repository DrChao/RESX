using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
 

namespace Language.Resource
{
    /// <summary>
    /// 系统日志文件处理类
    /// Hj 2013-11-28
    /// </summary>
    [Serializable]
    internal class SysLog
    {

        /// <summary>
        /// 保存错误信息及错误来源
        /// </summary>
        /// <param name="message">错误提示信息</param>
        /// <param name="trace">错误来源</param>
        public static void Log(string message, string trace)
        {
            Log(string.Empty, message, trace);
        }

        /// <summary>
        /// 保存错误信息及错误来源
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="message">错误提示消息</param>
        /// <param name="trace">错误来源</param>
        public static void Log(string fileName, string message, string trace)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                string strName = GetLogPath(fileName);
                lock (typeof(SysLog))
                {
                    fs = new FileStream(strName, FileMode.Append, FileAccess.Write, FileShare.None);
                    sw = new StreamWriter(fs);

                    sw.WriteLine(System.DateTime.Now.ToString() + "：");
                    sw.WriteLine("Message：" + message);
                    sw.WriteLine("Trace：" + trace);
                    sw.WriteLine("-------------------------------------------------------------------------------");
                    sw.Flush();

                    sw.Close();
                    fs.Close();
                    sw = null;
                    fs = null;
                }
            }
            catch (Exception ex)
            {
                if (null != sw) sw.Close();
                if (null != fs) fs.Close();
            }
        }

        /// <summary>
        /// 获取本地保存错误日志文件路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetLogPath(string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return Path.Combine(path, "Log.txt");
            }
            else
            {
                return Path.Combine(path, fileName.Trim());
            }
        }

    }
}
