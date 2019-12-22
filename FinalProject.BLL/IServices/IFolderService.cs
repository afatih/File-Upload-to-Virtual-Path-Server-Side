using FinalProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.BLL.IServices
{
    public interface IFolderService
    {
        int SaveFolder(Folder folder);

        List<Folder> GetFolders();

        Folder GetFolder(int id);

        int DeleteFolder(int id);
    }
}
