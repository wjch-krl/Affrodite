using System.IO;
using System.Xml.Serialization;
using Afrodite.Abstract;

namespace Afrodite.Concrete
{
    public class ConfigFileReader : IConfigReader
    {
        private readonly string path;

        public ConfigFileReader(string path)
        {
            this.path = path;
        }

        public LocalHost ReadConfig()
        {
            using (var fileReader = new StreamReader(path))
            {
                var serializer = new XmlSerializer(typeof(LocalHost));
                return (LocalHost)serializer.Deserialize(fileReader);
            }
        }

        public bool SaveConfig(LocalHost localHost)
        {
            using (var textWriter = new StreamWriter(path))
            {
                var serializer = new XmlSerializer(typeof(LocalHost));
                serializer.Serialize(textWriter,localHost);
            }
            return true;
        }
    }
}