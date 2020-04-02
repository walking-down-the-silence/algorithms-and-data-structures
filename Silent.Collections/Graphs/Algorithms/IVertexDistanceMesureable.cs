using System;

namespace Silent.Collections
{
    public interface IVertexDistanceMesureable<T> where T : IEquatable<T>
    {
        int MesureDistance(Vertex<T> source, Vertex<T> target);
    }
}
