using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//using System.Drawing;

namespace Hitcode_tangram
{
    public class Triangle : Polygon
    {
        public Triangle(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            Points = new Vector2[] { p0, p1, p2 };
        }
    }
}

