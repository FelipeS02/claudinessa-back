using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claudinessa.Data
{
    public class MySqlConfig
    {
        public string ConnectionString { get; set; }

        public MySqlConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
