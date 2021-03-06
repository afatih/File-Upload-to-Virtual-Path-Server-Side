﻿using Dapper;
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


        public  List<TEntity> ExecuteRead(string query, DynamicParameters param)
        {
            List<TEntity> models = new List<TEntity>();
            using (IDbConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                models = connection.Query<TEntity>(query, param).ToList();
                connection.Close();
            }
            return models;
        }

   
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
