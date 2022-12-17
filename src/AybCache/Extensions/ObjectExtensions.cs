using System.Text;
using System.Text.Json;

namespace AybCache.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Convert an object to a byte array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static byte[] ObjectToByteArray<T>(this T obj)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
    }

    public static object ByteArrayToObject(this byte[] bytes, Type type)
    {
        if (bytes == null)
        {
            return default;
        }

        return JsonSerializer.Deserialize(Encoding.UTF8.GetString(bytes), type);
    }
}