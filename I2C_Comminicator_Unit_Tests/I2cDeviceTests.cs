using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using I2C_Communication_Simulator;
using System.Collections.Generic;

namespace I2C_Comminicator_Unit_Tests
{
    [TestClass]
    public class I2cDeviceTests
    {
        static ManualClockGenerator clock;
        static I2cBus bus;
        static FakeI2cDevice dev;

        public class FakeI2cDevice : I2cDevice
        {
            public FakeI2cDevice(I2cBus _bus, ClockGenerator clock, byte _address)
                : base(_bus, clock, _address)
            {

            }
        }

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            clock = new ManualClockGenerator();
            bus = new I2cBusWorkingImplementation(clock);
            dev = new FakeI2cDevice(bus, clock, (byte)0x01);
        }

        [TestMethod]
        public void FakeI2cDeviceTest_ackShouldBeReceived()
        {
            //start transmission signal

            //address sending (0x01)

            //1 on ack should be ovverriden
            Assert.AreEqual(PinState.Low, bus.CurrSDA);
        }
    }
}
