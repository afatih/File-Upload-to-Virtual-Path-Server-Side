using FinalProject.DAL.DBConnection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DAL.DBConnection
{
    public class SqlProgress:ISqlProgress
    {
        public string connectionString ;

        public string LoadConnectionString()
        {
            var conn = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            return conn;
        }

      

    }
}
