using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Afrodite.Common
{
    public interface ISerializer
    {
        byte[] SerializeToRaw<T>(T value);
        byte[] SerializeToRaw(object value);
        T Deserialize<T>(byte[] data);
        object Deserialize(byte[] data);

        string Serialize<T>(T value);
        string Serialize(object value);
        T Deserialize<T>(string data);
        object Deserialize(string data);
    }
}
