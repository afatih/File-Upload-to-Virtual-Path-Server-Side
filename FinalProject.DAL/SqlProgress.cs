using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DAL
{
    public class SqlProgress
    {
        private readonly string connectionString ;
        public SqlProgress()
        {
            connectionString = LoadConnectionString("Default");
        }

        private string LoadConnectionString(string id)
        {
            var conn = ConfigurationManager.ConnectionStrings[id].ConnectionString;
            return conn;
        }

      

    }
}
