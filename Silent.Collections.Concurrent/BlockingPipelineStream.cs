using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace Silent.Collections
{
    public class BlockingPipelineStream : Stream
    {
        #region Private Fields

        private readonly ConcurrentQueue<byte[]> _chunks = new ConcurrentQueue<byte[]>();
        private byte[] _currentChunk;
        private int _currentChunkPosition;
        private long _internalSize;
        private bool _internalClosed;

        #endregion

        #region Properties

        public object ReadSync { get; } = new object();

        public object WriteSync { get; } = new object();

        public override bool CanRead => !_internalClosed;

        public override bool CanSeek => false;

        public override bool CanWrite => !_internalClosed;

        public override long Length => _internalSize;

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region Not Supported Methods

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

        #endregion

        public override int Read(byte[] buffer, int offset, int count)
        {
            // only one at a time is allowed to read
            lock (ReadSync)
            {
                // copy values to modify if needed
                int targetOffset = offset;
                int targetCount = count;

                while (true)
                {
                    lock (WriteSync)
                    {
                        if (_currentChunk == null)
                        {
                            // if nothing available then wait for write
                            while (!_chunks.TryDequeue(out _currentChunk))
                            {
                                Monitor.Wait(WriteSync);
                            }

                            // reset the position as new chunk was taken
                            _currentChunkPosition = 0;
                        }
                    }
                    
                    int currentChunkSize = _currentChunk.Length - _currentChunkPosition;
                    if (currentChunkSize > targetCount)
                    {
                        // chunk has more then requested
                        Buffer.BlockCopy(_currentChunk, _currentChunkPosition, buffer, targetOffset, targetCount);

                        // set the current position as chunk is not empty yet
                        _currentChunkPosition += targetCount;
                        Interlocked.Add(ref _internalSize, -targetCount);
                        break;
                    }

                    if (currentChunkSize <= targetCount)
                    {
                        // chunk has exactly as requested OR chunk has less then requested
                        Buffer.BlockCopy(_currentChunk, _currentChunkPosition, buffer, targetOffset, currentChunkSize);

                        // clear current chunk as it was fully read
                        _currentChunk = null;
                        _currentChunkPosition = 0;

                        // increment the values for further read
                        targetOffset += currentChunkSize;
                        targetCount -= currentChunkSize;
                        Interlocked.Add(ref _internalSize, -currentChunkSize);

                        // stop if read requested amount
                        if (targetCount == 0)
                            break;
                    }
                }

                return count;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // only one at a time is allowed to write
            lock (WriteSync)
            {
                // set write to not allowed and copy the buffer
                var bufferCopy = new byte[count];
                Buffer.BlockCopy(buffer, offset, bufferCopy, 0, count);

                // enqueue the buffer
                _chunks.Enqueue(bufferCopy);
                Interlocked.Add(ref _internalSize, count);

                // pulse the _commonSync to aquire lock back where waited
                Monitor.Pulse(WriteSync);
            }
        }

        public override void Close()
        {
            base.Close();

            lock (WriteSync)
            {
                Monitor.PulseAll(WriteSync);
            }

            lock (ReadSync)
            {
                Monitor.PulseAll(ReadSync);
            }

            _internalClosed = true;
        }
    }
}