using Dapper;
using FinalProject.BLL.IServices;
using FinalProject.DAL.DBConnection;
using FinalProject.DAL.IRepositories;
using FinalProject.DAL.Repositories;
using FinalProject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.BLL.Services
{
    public class FolderService:IFolderService
    {
        IFolderRepository _folderRepository;

        public FolderService(ISqlProgress sqlProgress,IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public int SaveFolder(Folder folder)
        {
            var result = _folderRepository.SaveFolder(folder);
            return result;
        }

        public List<Folder> GetFolders()
        {
            var folders = _folderRepository.GetFolders();
            return folders;
        }

        public Folder GetFolder(int id)
        {
            var folder = _folderRepository.GetFolder(id);
            return folder;
        }

        public int DeleteFolder(int id)
        {
            var selectedFolder = new Folder() { id = id };
            var result = _folderRepository.DeleteFolder(selectedFolder);
            return result;
        }
    }
}
