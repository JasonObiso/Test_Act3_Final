using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Activity3
{
    class Square
    {
        Point location;
        public bool passable = true;
        public bool explored = false;

        public Square() { }

        public Square(int x, int y)
        {
            location.X = x;
            location.Y = y;
        }

        public int X
        {
            get { return location.X; }
            set { location.X = value; }
        }

        public int Y
        {
            get { return location.Y; }
            set { location.Y = value; }
        }
    }
}
