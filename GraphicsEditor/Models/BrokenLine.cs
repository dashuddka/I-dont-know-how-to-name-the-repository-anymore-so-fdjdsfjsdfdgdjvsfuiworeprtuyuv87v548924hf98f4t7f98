using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEditor.Models
{
    public class BrokenLine : Figures
    {
        public string Points { get; set; }
        public string StrokeColor { get; set; }
        public double StrokeThickness { get; set; }
    }
}
