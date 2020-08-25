using System;
using System.Collections.Generic;

namespace opengeo.Models
{
    public partial class Basemap
    {
        public Basemap()
        {
            Map = new HashSet<Map>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Layers { get; set; }
        public string Format { get; set; }
        public bool? Transparent { get; set; }
        public string Service { get; set; }

        public virtual ICollection<Map> Map { get; set; }
    }
}
