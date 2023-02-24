using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace check_yo_self_api.Configuration
{
    public class ConnectionStringKeys
    {
        public const string SqlServer = "Data:SqlServer:ConnectionString";
        public const string MySql = "Data:MySql:ConnectionString";
        public const string SqlLite = "Data:SqlLite:ConnectionString";
        public const string MySqlVersion = "Data:MySql:Version";
    }
}
