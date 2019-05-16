using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect
{
    [XmlRpcUrl("common")]
    public interface IOdooCommonRpc : IXmlRpcProxy
    {
        [XmlRpcMethod("login")]
        int login(String dbName, string dbUser, string dbPwd);
    }
}
