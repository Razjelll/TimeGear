using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeGearTests
{
    [TestClass]
    public class TimerClockTest
    {

        private const int START_MINUTES = 0;
        private const int START_SECONDS = 3;

        [TestMethod]
        public void DecreaseTimeTest()
        {
            TimeGear.TimerClock clock = new TimeGear.TimerClock();
            clock.SetTime(START_MINUTES, START_SECONDS);
            clock.DecreaseTime();
            Assert.AreEqual(START_SECONDS - 1, clock.Seconds);
            Assert.AreEqual(START_MINUTES, clock.Minutes);
        }

        [TestMethod]
        public void DecreaseMinutesTest()
        {
            TimeGear.TimerClock clock = new TimeGear.TimerClock();
            clock.SetTime(1, 0);
            clock.DecreaseTime();
            Assert.AreEqual(0, clock.Minutes);
            Assert.AreEqual(59, clock.Seconds);
        }

        [TestMethod]
        public void TickClockDecreaseTest()
        {
            TimeGear.TimerClock clock1 = new TimeGear.TimerClock();
            TimeGear.TimerClock clock2 = new TimeGear.TimerClock();
            clock1.CountDown = true;
            clock1.SetTime(START_MINUTES, START_SECONDS);
            clock2.SetTime(START_MINUTES, START_SECONDS);
            clock1.TickClock();
            clock2.DecreaseTime();
            Assert.AreEqual(clock2.Minutes, clock1.Minutes);
            Assert.AreEqual(clock2.Seconds, clock1.Seconds);
        }

        [TestMethod]
        public void IncreaseTimeTest()
        {
            TimeGear.TimerClock clock = new TimeGear.TimerClock();
            clock.SetTime(START_MINUTES, START_SECONDS);
            clock.IncreaseTime();
            Assert.AreEqual(START_MINUTES, clock.Minutes);
            Assert.AreEqual(START_SECONDS + 1, clock.Seconds);
        }

        [TestMethod]
        public void IncreaseMinutesTest()
        {
            TimeGear.TimerClock clock = new TimeGear.TimerClock();
            clock.SetTime(0, 59);
            clock.IncreaseTime();
            Assert.AreEqual(1, clock.Minutes);
            Assert.AreEqual(0, clock.Seconds);
        }

        [TestMethod]
        public void TickClockIncrnoteeaseTest()
        {
            TimeGear.TimerClock clock1 = new TimeGear.TimerClock();
            TimeGear.TimerClock clock2 = new TimeGear.TimerClock();
            clock1.CountDown = false;
            clock1.SetTime(START_MINUTES, START_SECONDS);
            clock2.SetTime(START_MINUTES, START_SECONDS);
            clock1.TickClock();
            clock2.IncreaseTime();
            Assert.AreEqual(clock2.Minutes, clock1.Minutes);
            Assert.AreEqual(clock2.Seconds, clock1.Seconds);
        }
    }
}
