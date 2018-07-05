using System;
using System.Threading.Tasks;
using Xunit;

namespace Silent.Collections.Tests
{
    /// <summary>
    /// Summary description for ConcurrentCircularStreamTests
    /// </summary>
    public class ConcurrentCircularStreamTests
    {
        private const int BufferInitialSize = 16;

        private byte[] CreateByteArray(byte byteValue, int count)
        {
            var bufferStub = new byte[count];

            for (int i = 0; i < count; i++)
            {
                bufferStub[i] = byteValue;
            }

            return bufferStub;
        }

        [Fact]
        public void ReadWrite_WithOneCall_HasExpectedBuffer()
        {
            // Assign
            var stream = new CircularStream(BufferInitialSize);
            
            // Act
            var bufferStub = CreateByteArray(4, 6);
            stream.Write(bufferStub, 0, bufferStub.Length);
            var bufferRemains = stream.ToArray();

            // Assert
            const string expected = "4444440000000000";
            var actual = bufferRemains.GetString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadWrite_With2SequentialCalls_HasExpectedBuffer()
        {
            // Assign
            var stream = new CircularStream(BufferInitialSize);

            // Act
            var bufferStub = CreateByteArray(4, 6);
            stream.Write(bufferStub, 0, bufferStub.Length);
            stream.Write(bufferStub, 0, bufferStub.Length);
            var bufferRemains = stream.ToArray();

            // Assert
            const string expected = "4444444444440000";
            var actual = bufferRemains.GetString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Write_With3SequentialCalls_ShouldExtendAndHaveExpectedBuffer()
        {
            // Assign
            var stream = new CircularStream(BufferInitialSize);

            // Act
            var bufferStub = CreateByteArray(4, 6);
            stream.Write(bufferStub, 0, bufferStub.Length);
            stream.Write(bufferStub, 0, bufferStub.Length);
            stream.Write(bufferStub, 0, bufferStub.Length);
            var bufferRemains = stream.ToArray();

            // Assert
            const string expected = "44444444444444444400000000000000";
            var actual = bufferRemains.GetString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadWrite_WithWriteAndRead_HasExpectedBuffer()
        {
            // Assign
            var stream = new CircularStream(BufferInitialSize);

            // Act
            var bufferStub1 = CreateByteArray(4, 12);
            stream.Write(bufferStub1, 0, bufferStub1.Length);

            var bufferOut = new byte[6];
            stream.Read(bufferOut, 0, bufferOut.Length);

            var bufferRemains = stream.ToArray();

            // Assert
            const string expected = "0000004444440000";
            var actual = bufferRemains.GetString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadWrite_WithWriteReadWrite_HasExpectedBuffer()
        {
            // Assign
            var stream = new CircularStream(BufferInitialSize);

            // Act
            var bufferStub1 = CreateByteArray(4, 12);
            stream.Write(bufferStub1, 0, bufferStub1.Length);

            var bufferOut = new byte[6];
            stream.Read(bufferOut, 0, bufferOut.Length);

            var bufferStub2 = CreateByteArray(6, 6);
            stream.Write(bufferStub2, 0, bufferStub2.Length);

            var bufferRemains = stream.ToArray();

            // Assert
            const string expected = "6600004444446666";
            var actual = bufferRemains.GetString();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Read_WithNoWritePreciding_ThrowsTimeoutException()
        {
            // Assign
            var stream = new CircularStream(BufferInitialSize, true, ReadMode.Wait, 100);

            // Act
            var bufferOut = new byte[6];
            Assert.Throws<TimeoutException>(() => stream.Read(bufferOut, 0, bufferOut.Length));
        }

        [Fact]
        public async void Read_WithWriteCallAfter_DoesNotThrowTimeoutException()
        {
            // Assign
            var stream = new CircularStream(BufferInitialSize, true, ReadMode.Wait, 1000);

            // Act
            var bufferOut = new byte[6];
            var readTask = Task.Run(() => stream.Read(bufferOut, 0, bufferOut.Length));

            var bufferStub = CreateByteArray(4, 6);
            stream.Write(bufferStub, 0, bufferStub.Length);

            int bytesRead = await readTask;
            var bufferRemains = stream.ToArray();

            // Assert
            const string expected = "4444440000000000";
            var actual = bufferRemains.GetString();
            Assert.Equal(expected, actual);
            Assert.Equal(bufferOut.Length, bytesRead);
        }
    }
}
