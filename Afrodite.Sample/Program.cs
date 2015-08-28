using System;
using AffroditeP2P;

namespace Afrodite.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
//            var random = new Random();
//            Configurator<int>.SetMaxPriotity(1);
//            Configurator<int>.SetConfigPath("affrodite.xml");
//            Configurator<int>.RegisterMasterAction(i => random.Next());
//            Configurator<int>.RegisterSlaveAction(i =>
//            {
//                Console.WriteLine(i);
//                return true;
//            });
//            Configurator<int>.Start();
//            Thread.Sleep(100000);

            var random = new Random();
            SimpleConfig conf = new SimpleConfig("affrodite.xml");
            var task = conf.StrartLoadBallancer<int>(i => new[] {random.Next()}, i =>
            {
                Console.WriteLine(i);
                return true;
            }, 1);
            task.Wait();
        }
    }
}