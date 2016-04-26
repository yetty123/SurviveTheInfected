using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

/// <summary>
/// The A* pathfinding algorithm
/// </summary>

namespace AStar
{
    public class PathFinder
    {
        private int width;
        private int height;
        private Node[,] nodes;
        private Node startNode;
        private Node endNode;
        private SearchParameters searchParameters;

		// create a new instance of PathFinder
        public PathFinder(SearchParameters searchParameters)
        {
            this.searchParameters = searchParameters;
            InitializeNodes(searchParameters.Map);
            this.startNode = this.nodes[searchParameters.StartLocation.X, searchParameters.StartLocation.Y];
            this.startNode.State = NodeState.Open;
            this.endNode = this.nodes[searchParameters.EndLocation.X, searchParameters.EndLocation.Y];
        }

		// try to find a path from the start location to the end location based on the supplied SearchParameters and
		//  returns a List of points representing the path, otherwise an empty list
        public List<Point> FindPath()
        {
            
            List<Point> path = new List<Point>();
            bool success = Search(startNode);
            if (success)
            {
                
                Node node = this.endNode;
                while (node.ParentNode != null)
                {
                    path.Add(node.Location);
                    node = node.ParentNode;
                }

                
                path.Reverse();
            }

            return path;
        }

		// builds the node grid from a simple grid of booleans indicating areas which are and aren't walkable (overridden by Unity's NavMesh)
        private void InitializeNodes(bool[,] map)
        {
            this.width = map.GetLength(0);
            this.height = map.GetLength(1);
            this.nodes = new Node[this.width, this.height];
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    this.nodes[x, y] = new Node(x, y, map[x, y], this.searchParameters.EndLocation);
                }
            }
        }

		// boolean which tells if a path to the destination node is found using a starting location
        private bool Search(Node currentNode)
        {
			// set the current node to Closed since it cannot be traversed more than once
            currentNode.State = NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);

			// sort by F-value so that the shortest possible routes are considered first
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var nextNode in nextNodes)
            {
                
                if (nextNode.Location == this.endNode.Location)
                {
                    return true;
                }
                else
                {
                    if (Search(nextNode)) 
                        return true;
                }
            }

            return false;
        }

		// returns any nodes that are adjacent to given node and may be considered to form the next step in the path
        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

				// stay within the grid's boundaries (overriden by Unity's terrain values)
                if (x < 0 || x >= this.width || y < 0 || y >= this.height)
                    continue;

                Node node = this.nodes[x, y];
                
                if (!node.IsWalkable)
                    continue;

                
                if (node.State == NodeState.Closed)
                    continue;

				// already-open nodes are only added to the list if their G-value is lower going via this route
                if (node.State == NodeState.Open)
                {
                    float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
					// if it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }

       
		// returns the eight locations immediately adjacent to given location
        private static IEnumerable<Point> GetAdjacentLocations(Point fromLocation)
        {
            return new Point[]
            {
                new Point(fromLocation.X-1, fromLocation.Y-1),
                new Point(fromLocation.X-1, fromLocation.Y  ),
                new Point(fromLocation.X-1, fromLocation.Y+1),
                new Point(fromLocation.X,   fromLocation.Y+1),
                new Point(fromLocation.X+1, fromLocation.Y+1),
                new Point(fromLocation.X+1, fromLocation.Y  ),
                new Point(fromLocation.X+1, fromLocation.Y-1),
                new Point(fromLocation.X,   fromLocation.Y-1)
            };
        }
    }
}
