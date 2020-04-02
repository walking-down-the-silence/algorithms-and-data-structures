using System.Collections.Generic;

namespace Silent.Collections
{
    public interface IBidirectionalCollection<T> : ICollection<T>
    {
        T Current { get; }

        T Next();

        T Previous();
    }
}
