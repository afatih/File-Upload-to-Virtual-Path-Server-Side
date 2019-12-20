using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DTO
{
    public class FolderPropertiesDTO
    {
        public string fileName { get; set; }
        public string path { get; set; }
        public int[] bytes { get; set; }
        public string contentType { get; set; }
    }
}
