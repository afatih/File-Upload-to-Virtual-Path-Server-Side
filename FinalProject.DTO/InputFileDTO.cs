using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FinalProject.DTO
{
    public class InputFileDTO
    {
        public string path { get; set; }
        public HttpPostedFile file{ get; set; }
    }
}
