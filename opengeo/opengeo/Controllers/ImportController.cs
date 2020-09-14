using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using opengeo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.IO;
using System.Text;

using OSGeo.GDAL;
using OSGeo.OGR;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace opengeo.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class ImportController : ControllerBase
  {
    private readonly gisContext _context;
    private string _temp_storage_path = "C:\\Users\\Tim\\Desktop\\";

    public ImportController(IConfiguration config, gisContext context)
    {
      _context = context;
      _temp_storage_path = config.GetValue<string>("TempStoragePath");
      Gdal.AllRegister();
      Ogr.RegisterAll();
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

    protected int ImportShapefile(string filepathname)
    {
      int ret = -1;
      // read file and build object
      var driver = Ogr.GetDriverByName("ESRI Shapefile");
      var datasource = driver.Open(@"C:\Users\Tim\Desktop\Manhattan - Original\ManhattanPoint.shp", 0);
      var layer_index = datasource.GetLayerCount();

      // for each layer in file/datasource...
      while (layer_index-- > 0)
      {
        var layer = datasource.GetLayerByIndex(layer_index);
        GeojsonLayer geojson_layer = new GeojsonLayer();
        geojson_layer.Crs = "{ \"type\": \"name\", \"properties\": { \"name\": \"urn: ogc: def: crs: OGC: 1.3:CRS84\" } }";
        geojson_layer.Type = "FeatureCollection";
        geojson_layer.Name = layer.GetName();
        geojson_layer.Description = layer.GetName();

        // for each feature in layer...
        var feature_def = layer.GetLayerDefn();
        var feature = layer.GetNextFeature();
        while (feature != null)
        {
          GeojsonFeature geojson_feature = new GeojsonFeature();

          // set Type to default
          geojson_feature.Type = "Feature";

          // get OSGeo.OGR.Geometry object form datasource and convert to WKT
          // convert WKT string to NetTopologySuite.Geometries.Geometry
          // and use that to set the Geom property of the object 
          var geom = feature.GetGeometryRef();
          string wkt = null;
          geom.ExportToIsoWkt(out wkt);
          WKTReader wktReader = new WKTReader();
          geojson_feature.Geom = wktReader.Read(wkt);

          // build properties json string and use it to set Properties field
          int next_field_index = feature.GetFieldCount();
          string properties_json = "{";
          // for each field in properties list
          while (next_field_index-- > 0)
          {
            var field_defn = feature_def.GetFieldDefn(next_field_index);
            var field_name = field_defn.GetName();
            var field_type = field_defn.GetFieldType();
            var field_type_name = field_defn.GetFieldTypeName(field_type);

            if (field_type == FieldType.OFTInteger)
            {
              int field_val = feature.GetFieldAsInteger(field_name);
              properties_json += "\"" + field_name + "\":\"" + field_val + "\" ";
            }
            if (field_type == FieldType.OFTInteger64)
            {
              long field_val = feature.GetFieldAsInteger64(field_name);
              properties_json += "\"" + field_name + "\":\"" + field_val + "\" ";
            }
            else if (field_type == FieldType.OFTReal)
            {
              double field_val = feature.GetFieldAsDouble(field_name);
              properties_json += "\"" + field_name + "\":\"" + field_val + "\" ";
            }
            else if (field_type == FieldType.OFTString)
            {
              string field_val = feature.GetFieldAsString(field_name);
              properties_json += "\"" + field_name + "\":\"" + field_val + "\" ";
            }
            else
            {
              throw new Exception("Unknown FieldType " + field_type_name);
            }
            Console.WriteLine("  " + field_name + "->" + field_type_name);
          } // end while next_field_index
          properties_json += "}";
          geojson_feature.Properties = properties_json;

          // add geojson_feature to geojson_layer
          geojson_layer.GeojsonFeature.Add(geojson_feature);

          // get next feature in layer
          feature = layer.GetNextFeature();
        } // end while feature

        // put layer in database
        using (gisContext dbContext = new gisContext())
        {
          dbContext.GeojsonLayer.Add(geojson_layer);
          dbContext.SaveChanges();
          ret = geojson_layer.Id;
        }

        // standard allows for more than one layer, be we are only going to process one for now
        break;
      } // end while layer loop
      datasource.Dispose();  // perhaps put on using statement
      driver.Dispose();
      return ret;
    }

    [HttpPost]
    public async Task<IActionResult> Shapefile(IFormFile file)
    {
      string filePath = null;
      if (file.Length > 0)
      {
        filePath = Path.Combine(_temp_storage_path, file.FileName);
        //if(filePath != null) return Ok();

        // save file
        using (var stream = System.IO.File.Create(filePath))
        {
          await file.CopyToAsync(stream);
        }
      }
      int id = ImportShapefile(filePath);
      System.IO.File.Delete(filePath);
      return Ok(id); ;
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
