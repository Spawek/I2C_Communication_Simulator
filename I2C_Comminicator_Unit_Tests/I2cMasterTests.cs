using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using I2C_Communication_Simulator;
using System.IO;
using System.Collections;

namespace I2C_Comminicator_Unit_Tests
{
    [TestClass]
    public class I2cMasterTests
    {
        public class SomeI2cMaster : I2cMaster
        {
            public SomeI2cMaster(I2cBus _bus, ClockGenerator clock, byte _address, string devName)
                : base(_bus, clock, _address, devName)
            {

            }
        }

        ManualClockGenerator clock;
        I2cBus bus;
        SomeI2cMaster dev;

        [TestInitialize]
        public void TestInit()
        {
            clock = new ManualClockGenerator();
            bus = new I2cBusWorkingImplementation(clock);
            dev = new SomeI2cMaster(bus, clock, (byte)0x01, "testMasterDevice");
        }

        [TestMethod]
        public void I2cMasterTests_SCLTransmissionChannelGenerationTest()
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

        [TestMethod]
        public void I2cMasterTests_SDAandSCLTransmissionChannelGenerationTest()
        {
            //start state
            Assert.AreEqual(PinState.High, bus.CurrSDA);
            Assert.AreEqual(PinState.High, bus.CurrSCL);

            //check if bus is not changing too early
            clock.Tick();
            Assert.AreEqual(PinState.High, bus.CurrSDA);
            Assert.AreEqual(PinState.High, bus.CurrSCL);

            //msg and address are 0xFF to don't ground SDA in any moment (actually 0x7F is max address)
            dev.WriteI2cMsg((byte)0x7F, new List<byte>(new byte[] { (byte)0xFF }));

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
                Assert.AreEqual(PinState.High, bus.CurrSDA);
                clock.Tick();
                Assert.AreEqual(PinState.High, bus.CurrSCL);
                Assert.AreEqual(PinState.High, bus.CurrSDA);
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

        [TestMethod]
        public void I2cMasterTests_CorrectAddressingTest()
        {
            byte address = (byte)0x55;
            dev.WriteI2cMsg(address, new List<byte>(new byte[] { 0xFF }));  //0x55 = 00110011 -> in 7 bit 0110011

            //transmission start
            clock.Tick(); 

            bool[] bArr = Converter.ByteToBoolArr((byte)(address << 1));
            bArr[7] = true; //write mode

            //address transmission
            for (int i = 0; i < 8; i++)
            {
                clock.Tick();
                Assert.AreEqual(PinState.Low, bus.CurrSCL);
                clock.Tick(); //byte is transmitted every 2nd tick
                Assert.AreEqual(PinState.High, bus.CurrSCL); //SCL should be always high on bit transmission
                if (bArr[i])
                {
                    Assert.AreEqual(PinState.High, bus.CurrSDA);
                }
                else
                {
                    Assert.AreEqual(PinState.Low, bus.CurrSDA);
                }
            }

            //later actions are not important for addressing test
        }

    }
}
