namespace Afrodite
{
    interface IConfigReader
    {
        IConfig ReadConfig();
        bool SaveConfig(IConfig config);
    }
}
