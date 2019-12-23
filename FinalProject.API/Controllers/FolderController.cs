using FinalProject.BLL.IServices;
using FinalProject.BLL.Services;
using FinalProject.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FinalProject.API.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FolderController : ApiController
    {
        INodeService _nodeService;
        IFolderService _folderService;

        public FolderController(INodeService nodeService,IFolderService folderService)
        {
            _nodeService = nodeService;
            _folderService = folderService;
        }

        [HttpGet]
        [Route("api/nodes")]
        public IEnumerable<Node> GetNodes()
        {
            var nodes = _nodeService.GetNodes();
            return nodes;
        }


        [HttpGet]
        [Route("api/folders")]
        public IEnumerable<Folder> GetFolders()
        {
            var folders = _folderService.GetFolders();
            return folders;
        }


        [HttpGet]
        [Route("api/folder/{id}")]
        public HttpResponseMessage GetFile(int id)
        {
            try
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK);


                var folder = _folderService.GetFolder(id);
                if (folder==null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                var fileMemStream = new MemoryStream(folder.folder);
                result.Content = new StreamContent(fileMemStream);

                var headers = result.Content.Headers;
                headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                headers.ContentDisposition.FileName = folder.fileName;
                headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

          


        }

        [HttpPost]
        [Route("api/folder")]
        public IHttpActionResult UploadFile()
        {
            var result = 0;
            //https://www.youtube.com/watch?v=aX6U2cY7fvg  sitesindeki gibi validasyonlar eklenecek
            if (HttpContext.Current.Request.Files.Count>0)
            {
                try
                {
                    foreach (var fileNameKey in HttpContext.Current.Request.Files.AllKeys)
                    {
                        HttpPostedFile file = HttpContext.Current.Request.Files[fileNameKey];

                        var fileName = Path.GetFileName(file.FileName);
                        var contentType = file.ContentType;
                        using (Stream fs = file.InputStream)
                        {
                            using (BinaryReader br = new BinaryReader(fs))
                            {
                                byte[] bytes = br.ReadBytes((int)fs.Length);
                                var path = "";

                                #region Body içerisinde sadece dosyayı kabul ettiği için dosya yolunu header içinde gönderdik.
                                IEnumerable<string> customJsonInputString;
                                Request.Headers.TryGetValues("path", out customJsonInputString);
                                var customJsonInputArray = customJsonInputString.ToArray();
                                path = customJsonInputArray[0];
                                if (string.IsNullOrEmpty(path))
                                {
                                    return BadRequest();
                                }
                                #endregion

                                var folder = new Folder() { fileName = fileName, path = path, folder = bytes, contentType = contentType };


                                result = _folderService.SaveFolder(folder);
                            }
                        }
                    }
                    if (result>0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return InternalServerError();
                    }
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return BadRequest();
            }
          
        }


        [HttpDelete]
        [Route("api/folder/{id}")]
        public IHttpActionResult DeleteFile(int id)
        {
            var result = _folderService.DeleteFolder(id);
            if (result>0)
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }






    }
}
