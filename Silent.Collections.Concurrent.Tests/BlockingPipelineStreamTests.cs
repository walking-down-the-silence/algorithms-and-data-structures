using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Silent.Collections.Tests
{
    public class BlockingPipelineStreamTests
    {
        private readonly Random _random = new Random();
        private readonly List<IAnimal> _animals = new List<IAnimal>();

        public BlockingPipelineStreamTests()
        {
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            _animals.Add(new Firefox { Id = Guid.NewGuid().ToString(), Name = "Firefox" });
            _animals.Add(new Thunderbird { Id = Guid.NewGuid().ToString(), Name = "Thunderbird" });
            _animals.Add(new Phoenix { Id = Guid.NewGuid().ToString(), Name = "Phoenix" });
            Serializer.PrepareSerializer<IAnimal>();
        }

        #region Protocol Buffers Types

        [ProtoContract]
        [ProtoInclude(10, typeof(Firefox))]
        [ProtoInclude(11, typeof(Thunderbird))]
        [ProtoInclude(12, typeof(Phoenix))]
        private interface IAnimal
        {
            [ProtoMember(1)]
            string Id { get; set; }

            [ProtoMember(2)]
            string Name { get; set; }
        }

        [ProtoContract]
        public class Firefox : IAnimal
        {
            [ProtoMember(1)]
            public string Id { get; set; }

            public string Name { get; set; }
        }

        [ProtoContract]
        public class Thunderbird : IAnimal
        {
            [ProtoMember(1)]
            public string Id { get; set; }

            [ProtoMember(2)]
            public string Name { get; set; }
        }

        [ProtoContract]
        public class Phoenix : IAnimal
        {
            [ProtoMember(1)]
            public string Id { get; set; }

            [ProtoMember(2)]
            public string Name { get; set; }
        }

        #endregion

        private byte[] CreateRandomBytes(int count)
        {
            var bytes = new byte[count];
            _random.NextBytes(bytes);
            return bytes;
        }

        private void Serialize(BlockingPipelineStream stream, int index)
        {
            lock (stream.WriteSync)
            {
                Serializer.SerializeWithLengthPrefix(stream, _animals[index], PrefixStyle.Fixed32BigEndian);
            }
        }

        private void Deserialize(BlockingPipelineStream stream, List<IAnimal> animals)
        {
            lock (stream.ReadSync)
            {
                animals.Add(Serializer.DeserializeWithLengthPrefix<IAnimal>(stream, PrefixStyle.Fixed32BigEndian));
            }
        }

        [Theory]
        [InlineData(10, 10, true, true)]
        [InlineData(20, 20, true, true)]
        [InlineData(20, 10, true, false)]
        [InlineData(10, 20, false, true)]
        public void ReadWrite_OnMultipleThreads_ShouldWork(int writeTasksCount, int readTasksCount, bool notTimedOutExpected, bool zeroLenghtExpected)
        {
            // Assign
            var stream = new BlockingPipelineStream();
            var writeTasks = Enumerable.Range(1, writeTasksCount)
                .Select(i => new Task(() => stream.Write(CreateRandomBytes(i * 10), 0, i * 10), TaskCreationOptions.LongRunning))
                .ToList();
            var readTasks = Enumerable.Range(1, readTasksCount)
                .Select(i => new Task(() => stream.Read(new byte[i * 10], 0, i * 10), TaskCreationOptions.LongRunning))
                .ToList();

            // Act
            readTasks.ForEach(x => x.Start());
            writeTasks.ForEach(x => x.Start());
            var tasks = writeTasks.Concat(readTasks).ToArray();
            var notTimedOut = Task.WaitAll(tasks, 1000);

            // Assert
            Assert.Equal(notTimedOutExpected, notTimedOut);
            Assert.Equal(zeroLenghtExpected, stream.Length == 0);
        }

        [Theory]
        [InlineData(10, 10, true, 10)]
        [InlineData(30, 30, true, 30)]
        [InlineData(30, 20, true, 20)]
        [InlineData(20, 30, false, 20)]
        public void ReadWrite_WithProtocolBuffers_ShouldWork(int writeTasksCount, int readTasksCount, bool notTimedOutExpected, int animalsExpected)
        {
            // Assign
            var stream = new BlockingPipelineStream();
            var animals = new List<IAnimal>();
            var writeTasks = Enumerable.Range(0, writeTasksCount)
                .Select(i => new Task(() => Serialize(stream, i), TaskCreationOptions.LongRunning))
                .ToList();
            var readTasks = Enumerable.Range(0, readTasksCount)
                .Select(i => new Task(() => Deserialize(stream, animals), TaskCreationOptions.LongRunning))
                .ToList();

            // Act
            readTasks.ForEach(x => x.Start());
            writeTasks.ForEach(x => x.Start());
            var tasks = writeTasks.Concat(readTasks).ToArray();
            var notTimedOut = Task.WaitAll(tasks, 1000);

            // Assert
            Assert.Equal(notTimedOutExpected, notTimedOut);
            Assert.Equal(animalsExpected, animals.Count);
        }
    }
}
