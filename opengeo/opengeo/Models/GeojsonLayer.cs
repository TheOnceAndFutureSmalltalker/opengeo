using System;
using System.Collections.Generic;

namespace opengeo.Models
{
    public partial class GeojsonLayer
    {
        public GeojsonLayer()
        {
            GeojsonFeature = new HashSet<GeojsonFeature>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Crs { get; set; }
        public string Description { get; set; }

        public virtual ICollection<GeojsonFeature> GeojsonFeature { get; set; }
    }
}
