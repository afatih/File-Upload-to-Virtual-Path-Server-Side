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
    public class NodeService : INodeService
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
            //Max katman sayısını belirlemek için tüm klasörleri split ile parçalayıp en fazla katmana sahip klasörün katman sayısını buluyoruz. Bu max katman sayısı kadar dosyaların içinde geziyoruz.
            foreach (var folder in Folders)
            {
                var pathArray = folder.path.Split('/');
                var arrayLength = pathArray.Length;
                if (arrayLength > layerCount)
                {
                    layerCount = arrayLength;
                }
            }

            //katman sayısını en uzun dosya yolu içeren klasörün split edildiği zaman ki gelen ifadenin 2 eksiği olarak belirledik.
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

        public List<Node> AddFirstNodes(string[] pathArray, List<Node> allNodes, int count, Folder folder)
        {
            AddNodeIfNotExist(allNodes, pathArray, count, folder);
            return allNodes.OrderBy(x => x.text).ToList();
        }

        public Node AddChildNodes(string[] pathArray, Node currentNode, int count, Folder folder)
        {
            var arrayLength = pathArray.Length;
            if (count >= arrayLength)
            {
                return currentNode;
            }
            AddNodeIfNotExist(currentNode.nodes, pathArray, count, folder);
            currentNode.nodes=currentNode.nodes.OrderBy(x => x.text).ToList();
            return currentNode;
        }

        public void AddNodeIfNotExist(List<Node> Nodes, string[] pathArray, int count, Folder folder)
        {

            var arrayLength = pathArray.Length;

            //Eğer allNodes'ların Count'u 0 sa ilk gelen path array parametresine göre bir node oluşturur.
            if (Nodes.Count == 0 && pathArray[count] != "")
            {
                Nodes.Add(new Node()
                {
                    text = pathArray[count],
                });

                //Eğer katman sayısı dosya yolu split edildikten sonra elde edilen arrayin eleman sayısının 2 eksiğiyse yani dosyanın yükleneceği en son klasöre kadar döngü döndü ise o katmana gerekli dosyayı yüklüyoruz.
                if (arrayLength - 2 == count)
                {
                    AddLinks(Nodes, pathArray, arrayLength, folder, count);
                }

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

                if (arrayLength - 2 == count)
                {
                    AddLinks(Nodes, pathArray, arrayLength, folder, count);
                }


            }
        }

        public Node ChooseCurrentNode(string[] pathArray, List<Node> allNodes, int count)
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

        public void AddLinks(List<Node> Nodes, string[] pathArray, int arrayLength, Folder folder, int count)
        {
            foreach (var node in Nodes)
            {
                //Mevcut node un text i ile girilen dosya yolundaki son dosya klasörü eşleşiyorsa o nodun nodalarına uygun formatta linki ekliyoruz.
                if (node.text == pathArray[arrayLength - 2])
                {
                    node.nodes.Add(new Node()
                    {
                        link = new Link()
                        {
                            value = "http://localhost:2525/api/folder/" + folder.id.ToString(),
                            type = folder.id
                        },
                        text = folder.fileName
                    });
                    node.nodes= node.nodes.OrderBy(x => x.text).ToList();
                }
            }
        }
    }
}
