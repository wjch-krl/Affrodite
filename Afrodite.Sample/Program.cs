using System;
using System.Threading;
using Afrodite;

namespace Afrodite.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            Configurator<int>.SetMaxPriotity(1);
            Configurator<int>.SetConfigPath("affrodite.xml");
            Configurator<int>.RegisterMasterAction(i => random.Next());
            Configurator<int>.RegisterSlaveAction(i =>
            {
                Console.WriteLine(i);
                return true;
            });
            Configurator<int>.Start();
            Thread.Sleep(100000);
        }
    }
}
