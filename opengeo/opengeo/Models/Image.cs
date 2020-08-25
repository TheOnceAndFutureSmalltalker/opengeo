using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opengeo.Models
{
  public class Image
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public Guid Guid { get; set; }

    public string Pathname { get; set; }

    public string ContentType { get; set; }

    public byte[] Content { get; set; }

    public int QgsFid { get; set; }

    public string MapName { get; set; }
  }
}
