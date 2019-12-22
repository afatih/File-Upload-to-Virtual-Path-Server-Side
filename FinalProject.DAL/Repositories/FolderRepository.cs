using Dapper;
using FinalProject.DAL.DBConnection;
using FinalProject.DAL.IRepositories;
using FinalProject.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DAL.Repositories
{
    public class FolderRepository:IFolderRepository
    {
        SqliteDataAccess<Folder> sqlFolders;
        ISqlProgress _sqlProgress;

        public FolderRepository(ISqlProgress sqlProgress)
        {
            _sqlProgress = sqlProgress;
            sqlFolders = new SqliteDataAccess<Folder>(_sqlProgress);
        }
        public List<Folder> GetFolders()
        {
            var folders = sqlFolders.ExecuteRead("select * from folders",null);
            return folders;
        }

        public int SaveFolder(Folder folder)
        {
            #region AddParameters
            //var parameters = new DynamicParameters();
            //parameters.Add("@FileName", folder.fileName, DbType.String, ParameterDirection.Input);
            //parameters.Add("@Path", folder.path, DbType.String, ParameterDirection.Input);
            //parameters.Add("@Bytes", folder.bytes, DbType.Binary, ParameterDirection.Input);
            //parameters.Add("@ContentType", folder.fileName, DbType.String, ParameterDirection.Input);
            #endregion

            var result = sqlFolders.ExecuteWrite("insert into Folders (Path,Folder,ContentType,FileName) values (@Path,@Folder,@ContentType,@FileName)", folder);
            return result;

            #region Sqlite old version
            //string constr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            //using (SQLiteConnection conn = new SQLiteConnection(connString))
            //{
            //    string query = "insert into Folders (Path,Folder,ContentType,FileName) values (@Path,@Folder,@ContentType,@FileName)";
            //    using (SQLiteCommand cmd = new SQLiteCommand(query))
            //    {
            //        cmd.Connection = conn;
            //        cmd.Parameters.AddWithValue("@FileName", folder.fileName);
            //        cmd.Parameters.AddWithValue("@Path", folder.path);
            //        cmd.Parameters.AddWithValue("@Folder", folder.bytes);
            //        cmd.Parameters.AddWithValue("@ContentType", folder.contentType);
            //        conn.Open();
            //        cmd.ExecuteNonQuery();
            //        conn.Close();
            //    }
            //}
            #endregion
        }

        public Folder GetFolder(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

            var folder = sqlFolders.ExecuteRead("select * from Folders where Id=@Id", parameters).SingleOrDefault();
            return folder;
        }

        public int DeleteFolder(Folder folder )
        {
            var result = sqlFolders.ExecuteWrite("delete from Folders where Id=@Id", folder);
            return result;

        }




    }
}
