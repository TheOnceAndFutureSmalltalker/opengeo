using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using opengeo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.IO;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace opengeo.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class ImportController : ControllerBase
  {
    private readonly gisContext _context;


    public ImportController(gisContext context)
    {
      _context = context;
    }

    // GET: api/<ImportController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/<ImportController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    [HttpPost]
    public async Task<ActionResult> GeoJSON()
    {
      string json = "";
      using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
      {
        json = await reader.ReadToEndAsync();
      }

      System.Data.Common.DbConnection conn = _context.Database.GetDbConnection();

      SqlCommand cmd = new SqlCommand();
      cmd.CommandType = System.Data.CommandType.StoredProcedure;
      cmd.CommandText = "dbo.import_geojson";
      cmd.Connection = (SqlConnection)conn;
      cmd.Parameters.AddWithValue("@json", json);
      cmd.Parameters.Add("@layer_id", System.Data.SqlDbType.Int);
      cmd.Parameters["@layer_id"].Direction = System.Data.ParameterDirection.Output;

      int layer_id;
      try
      {
        conn.Open();
        cmd.ExecuteNonQuery();
        layer_id = Convert.ToInt32(cmd.Parameters["@layer_id"].Value);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        conn.Close();
      }

      return Content(layer_id.ToString());  // id of new geojson_layer ID
    }

    // POST api/<ImportController>
    [HttpPost]
    public void Post()
    {
      int i = 5;
    }

    // PUT api/<ImportController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ImportController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
