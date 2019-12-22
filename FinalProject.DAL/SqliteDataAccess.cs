using Dapper;
using FinalProject.DAL.DBConnection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DAL
{
    public class SqliteDataAccess<TEntity>
    {
        public  string connString = "";

        public SqliteDataAccess(ISqlProgress sqlProgress)
        {
            connString = sqlProgress.LoadConnectionString();
        }

        #region İlk yazılan dapper kodları
        public  List<TEntity> LoadPerson()
        {
            using (IDbConnection cnn = new SQLiteConnection(connString))
            {
                var output = cnn.Query<TEntity>("select * from Person", new DynamicParameters());
                return output.ToList();
            }
        }


        public  void SavePerson(TEntity person)
        {
            using (IDbConnection cnn = new SQLiteConnection(connString))
            {
                cnn.Execute("insert into Person (FirstName,LastName) values (@FirstName,@LastName)", person);
            }
        }

        #endregion


        //This method gets all record from student table    
        public  List<TEntity> ExecuteRead(string query, DynamicParameters param)
        {
            List<TEntity> students = new List<TEntity>();
            using (IDbConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                students = connection.Query<TEntity>(query, param).ToList();
                connection.Close();
            }
            return students;
        }

        //This method inserts a student record in database    
        public  int ExecuteWrite(string query,TEntity student)
        {
            using (IDbConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                var affectedRows = connection.Execute(query, student);
                connection.Close();
                return affectedRows;
            }
        }

        #region Update Delete Methods
        ////This method update student record in database    
        //private int UpdateStudent(TEntity student,string query)
        //{
        //    using (IDbConnection connection = new SQLiteConnection(connString))
        //    {
        //        connection.Open();
        //        var affectedRows = connection.Execute(query, student);
        //        connection.Close();
        //        return affectedRows;
        //    }
        //}

        ////This method deletes a student record from database    
        //private int DeleteStudent(TEntity student,string query)
        //{
        //    using (IDbConnection connection = new SQLiteConnection(connString))
        //    {
        //        connection.Open();
        //        var affectedRows = connection.Execute("Delete from Student Where Id = @Id", new { Id = studentId });
        //        connection.Close();
        //        return affectedRows;
        //    }
        //}
        #endregion





    }
}
