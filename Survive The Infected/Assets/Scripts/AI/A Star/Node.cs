using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

/// <summary>
/// Represents a single node on a grid that is being searched for a path between two points
/// </summary>

namespace AStar
{
    
    public class Node
    {
        private Node parentNode;

		// the node's location in the grid
        public Point Location { get; private set; }

		// true when the node may be traversed
        public bool IsWalkable { get; set; }
        
		// cost from start to here
        public float G { get; private set; }

		// estimated cost from here to end
        public float H { get; private set; }

		// flags whether the node is open, closed or untested by the PathFinder
        public NodeState State { get; set; }

		// estimated total cost (F = G + H)
        public float F
        {
            get { return this.G + this.H; }
        }

		// gets or sets the parent node
        public Node ParentNode
        {
            get { return this.parentNode; }
            set
            {
             	this.parentNode = value;
                this.G = this.parentNode.G + GetTraversalCost(this.Location, this.parentNode.Location);
            }
        }

		// creates a new instance of Node
        public Node(int x, int y, bool isWalkable, Point endLocation)
        {
            this.Location = new Point(x, y);
            this.State = NodeState.Untested;
            this.IsWalkable = isWalkable;
            this.H = GetTraversalCost(this.Location, endLocation);
            this.G = 0;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}: {2}", this.Location.X, this.Location.Y, this.State);
        }

		// gets the distance between two points
        internal static float GetTraversalCost(Point location, Point otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaY = otherLocation.Y - location.Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
    }
}