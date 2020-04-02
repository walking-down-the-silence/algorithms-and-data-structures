using System;

namespace Silent.Collections
{
    public class ManhattanDistance<T> : IVertexDistanceMesureable<T> where T : IEquatable<T>, IPosition
    {
        public int MesureDistance(Vertex<T> source, Vertex<T> target)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (target is null) throw new ArgumentNullException(nameof(target));

            return Math.Abs(source.Value.X - target.Value.X) + Math.Abs(source.Value.Y - target.Value.Y);
        }
    }
}
