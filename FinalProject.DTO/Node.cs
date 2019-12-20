using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DTO
{
    public class Node
    {
        public Node()
        {
            nodes = new List<Node>();
            link = new Link();
        }

        public string text { get; set; }
        public List<Node> nodes { get; set; }
        public Link link { get; set; }
    }
}
