using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

namespace Activity3
{
    public partial class Form1 : Form
    {
        int side;
        int numX = 20;
        int numY = 16;
        Square[,] grid;

        Square highlighted = new Square();
        Square selected = new Square(0,0);

        //int mode = 0;
        LinkedList<PathNode> tree;
        Queue<PathNode> search;
        int exploreLimit;
        int exploreCount;

        PathNode goalNode;

        public Form1()
        {
            InitializeComponent();

            grid = new Square[numX, numY];
            exploreLimit = numX * numY;
            resetGrid();
            side = Convert.ToInt16(pictureBox1.Width / numX);

            tree = new LinkedList<PathNode>();
            tree.AddFirst(new PathNode(selected));
            search = new Queue<PathNode>();
            search.Enqueue(tree.First.Value);

        }

        public void resetGrid()
        {
            for (int x = 0; x < numX; x++)
            {
                for (int y = 0; y < numY; y++)
                {
                    grid[x, y] = new Square(x,y);
                }
            }
        }

        public bool isValid(int x, int y)
        {
            if (x >= 0 && x < numX && y >= 0 && y < numY)
                return true;
            else
                return false;
        }

        private bool hasArrived(PathNode target)
        {
            bool arrived = target.location.Equals(highlighted);
            return arrived;
        }

        private void exploreNode(PathNode target)
        {
            // UP DOWN LEFT RIGHT
            Point UP = new Point(target.location.X, target.location.Y - 1);
            Point DOWN = new Point(target.location.X, target.location.Y + 1);
            Point LEFT = new Point(target.location.X - 1, target.location.Y);
            Point RIGHT = new Point(target.location.X + 1, target.location.Y);

            Point[] direction = { UP, DOWN, LEFT, RIGHT };

            for(int i = 0; i<4 ; i++)
            {
                if (isValid(direction[i].X, direction[i].Y))
                {
                    Square check = grid[direction[i].X, direction[i].Y];
                    if (check.passable && !check.explored)
                    {
                        tree.AddLast(new PathNode(grid[direction[i].X, direction[i].Y], target));
                        search.Enqueue(tree.Last.Value);
                        grid[target.location.X, target.location.Y].explored = true;
                    }
                }
            }

            exploreCount++;
        }

        private List<PathNode> path; // New path list

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            highlighted.X = e.X / side;
            highlighted.Y = e.Y / side;

            tree = new LinkedList<PathNode>();
            tree.AddFirst(new PathNode(selected, null));
            search = new Queue<PathNode>();
            search.Enqueue(tree.First.Value);

            bool foundGoal = false;

            path = null; // Reset the path
            while (search.Count > 0 && !foundGoal)
            {
                PathNode target = search.Dequeue();

                foundGoal = hasArrived(target);
                if (foundGoal)
                {
                    goalNode = target;
                    lblPath.Text = "Path Found";

                    // Build the path
                    path = new List<PathNode>();
                    while (target != null)
                    {
                        path.Add(target);
                        target.location.explored = true; // Mark the path nodes as explored
                        target = target.origin;
                    }
                    path.Reverse();
                }
                else if (exploreCount > exploreLimit)
                    break;
                else
                {
                    exploreNode(target);
                }
            }

            pictureBox1.Refresh();

            lblMouse.Text = "X: " + e.X + " Y: " + e.Y;
            lblSquare.Text = "( " + highlighted.X + " , " + highlighted.Y + " ) ";

            listBox1.Items.Clear();
            if (path != null)
            {
                foreach (var node in path)
                {
                    listBox1.Items.Add($"-> {node.location.X}, {node.location.Y}");
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            for (int x = 0; x < numX; x++)
            {
                for (int y = 0; y < numY; y++)
                {
                    Rectangle s = new Rectangle(x * side, y * side, side, side);
                    if (x == selected.X && y == selected.Y)
                        e.Graphics.FillRectangle(Brushes.SlateBlue, s);
                    else if (x == highlighted.X && y == highlighted.Y)
                        e.Graphics.FillRectangle(Brushes.SkyBlue, s);
                    else if (path != null && path.Any(p => p.location.X == x && p.location.Y == y))
                        e.Graphics.FillRectangle(Brushes.Yellow, s); // Highlight the path
                    else
                        e.Graphics.FillRectangle(Brushes.Silver, s);
                    e.Graphics.DrawRectangle(Pens.Gray, s);
                }

                if (goalNode != null)
                {
                    e.Graphics.DrawLine(Pens.Red, selected.X * side + side / 2, selected.Y * side + side / 2,
                        goalNode.location.X * side + side / 2, goalNode.location.Y * side + side / 2);
                }
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            selected.X = e.X / side;
            selected.Y = e.Y / side;

            pictureBox1.Refresh();

            lblSelected.Text = "( " + selected.X + " , " + selected.Y + " ) ";
        }
    }
}
