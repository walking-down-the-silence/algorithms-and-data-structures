using System;
using System.IO;

namespace Silent.Collections
{
    /// <summary>
    /// Represents a concurrent circular stream
    /// </summary>
    public class CircularStream : Stream
    {
        private readonly CircularBuffer<byte> _circularBuffer;
        private bool _isClosed;

        public CircularStream(int initialCapacity, bool allowExtension, ReadMode readMode, int readTimeout)
        {
            _circularBuffer = new CircularBuffer<byte>(initialCapacity, allowExtension, readMode, readTimeout);
        }

        public CircularStream(int initialCapacity, bool allowExtension)
            : this(initialCapacity, allowExtension, ReadMode.Wait, -1)
        {
        }

        public CircularStream(int initialCapacity, ReadMode readMode, int readTimeout)
            : this(initialCapacity, true, readMode, readTimeout)
        {
        }

        public CircularStream(int initialCapacity)
            : this(initialCapacity, true, ReadMode.Wait, -1)
        {
        }

        #region Properties

        public override bool CanRead => _circularBuffer.Size > 0;

        public override bool CanSeek => false;

        public override bool CanWrite => _circularBuffer.Capacity - _circularBuffer.Size > 0;

        public override long Length => _circularBuffer.Size;

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region Overrides

        public override void Close()
        {
            _isClosed = true;
            base.Close();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _isClosed ? -1 : _circularBuffer.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!_isClosed)
            {
                _circularBuffer.Write(buffer, offset, count);
            }
        }

        #endregion

        public byte[] ToArray() => _circularBuffer.ToArray();
    }
}
