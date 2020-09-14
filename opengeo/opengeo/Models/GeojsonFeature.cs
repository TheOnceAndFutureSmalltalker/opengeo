using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace opengeo.Models
{
    public partial class GeojsonFeature
    {
        public int Id { get; set; }
        public int? GeojsonLayerId { get; set; }
        public Geometry Geom { get; set; }
        public string Properties { get; set; }
        public string Type { get; set; }

        public virtual GeojsonLayer GeojsonLayer { get; set; }
    }
}
