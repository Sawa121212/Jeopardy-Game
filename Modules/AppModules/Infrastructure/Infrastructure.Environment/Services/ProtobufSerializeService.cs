using System;
using System.IO;
using Infrastructure.Interfaces.Services;
using ProtoBuf;

namespace Infrastructure.Environment.Services
{
    /// <inheritdoc />
    public class ProtobufSerializeService : IProtobufSerializeService
    {
        private const string CannotBeNull = "can not be null";
        private const string SourceAndInstanceCannotBeNull = "Source and instance " + CannotBeNull;
        private const string SourceCannotBeNull = "Source " + CannotBeNull;
        private const string InstanceCannotBeNull = "Instance " + CannotBeNull;

        /// <inheritdoc />
        public T Deserialize<T>(string filename)
        {
            T instance;
            using (FileStream fs = new(filename, FileMode.Open, FileAccess.Read))
            {
                instance = Deserialize<T>(fs);
                fs.Flush();
            }
            return instance;
        }

        /// <inheritdoc />
        public T Deserialize<T>(Stream source)
        {
            if (source == null) throw new ArgumentException(SourceCannotBeNull);
            return Serializer.Deserialize<T>(source);
        }

        /// <inheritdoc />
        public object Deserialize(Type type, Stream source)
        {
            if (source == null) throw new ArgumentException(SourceCannotBeNull);
            return Serializer.Deserialize(type, source);
        }

        /// <inheritdoc />
        public T Deserialize<T>(byte[] source)
        {
            using (MemoryStream memStream = new(source))
            {
                return Deserialize<T>(memStream);
            }
        }

        /// <inheritdoc />
        public object Deserialize(Type type, byte[] source)
        {
            using (MemoryStream memStream = new(source))
            {
                return Deserialize(type, memStream);
            }
        }

        /// <inheritdoc />
        public T Merge<T>(Stream source, T instance)
        {
            return Serializer.Merge(source, instance);
        }

        /// <inheritdoc />
        public void Serialize<T>(string filename, T instance)
        {
            using (FileStream fs = new(filename, FileMode.Create, FileAccess.Write))
            {
                Serialize(fs, instance);
                fs.Flush();
            }
        }

        /// <inheritdoc />
        public byte[] Serialize<T>(T instance)
        {
            if (instance == null) throw new ArgumentException(InstanceCannotBeNull);
            using (MemoryStream stream = new())
            {
                Serializer.Serialize(stream, instance);
                return stream.ToArray();
            }
        }

        /// <inheritdoc />
        public void Serialize<T>(Stream destinationStream, T instance)
        {
            if (destinationStream == null || instance == null)
            {
                throw new ArgumentException(SourceAndInstanceCannotBeNull);
            }
            Serializer.Serialize(destinationStream, instance);
        }
    }
}
