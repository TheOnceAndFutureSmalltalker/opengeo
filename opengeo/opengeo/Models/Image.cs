using System;
using System.Collections.Generic;

namespace opengeo.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Pathname { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public int? QgsFid { get; set; }
        public string MapName { get; set; }
    }
}
