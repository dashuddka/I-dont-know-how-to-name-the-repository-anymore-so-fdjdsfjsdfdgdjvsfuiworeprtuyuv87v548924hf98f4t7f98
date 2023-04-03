using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GraphicsEditor.Models
{
    public class LineElement : Figures
    {
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string StrokeColor { get; set; }
        public double StrokeThickness { get; set; }
    }
}
