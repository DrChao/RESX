using System; 
using System.Linq;
using System.Text; 
using System.Collections;
using System.Collections.Generic; 
using System.Web.Script.Serialization;
using Newtonsoft.Json;
namespace Language.Public
{
    [Serializable]
    public class SerializeInfo
    {
        public SerializeInfo() { }
        public static String SerializeClientObject<T>(T _Para)
        {
            String _Context = JavaScriptConvert.SerializeObject(_Para);
            return _Context;
        }
        public static object DerializeClientObject(String _Para, Type _Type)
        {
            return JavaScriptConvert.DeserializeObject(_Para, _Type);
        }
        public static List<T> DerializeClientsObject<T>(String _Para)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> _Object = Serializer.Deserialize<List<T>>(_Para);
            return _Object;
        }
    }
}
