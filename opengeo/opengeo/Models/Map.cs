using System;
using System.Collections.Generic;

namespace opengeo.Models
{
    public partial class Map
    {
        public Map()
        {
            Layer = new HashSet<Layer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double CenterLat { get; set; }
        public double CenterLong { get; set; }
        public string Crs { get; set; }
        public int Zoom { get; set; }
        public int BasemapId { get; set; }

        public virtual Basemap Basemap { get; set; }
        public virtual ICollection<Layer> Layer { get; set; }
    }
}
