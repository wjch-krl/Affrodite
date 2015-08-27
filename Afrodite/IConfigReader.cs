namespace Afrodite
{
    interface IConfigReader
    {
        Config ReadConfig();
        bool SaveConfig(Config config);
    }
}
