﻿using FinalProject.BLL;
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
        NodeService nodeService;
        FolderService folderService;

        public FolderController()
        {
            nodeService = new NodeService();
            folderService = new FolderService();
        }

        [HttpGet]
        [Route("api/nodes")]
        public async Task<List<Node>> GetNodes()
        {
            var nodes = nodeService.GetNodes();
            return nodes;
        }


        [HttpGet]
        [Route("api/folders")]
        public IEnumerable<Folder> GetFolders()
        {
            var folders = folderService.GetFolders();
            return folders;
        }


        [HttpGet]
        [Route("api/folder/{id}")]
        public HttpResponseMessage GetFile(int id)
        {
            try
            {
                //https://www.youtube.com/watch?v=zxPVmGpX07I sitesindeki gibi validasyonlar ekle
                var result = new HttpResponseMessage(HttpStatusCode.OK);

                #region GetFolder
                var folder = folderService.GetFolder(id);
                #endregion

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
                                #endregion

                                var folder = new Folder() { fileName = fileName, path = path, folder = bytes, contentType = contentType };


                                result = folderService.SaveFolder(folder);
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
            var result = folderService.DeleteFolder(id);
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