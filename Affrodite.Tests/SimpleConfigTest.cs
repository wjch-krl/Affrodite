using AffroditeP2P;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Moq;

namespace Affrodite.Tests
{
    /// <summary>
    ///This is a test class for SimpleConfigTest and is intended
    ///to contain all SimpleConfigTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SimpleConfigTest
    {
        /// <summary>
        ///A test for SimpleConfig Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof (ArgumentNullException))]
        public void SimpleConfigConstructorTest()
        {
            SimpleConfig target = new SimpleConfig(null);
            Assert.IsNull(target);
        }


        [TestMethod()]
        [ExpectedException(typeof (FileNotFoundException))]
        public void StrartLoadBallancerTestNoConf()
        {
            var moq = new MockRepository(MockBehavior.Default);

            var config = new SimpleConfig(@"nonfile.xml");
            config.StrartLoadBallancer(moq.OneOf<IBallancerTask<int>>());
        }

        [TestMethod()]
        [DeploymentItem("Affrodite.Tests\\affrodite.xml")]
        public void StrartLoadBallancerTest()
        {
            var config =
                new SimpleConfig(@"C:\Users\Wojciech.Krol\Documents\Affrodite\Affrodite\Affrodite.Tests\affrodite.xml");
            int ijk = 0;
            var recived = new List<int>();
            config.StrartLoadBallancer(i =>
            {
                if (ijk != 5) return new int?[] {ijk++};
                return Enumerable.Empty<int?>();
            }, i =>
            {
                if (i != null) recived.Add(i.Value);
                else
                {
                        lock (recived)
                        {
                            Monitor.PulseAll(recived);
                            return true;
                        }
                }
                return true;
            }, 1);
            lock (recived)
            {
                Monitor.Wait(recived);
                Assert.AreEqual(recived.Count, ijk);
            }
        }


    }
}