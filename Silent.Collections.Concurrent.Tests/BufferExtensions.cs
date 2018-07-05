using System.Globalization;
using System.Text;

namespace Silent.Collections.Tests
{
    public static class BufferExtensions
    {
        public static string GetString(this byte[] buffer)
        {
            var bytesString = new StringBuilder();

            foreach (byte byteValue in buffer)
            {
                bytesString.Append(byteValue.ToString(CultureInfo.InvariantCulture));
            }

            return bytesString.ToString();
        }
    }
}