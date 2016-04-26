namespace AStar
{
	/// <summary>
	/// Represents the search state of a Node
	/// </summary>
    public enum NodeState
    {
		// the node has not yet been considered in any possible paths
        Untested,

		// the node has been identified as a possible step in a path
        Open,
        
		// the node has already been included in a path and will not be considered again
        Closed
    }
}