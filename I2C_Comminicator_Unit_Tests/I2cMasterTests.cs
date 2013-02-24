using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using I2C_Communication_Simulator;
using System.IO;

namespace I2C_Comminicator_Unit_Tests
{
    [TestClass]
    public class I2cMasterTests
    {
        public class SomeI2cMaster : I2cMaster
        {
            public SomeI2cMaster(I2cBus _bus, ClockGenerator clock, byte _address)
                : base(_bus, clock, _address)
            {

            }
        }

        static ManualClockGenerator clock;
        static I2cBus bus;
        static SomeI2cMaster dev;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            clock = new ManualClockGenerator();
            bus = new I2cBusWorkingImplementation(clock);
            dev = new SomeI2cMaster(bus, clock, (byte)0x01);
        }

        [TestMethod]
        public void I2cMasterTests_TransmissionChannelGenerationTest()
        {
            //start state
            Assert.AreEqual(PinState.High, bus.CurrSDA);
            Assert.AreEqual(PinState.High, bus.CurrSCL);

            //check if bus is not changing too early
            clock.Tick();
            Assert.AreEqual(PinState.High, bus.CurrSDA);
            Assert.AreEqual(PinState.High, bus.CurrSCL);

            //messages and addresses in here are not important - this test is checking only channel generation
            dev.WriteI2cMsg((byte)0x02, new List<byte>(new byte[] { (byte)0x1F })); 

            //starting transmission
            clock.Tick();
            Assert.AreEqual(PinState.Low, bus.CurrSDA);
            Assert.AreEqual(PinState.High, bus.CurrSCL);

            //transmission itself
            for (int i = 0; i < 18; i++) //2 bytes * 9(with ack) = 18
            {
                //I dont care about SDA in here - SDA keeps msg - its not important for us
                clock.Tick();
                Assert.AreEqual(PinState.Low, bus.CurrSCL);
                clock.Tick();
                Assert.AreEqual(PinState.High, bus.CurrSCL);
            }
            
            //stopping transmission
            clock.Tick();
            Assert.AreEqual(PinState.Low, bus.CurrSDA);
            Assert.AreEqual(PinState.High, bus.CurrSCL);
            clock.Tick();
            Assert.AreEqual(PinState.High, bus.CurrSDA);
            Assert.AreEqual(PinState.High, bus.CurrSCL);

            //using (StreamWriter sw = new StreamWriter("I2cMasterTests_TransmissionChannelGenerationTest.txt"))
            //{
            //    sw.WriteLine("SDA\tSCL");
            //    for (int i = 0; i < 100; i++)
            //    {
            //        sw.WriteLine("{0} \t{1}", bus.CurrSDA, bus.CurrSCL);
            //        clock.Tick();
            //    }
            //}
        }

    }
}
