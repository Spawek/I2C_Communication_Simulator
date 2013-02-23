using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using I2C_Communication_Simulator;

namespace I2C_Comminicator_Unit_Tests
{
    [TestClass]
    public class ClockGeneratorTests
    {
    //    public volatile int resetTicks = 0;
    //    public volatile int writeTicks = 0;
    //    public volatile int readTicks = 0;

        [TestMethod]
        public void FixedTimeClockGeneratorTest()
        {
            FixedTimeClockGenerator generator = new FixedTimeClockGenerator(2);

            int resetTicks = 0;
            int writeTicks = 0;
            int readTicks = 0;

            generator.I2cResetTick += delegate { resetTicks++; };
            generator.I2cWriteTick += delegate { writeTicks++; };
            generator.I2cReadTick += delegate { readTicks++; };

            generator.Start();
            System.Threading.Thread.Sleep(20);
            generator.Stop();

            int expected = 10;
            int delta = 1;

            Assert.AreEqual(expected, resetTicks, delta);
            Assert.AreEqual(expected, writeTicks, delta);
            Assert.AreEqual(expected, readTicks, delta);
        }
    }
}
