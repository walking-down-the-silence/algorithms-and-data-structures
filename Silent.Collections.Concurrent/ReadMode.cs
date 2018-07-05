namespace Silent.Collections
{
    /// <summary>
    /// Represents the read mode when requested buffer size is larger than existing buffer size
    /// </summary>
    public enum ReadMode
    {
        /// <summary>
        /// Will read nothing and return right await (none blocking operation)
        /// </summary>
        None,

        /// <summary>
        /// Will wait with timeout for buffer to be extended (blocking operation)
        /// </summary>
        Wait,

        /// <summary>
        /// Will read the available buffer
        /// </summary>
        Available
    }
}