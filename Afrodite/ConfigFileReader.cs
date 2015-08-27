using System.IO;
using System.Xml.Serialization;

namespace Afrodite
{
    class ConfigFileReader : IConfigReader
    {
        private readonly string path;

        public ConfigFileReader(string path)
        {
            this.path = path;
        }

        public Config ReadConfig()
        {
            using (var fileReader = new StreamReader(path))
            {
                var serializer = new XmlSerializer(typeof(Config));
                return (Config)serializer.Deserialize(fileReader);
            }
        }

        public bool SaveConfig(Config config)
        {
            using (var textWriter = new StreamWriter(path))
            {
                var serializer = new XmlSerializer(typeof(Config));
                serializer.Serialize(textWriter,config);
            }
            return true;
        }
    }
}