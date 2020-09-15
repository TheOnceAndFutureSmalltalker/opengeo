using System;
using System.Collections.Generic;

namespace opengeo.Models
{
    public partial class LayerStyles
    {
        public int Id { get; set; }
        public int LayerId { get; set; }
        public string ApplyRule { get; set; }
        public string Color { get; set; }
        public int? Weight { get; set; }
        public double? Opacity { get; set; }
        public string IconUrl { get; set; }

        public virtual Layer Layer { get; set; }
    }
}
