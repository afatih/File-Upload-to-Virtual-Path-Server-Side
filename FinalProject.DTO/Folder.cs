using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DTO
{
    public class Folder
    {

        public int id { get; set; }
        public string path { get; set; }
        public string fileName { get; set; }
        public byte[] folder { get; set; }
        public string contentType { get; set; }
    }
}
