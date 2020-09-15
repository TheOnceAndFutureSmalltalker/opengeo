using System;
using System.Collections.Generic;

namespace opengeo.Models
{
    public partial class Layer
    {
        public Layer()
        {
            LayerStyles = new HashSet<LayerStyles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Layers { get; set; }
        public string Format { get; set; }
        public bool? Transparent { get; set; }
        public bool IsBasemap { get; set; }
        public int MapId { get; set; }
        public string Group { get; set; }
        public int? GroupNumber { get; set; }
        public int? LayerNumber { get; set; }
        public string StylesUrl { get; set; }
        public string LegendUrl { get; set; }
        public bool IsWfs { get; set; }
        public string WfsUrl { get; set; }
        public string Namespace { get; set; }
        public string Layer1 { get; set; }
        public string GeometryType { get; set; }

        public virtual Map Map { get; set; }
        public virtual ICollection<LayerStyles> LayerStyles { get; set; }
    }
}
