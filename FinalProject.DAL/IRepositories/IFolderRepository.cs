using FinalProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DAL.IRepositories
{
    public interface IFolderRepository
    {

        List<Folder> GetFolders();

        int SaveFolder(Folder folder);
        
        Folder GetFolder(int id);

        int DeleteFolder(Folder folder);
       
    }
}
