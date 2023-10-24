using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Activity3
{
    class PathNode
    {
        public Square location;
        public PathNode origin;

        public PathNode() { }
        public PathNode(Square location)
        {
            this.location = location;
        }
        public PathNode(Square location, PathNode origin)
        {
            this.location = location;
            this.origin = origin;
        }
    }
}
