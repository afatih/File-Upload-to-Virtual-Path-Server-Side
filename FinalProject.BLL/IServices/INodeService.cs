using FinalProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.BLL.IServices
{
    public interface INodeService
    {
        List<Node> GetNodes();

        List<Node> AddFirstNodes(string[] pathArray, List<Node> allNodes, int count, Folder folder);

        Node AddChildNodes(string[] pathArray, Node currentNode, int count, Folder folder);
   
        void AddNodeIfNotExist(List<Node> Nodes, string[] pathArray, int count, Folder folder);

        Node ChooseCurrentNode(string[] pathArray, List<Node> allNodes, int count);

        void AddLinks(List<Node> Nodes, string[] pathArray, int arrayLength, Folder folder, int count);
       
    }
}
