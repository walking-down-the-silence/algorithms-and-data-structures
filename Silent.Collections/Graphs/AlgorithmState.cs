namespace Silent.Collections
{
    /// <summary>
    /// AStar algorithm states while searching for the goal.
    /// </summary>
    public enum AlgorithmState
    {
        /// <summary>
        /// The AStar algorithm is still searching for the goal.
        /// </summary>
        Searching,

        /// <summary>
        /// The AStar algorithm has found the goal.
        /// </summary>
        PathFound,

        /// <summary>
        /// The AStar algorithm has failed to find a solution.
        /// </summary>
        PathDoesNotExist
    }
}
