using Afrodite.Concrete;

namespace Afrodite.Abstract
{
    interface IConfigReader
    {
        LocalHost ReadConfig();
        bool SaveConfig(LocalHost localHost);
    }
}
