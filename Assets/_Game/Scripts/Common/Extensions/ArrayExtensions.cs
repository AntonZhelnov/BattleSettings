using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common.Extensions
{
    public static class ArrayExtensions
    {
        public static object Copy(this object input)
        {
            using var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, input);
            memoryStream.Position = 0;
            return binaryFormatter.Deserialize(memoryStream);
        }
    }
}