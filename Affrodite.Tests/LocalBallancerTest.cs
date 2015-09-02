using Afrodite.Connection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Afrodite.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AffroditeP2P;
using Afrodite.Concrete;
using Moq;

namespace Affrodite.Tests
{
    /// <summary>
    ///This is a test class for LocalBallancerTest and is intended
    ///to contain all LocalBallancerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LocalBallancerTest
    {
        [TestMethod]
        public void LocalBallancerConstructorTest()
        {
            var moq = new MockRepository(MockBehavior.Default);

            var ballancer = new LocalBallancer<int>(moq.OneOf<IBallancerTask<int>>(), moq.OneOf<IPerformanceManager>(),
                moq.OneOf<IRemoteMachinesManager>(), 1);
            Assert.IsNotNull(ballancer);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void LocalBallancerConstructorTestNull1()
        {
            var moq = new MockRepository(MockBehavior.Default);

            var ballancer = new LocalBallancer<int>(null, moq.OneOf<IPerformanceManager>(),
                moq.OneOf<IRemoteMachinesManager>(), 1);
            Assert.IsNotNull(ballancer);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void LocalBallancerConstructorTestNull2()
        {
            var moq = new MockRepository(MockBehavior.Default);

            var ballancer = new LocalBallancer<int>(moq.OneOf<IBallancerTask<int>>(), null,
                moq.OneOf<IRemoteMachinesManager>(), 1);
            Assert.IsNotNull(ballancer);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void LocalBallancerConstructorTestNull3()
        {
            var moq = new MockRepository(MockBehavior.Default);

            var ballancer = new LocalBallancer<int>(moq.OneOf<IBallancerTask<int>>(), moq.OneOf<IPerformanceManager>(),
                null, 1);
            Assert.IsNotNull(ballancer);
        }

        [TestMethod]
        public void LocalBallancerStartTest()
        {
            var machineMgrMoq = new Mock<IRemoteMachinesManager>();
            var moq = new MockRepository(MockBehavior.Default);
            const int machCount = 7;
            machineMgrMoq.Setup(manager => manager.UnaviableHosts()).Returns(new[] { 2,5,6 });
            machineMgrMoq.Setup(m => m.Count).Returns(machCount);

            int ijk = 0;
            var recived = new List<int>();
            int tmp = 0;

            var ballancerTask = new BallancerDelagateTask<int?>(i =>
            {
                if (ijk != 100) return new int?[] {ijk++};
                return Enumerable.Empty<int?>();
            }, i =>
            {
                if (i != null) recived.Add(i.Value);
                else
                {
                    tmp++;
                    if (tmp > ijk)
                    lock (this)
                    {
                        Monitor.PulseAll(this);
                        return true;
                    }
                }
                return true;
            }, 1);

            var ballancer = new LocalBallancer<int?>(ballancerTask,
                moq.OneOf<IPerformanceManager>(),
                machineMgrMoq.Object, 1);
            var task = ballancer.StartAsync();

            lock (this)
            {
                Monitor.Wait(this);
                Assert.IsTrue(recived.Count > ijk / machCount, "{0}", recived.Count);
            }

            Assert.IsNotNull(ballancer);
            Assert.IsFalse(task.IsCanceled);
            Assert.IsFalse(task.IsFaulted);
            Assert.IsFalse(task.IsCanceled);
        }

        [TestMethod]
        public void TestDisconect()
        {
            var machineMgrMoq = new Mock<IRemoteMachinesManager>();
            var moq = new MockRepository(MockBehavior.Default);
            const int machCount = 5;
            int iter = 0;
            machineMgrMoq.Setup(manager => manager.UnaviableHosts()).
                Returns((iter > 50) ? new int[0] : new []{2,3,4,5}).
                Callback(()=>iter++);
            machineMgrMoq.Setup(m => m.Count).Returns(machCount);

            int ijk = 0;
            var recived = new List<int>();
            int tmp = 0;

            var ballancerTask = new BallancerDelagateTask<int?>(i =>
            {
                if (ijk != 100) return new int?[] { ijk++ };
                return Enumerable.Empty<int?>();
            }, i =>
            {
                if (i != null) recived.Add(i.Value);
                else
                {
                    tmp++;
                    if (tmp > ijk)
                        lock (this)
                        {
                            Monitor.PulseAll(this);
                            return true;
                        }
                }
                return true;
            }, 1);

            var ballancer = new LocalBallancer<int?>(ballancerTask,
                moq.OneOf<IPerformanceManager>(),
                machineMgrMoq.Object, 1);
            var task = ballancer.StartAsync();

            lock (this)
            {
                Monitor.Wait(this);
                Assert.IsTrue(recived.Count > 49 + (ijk - 50)/ machCount, "{0}", recived.Count);
            }

            Assert.IsNotNull(ballancer);
            Assert.IsFalse(task.IsCanceled);
            Assert.IsFalse(task.IsFaulted);
            Assert.IsFalse(task.IsCanceled);
        }
    }
}