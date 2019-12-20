using Dapper;
using FinalProject.DAL.Repositories;
using FinalProject.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.BLL
{
    public class FolderService
    {
        FolderRepository folderRepository;

        public FolderService()
        {
            folderRepository = new FolderRepository();
        }

        public int SaveFolder(Folder folder)
        {
            var result = folderRepository.SaveFolder(folder);
            return result;
        }

        public List<Folder> GetFolders()
        {
            var folders = folderRepository.GetFolders();
            return folders;
        }

        public Folder GetFolder(int id)
        {
            var folder = folderRepository.GetFolder(id);
            return folder;
        }

        public int DeleteFolder(int id)
        {
            var selectedFolder = new Folder() { id = id };
            var result = folderRepository.DeleteFolder(selectedFolder);
            return result;
        }
    }
}
