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

  [Route("api/[controller]")]
  [ApiController]
  public class GeoJSONController : ControllerBase
  {
    private readonly gisContext _context;

    public GeoJSONController(gisContext context)
    {
      _context = context;
    }

    // GET: api/<GeoJSONController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }

    // GET api/<GeoJSONController>/5
    [HttpGet("{layer_name}")]
    public ActionResult Get(string layer_name)
    {
      System.Data.Common.DbConnection conn = _context.Database.GetDbConnection();

      SqlCommand cmd = new SqlCommand();
      cmd.CommandType = System.Data.CommandType.StoredProcedure;
      cmd.CommandText = "dbo.get_geojson";
      cmd.Connection = (SqlConnection)conn;

      cmd.Parameters.AddWithValue("@table_name", layer_name);
      cmd.Parameters.Add("@geoJSON", System.Data.SqlDbType.NVarChar, -1);
      cmd.Parameters["@geoJSON"].Direction = System.Data.ParameterDirection.Output;

      string geoJSON = "";
      try
      {
        conn.Open();
        int i = cmd.ExecuteNonQuery();
        geoJSON = Convert.ToString(cmd.Parameters["@geoJSON"].Value);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        conn.Close();
      }

      return Content(geoJSON, "application/json; charset=utf-8");
    }


    // POST api/<GeoJSONController>
    [HttpPost("{layer_name}")]
    public async Task<ActionResult> Post(string layer_name)
    {
      System.Data.Common.DbConnection conn = _context.Database.GetDbConnection();

      SqlCommand cmd = new SqlCommand();
      cmd.CommandType = System.Data.CommandType.StoredProcedure;
      cmd.CommandText = "dbo.add_feature_2";
      cmd.Connection = (SqlConnection)conn;

      string json = "";
      using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
      {
        json = await reader.ReadToEndAsync();
      }

      cmd.Parameters.AddWithValue("@table_name", layer_name);
      cmd.Parameters.AddWithValue("@json", json);
      cmd.Parameters.Add("@fid", System.Data.SqlDbType.Int);
      cmd.Parameters["@fid"].Direction = System.Data.ParameterDirection.Output;

      int fid;
      try
      {
        conn.Open();
        cmd.ExecuteNonQuery();
        fid = Convert.ToInt32(cmd.Parameters["@fid"].Value);
      }
      catch(Exception ex)
      {
        throw ex;
      }
      finally
      {
        conn.Close();
      }

      return Content(fid.ToString());  // needs to be new qgs_fid
    }

    // PUT api/<GeoJSONController>
    [HttpPut("{layer_name}")]
    public async Task<ActionResult> Put(string layer_name)
    {
      System.Data.Common.DbConnection conn = _context.Database.GetDbConnection();

      SqlCommand cmd = new SqlCommand();
      cmd.CommandType = System.Data.CommandType.StoredProcedure;
      cmd.CommandText = "dbo.update_feature_2";
      cmd.Connection = (SqlConnection)conn;

      string json = "";
      using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
      {
        json = await reader.ReadToEndAsync();
      }

      cmd.Parameters.AddWithValue("@table_name", layer_name);
      cmd.Parameters.AddWithValue("@json", json);

      try
      {
        conn.Open();
        cmd.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        conn.Close();
      }

      return Content("Success");
    }

    // DELETE api/<GeoJSONController>/5
    [HttpDelete("{layer_name}/{fid}")]
    public ActionResult Delete(string layer_name, int fid)
    {
      System.Data.Common.DbConnection conn = _context.Database.GetDbConnection();

      SqlCommand cmd = new SqlCommand();
      cmd.CommandType = System.Data.CommandType.StoredProcedure;
      cmd.CommandText = "dbo.delete_feature";
      cmd.Connection = (SqlConnection)conn;

      cmd.Parameters.AddWithValue("@table_name", layer_name);
      cmd.Parameters.AddWithValue("@fid", fid);

      try
      {
        conn.Open();
        cmd.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        conn.Close();
      }

      return Content("8");
    }
  }
}
