using FinalProject.BLL.IServices;
using FinalProject.DAL;
using FinalProject.DAL.Repositories;
using FinalProject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.BLL.Services
{
    public class NodeService:INodeService
    {
        IFolderService _folderService;

        public NodeService(IFolderService folderService)
        {
            _folderService = folderService;
        }

        public List<Node> GetNodes()
        {
            var Folders = _folderService.GetFolders();

            var count = 0;
            var allNodes = new List<Node>();


            var layerCount = -9999999;
            //Max katman sayısını belirlemek için tüm klasörleri split ile parçalayıp en fazla katmana sahip klasörün katman sayısını buluyoruz.Ona göre döngümüz dönüyor.
            foreach (var folder in Folders)
            {
                var pathArray = folder.path.Split('/');
                var arrayLength = pathArray.Length;
                if (arrayLength > layerCount)
                {
                    layerCount = arrayLength;
                }
            }

            //katman sayısını en uzun dosya yolu içerek klasörün split edildiği zaman ki gelen ifadenin 2 eksiği olarak belirledik.
            layerCount = layerCount - 2;



            for (int j = 0; j <= layerCount; j++)
            {
                foreach (var folder in Folders)
                {
                    var pathArray = folder.path.Split('/');

                    if (count == 0)
                    {
                        allNodes = AddFirstNodes(pathArray, allNodes, count, folder);
                    }
                    if (count > 0)
                    {
                        Node currentNode = ChooseCurrentNode(pathArray, allNodes, count);
                        if (currentNode == null)
                        {
                            continue;
                        }
                        currentNode = AddChildNodes(pathArray, currentNode, count, folder);
                    }
                }
                count++;
            }

            var result = allNodes;
            return result;
        }

        public  List<Node> AddFirstNodes(string[] pathArray, List<Node> allNodes, int count, Folder folder)
        {
            AddNodeIfNotExist(allNodes, pathArray, count, folder);
            return allNodes;
        }

        public  Node AddChildNodes(string[] pathArray, Node currentNode, int count, Folder folder)
        {
            var arrayLength = pathArray.Length;
            if (count >= arrayLength)
            {
                return currentNode;
            }
            AddNodeIfNotExist(currentNode.nodes, pathArray, count, folder);
            return currentNode;
        }

        public  void AddNodeIfNotExist(List<Node> Nodes, string[] pathArray, int count, Folder folder)
        {

            var arrayLength = pathArray.Length;

            //Eğer allNodes'ların Count'u 0 sa ilk gelen path array parametresine göre bir node oluştur.
            if (Nodes.Count == 0 && pathArray[count] != "")
            {
                Nodes.Add(new Node()
                {
                    text = pathArray[count],
                });

                AddLinks(Nodes, pathArray, arrayLength, folder, count);
            }
            //Eğer daha önce oluşturulmuş bir node varsa bu allNodes lardaki node ları gez ve o isimde başka bir node varmı diye kontrol et eğer yoksa ekle.
            else
            {
                bool isThereAny = false;
                foreach (var node in Nodes)
                {
                    if (node.text == pathArray[count])
                    {
                        isThereAny = true;
                    }
                }
                if (!isThereAny && pathArray[count] != "")
                {
                    Nodes.Add(new Node()
                    {
                        text = pathArray[count]
                    });
                }

                AddLinks(Nodes, pathArray, arrayLength, folder, count);
            }
        }

        public  Node ChooseCurrentNode(string[] pathArray, List<Node> allNodes, int count)
        {
            Node currentNode = null;
            var arrayLength = pathArray.Length;

            //Eğer adım sayısı dizinin uzunluğuna eşit yada fazla ise direk null değer döndür bir işlem yapılmasın.
            if (count >= arrayLength)
            {
                return currentNode;
            }

            //Daha sonra adım sayısına göre all nodes ların yada mevcut node un içindeki nodlarda gezerek üzerinde çalışılacak node bulunur.
            for (int i = 0; i < count; i++)
            {
                var text = pathArray[i];
                if (i == 0)
                {
                    foreach (var node in allNodes)
                    {
                        if (node.text == text)
                        {
                            currentNode = node;
                        }
                    }
                }
                else
                {
                    foreach (var node in currentNode.nodes)
                    {
                        if (node.text == text)
                        {
                            currentNode = node;
                        }
                    }
                }
            }
            return currentNode;
        }

        public  void AddLinks(List<Node> Nodes, string[] pathArray, int arrayLength, Folder folder,int count)
        {
            foreach (var node in Nodes)
            {
                if (node.text == pathArray[arrayLength - 2] && (arrayLength - 2) == count)
                {
                    node.nodes.Add(new Node() {  link = new Link() { value= "http://localhost:2525/api/folder/"+folder.id.ToString(), type=folder.id } , text= folder.fileName });
                }
            }
        }
    }
}
