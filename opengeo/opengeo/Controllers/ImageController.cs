using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using opengeo.Models;
using Microsoft.Data.SqlClient;

namespace opengeo.Controllers
{

  public class ImageController : Controller
  {

    private readonly IConfiguration _config;
    private readonly gisContext _context;
    private string _image_storage_path = "C:\\Users\\Tim\\Desktop\\";

    public ImageController(IConfiguration config, gisContext context)
    {
      _config = config;
      _context = context;
      _image_storage_path = _config.GetValue<string>("ImageStoragePath");
    }

    // POST: ImageController/Create
    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile image, int qgs_fid)
    {
      if (image.Length > 0)
      {
        var filePath = Path.Combine(_image_storage_path, image.FileName);
        //if(filePath != null) return Ok();

        // save file
        using (var stream = System.IO.File.Create(filePath))
        {
          await image.CopyToAsync(stream);
        }

        // save database record
        Image db_image = new Image();
        db_image.Guid = System.Guid.NewGuid();
        db_image.Name = filePath;
        db_image.ContentType = image.ContentType;
        db_image.QgsFid = qgs_fid; // Int32.Parse(qgs_fid);
        _context.Image.Add(db_image);
        await _context.SaveChangesAsync();
      }
      return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Store(IFormFile image)
    {
      Image db_image = new Image();
      db_image.Guid = System.Guid.NewGuid();
      db_image.Name = image.FileName;
      db_image.ContentType = image.ContentType;
      //if (db_image.Name != null) return Ok();
      using (MemoryStream ms = new MemoryStream())
      {
        await image.CopyToAsync(ms);
        db_image.Content = ms.ToArray();
      }

      _context.Image.Add(db_image);
      await _context.SaveChangesAsync();

      return CreatedAtAction("AddImage", new { guid = db_image.Guid.ToString() });
    }


    /// <summary>
    /// Gets image, bytes and all, from database.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<Map>> Retrieve(string guid)
    {
      var image = await _context.Image.FirstOrDefaultAsync(m => m.Guid == System.Guid.Parse(guid));

      if (image == null)
      {
        return NotFound();
      }

      return File(image.Content, image.ContentType);
    }


    /// <summary>
    /// Gets image from file but file path name of image is stored in database.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<Map>> Guid(string guid)
    {
      var image = await _context.Image.FirstOrDefaultAsync(m => m.Guid == System.Guid.Parse(guid));

      if (image == null)
      {
        return NotFound();
      }

      Byte[] bytes = System.IO.File.ReadAllBytes(image.Pathname);
      return File(bytes, "image/jpeg");
    }


    /// <summary>
    /// Gets the specified image file.
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Download(string imageName)
    {
      var filePath = Path.Combine(_image_storage_path, imageName);
      Byte[] bytes = System.IO.File.ReadAllBytes(filePath);
      return File(bytes, "image/jpeg");
    }
  }
}
