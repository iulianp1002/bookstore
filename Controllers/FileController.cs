using BookstoreAPI.Models;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        const string FILE_PATH = @"C:\Samples\";

        private readonly IWebHostEnvironment _environment;

        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        /// <summary>
        /// reading image as Base64String from
        /// </summary>
        /// <param name="theFile"></param>
        /// <returns></returns>
        [HttpPost("Upload64")]
        public IActionResult Post([FromBody] FileUploadModel theFile)
        {
            // Create unique file name
            var filePathName = FILE_PATH +
                Path.GetFileNameWithoutExtension(theFile.FileName) + "-" +
                DateTime.Now.ToString().Replace("/", "")
                .Replace(":", "").Replace(" ", "") +
                Path.GetExtension(theFile.FileName);

            // Remove file type from base64 encoding, if any
            if (theFile.FileAsBase64.Contains(","))
            {
                theFile.FileAsBase64 = theFile.FileAsBase64
                  .Substring(theFile.FileAsBase64.IndexOf(",") + 1);
            }

            // Convert base64 encoded string to binary
            theFile.FileAsByteArray = Convert.FromBase64String(theFile.FileAsBase64);

            // Write binary file to server path
            using (var fs = new FileStream(filePathName, FileMode.CreateNew))
            {
                fs.Write(theFile.FileAsByteArray, 0, theFile.FileAsByteArray.Length);
            }
            return Ok();
        }

        [HttpPost("Upload"),DisableRequestSizeLimit]
        public async Task<ActionResult> Upload()
        {
            try
            {
                //var requestFile = formFile;
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        
                    }
                    return Ok(new { fileName });
                    
                //return "succes" + file != null ? "/uploads/" + file.FileName : null;
                }
                else
                {
                    return BadRequest("");
                    //return "failed";
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
                //return "fatal error!";
            }
        }


        [HttpPut("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile formFile,string bookCode)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                string Filepath = GetFilepath(bookCode);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                string imagepath = Filepath + "\\" + bookCode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await formFile.CopyToAsync(stream);
                    response.ResponseCode = 200;
                    response.Result = "pass";
                }
            }
            catch (Exception ex)
            {
                response.Errormessage = ex.Message;
            }
            return Ok(response);
        }

        [HttpPut("MultiUploadImage")]
        public async Task<IActionResult> MultiUploadImage(IFormFileCollection filecollection, string productcode)
        {
            ApiResponse response = new ApiResponse();
            int passcount = 0; int errorcount = 0;
            try
            {
                string Filepath = GetFilepath(productcode);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }
                foreach (var file in filecollection)
                {
                    string imagepath = Filepath + "\\" + file.FileName;
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        await file.CopyToAsync(stream);
                        passcount++;

                    }
                }


            }
            catch (Exception ex)
            {
                errorcount++;
                response.Errormessage = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passcount + " Files uploaded &" + errorcount + " files failed";
            return Ok(response);
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string bookCode)
        {
            string Imageurl = string.Empty;
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(bookCode);
                string imagepath = Filepath + "\\" + bookCode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    Imageurl = hosturl + "/Upload/product/" + bookCode + "/" + bookCode + ".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }

        [HttpGet("GetMultiImage")]
        public async Task<IActionResult> GetMultiImage(string productcode)
        {
            List<string> Imageurl = new List<string>();
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(productcode);

                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imagepath = Filepath + "\\" + filename;
                        if (System.IO.File.Exists(imagepath))
                        {
                            string _Imageurl = hosturl + "/Upload/product/" + productcode + "/" + filename;
                            Imageurl.Add(_Imageurl);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }

        [HttpGet("download")]
        public async Task<IActionResult> download(string productcode)
        {
            // string Imageurl = string.Empty;
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    MemoryStream stream = new MemoryStream();
                    using (FileStream fileStream = new FileStream(imagepath, FileMode.Open))
                    {
                        await fileStream.CopyToAsync(stream);
                    }
                    stream.Position = 0;
                    return File(stream, "image/png", productcode + ".png");
                    //Imageurl = hosturl + "/Upload/product/" + productcode + "/" + productcode + ".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }


        }

        [HttpGet("remove")]
        public async Task<IActionResult> remove(string productcode)
        {
            // string Imageurl = string.Empty;
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                    return Ok("pass");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }


        }

        [HttpGet("multiremove")]
        public async Task<IActionResult> multiremove(string productcode)
        {
            // string Imageurl = string.Empty;
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilepath(productcode);
                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        fileInfo.Delete();
                    }
                    return Ok("pass");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }


        }

       

        [NonAction]
        private string GetFilepath(string productcode)
        {
            return this._environment.WebRootPath + "\\Upload\\product\\" + productcode;
        }
    }
}
