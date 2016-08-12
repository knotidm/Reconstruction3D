using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reconstruction3D.Models
{
    public class Transformation
    {
        public float TranslateX { get; set; }
        public float TranslateY { get; set; }
        public float TranslateZ { get; set; }
        public float RotateX { get; set; }
        public float RotateY { get; set; }
        public float RotateZ { get; set; }
        public float Depth { get; set; }
    }
}
