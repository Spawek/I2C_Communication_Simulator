using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using I2C_Communication_Simulator;
using System.Collections.Generic;

namespace I2C_Comminicator_Unit_Tests
{
    [TestClass]
    public class I2cDeviceTests
    {
        ManualClockGenerator clock;
        I2cBus bus;
        FakeI2cDevice slaveDev;
        const byte slavDevAddr = 0x70;
        FakeI2cMasterDevice masterDev;
        const byte masterDevAddr = 0x01;

        public class FakeI2cDevice : I2cDevice
        {
            public Frame lastFrameReceived = null;
            public FakeI2cDevice(I2cBus _bus, ClockGenerator clock, byte _address, string devName)
                : base(_bus, clock, _address, devName)
            {
                FrameReceived += FakeI2cDevice_FrameReceived;
            }

            void FakeI2cDevice_FrameReceived(object sender, Frame frame)
            {
                lastFrameReceived = frame;
            }

            public void EnqueueOutputMsg(byte msg)
            {
                outputMsgQueue.Enqueue(msg);
            }

        }

        public class FakeI2cMasterDevice : I2cMaster
        {
            public Frame lastFrameReceived = null;
            public FakeI2cMasterDevice(I2cBus _bus, ClockGenerator clock, byte _address, string devName)
                : base(_bus, clock, _address, devName)
            {

            }
        }

        [TestInitialize]
        public void TestInit()
        {
            clock = new ManualClockGenerator();
            bus = new I2cBusWorkingImplementation(clock);
            masterDev = new FakeI2cMasterDevice(bus, clock, masterDevAddr, "masterDev");
            slaveDev = new FakeI2cDevice(bus, clock, slavDevAddr, "slaveDev");
        }

        //I2cMasterTests_CorrectAddressingTest passing is precondition for this test to have any sense
        //in the other way addressing can be wrong, so address matching has no sense
        [TestMethod]
        public void I2cDeviceTest_ackShouldBeSendWhenAddressMatches()
        {
            masterDev.WriteI2cMsg(slavDevAddr, new List<byte>(new byte[] { 0xFF }));  //0x55 = 00110011 -> in 7 bit 0110011

            //transmission start
            clock.Tick();

            //address transmission
            for (int i = 7; i >= 0; i--)
            {
                clock.Tick();
                clock.Tick(); //byte is transmitted every 2nd tick
            }

            //go to ack sending clock
            clock.Tick();
            clock.Tick();

            //check if ack was sent
            Assert.AreEqual(PinState.Low, bus.CurrSDA);
        }

        [TestMethod]
        public void I2cDeviceTest_ackShouldntBeSendWhenAddressDoesntMatch()
        {
            byte wrongSlaveAddress = slavDevAddr + 2; //+ 2 coz why not - it just has to be different
            masterDev.WriteI2cMsg(wrongSlaveAddress, new List<byte>(new byte[] { 0xFF }));  //0x55 = 00110011 -> in 7 bit 0110011

            //transmission start
            clock.Tick();

            //address transmission
            for (int i = 7; i >= 0; i--)
            {
                clock.Tick();
                clock.Tick(); //byte is transmitted every 2nd tick
            }

            //go to ack sending clock
            clock.Tick();
            clock.Tick();

            //check if ack was sent
            Assert.AreEqual(PinState.High, bus.CurrSDA);
        }

        [TestMethod]
        public void I2cDeviceTestAndMaster_slaveDeviceReceivesCorrectMsgFromMasterWhenMsgIs0x00()
        {
            byte msgToSend = 0x00;
            masterDev.WriteI2cMsg(slavDevAddr, new List<byte>(new byte[] { msgToSend }));  //0x55 = 00110011 -> in 7 bit 0110011

            //transmission start
            clock.Tick();

            //address transmission
            for (int i = 0; i < 8; i++)
            {
                clock.Tick();
                clock.Tick(); //byte is transmitted every 2nd tick
            }

            //go to ack sending clock
            clock.Tick();
            clock.Tick();

            //check if ack was sent
            Assert.AreEqual(PinState.Low, bus.CurrSDA);
            
            //till this moment its same situation that in test before 

            //data transmission //8 bits
            for (int i = 0; i < 8; i++)
            {
                clock.Tick();
                clock.Tick();
            }

            //ack byte
            clock.Tick();
            clock.Tick();

            //end of msg byte should came here
            Assert.AreEqual(null, slaveDev.lastFrameReceived);
            clock.Tick();
            clock.Tick();
            Assert.AreNotEqual(null, slaveDev.lastFrameReceived); //frame should be received exactly in this momoent

            Assert.AreEqual(1, slaveDev.lastFrameReceived.data.Count); //exactly 1 byte should be received
            Assert.AreEqual(msgToSend, slaveDev.lastFrameReceived.data[0]);
        }

        [TestMethod]
        public void I2cDeviceTestAndMaster_slaveDeviceReceivesCorrectMsgFromMasterWhenMsgIs0xFF()
        {
            byte msgToSend = 0xFF;
            masterDev.WriteI2cMsg(slavDevAddr, new List<byte>(new byte[] { msgToSend }));  //0x55 = 00110011 -> in 7 bit 0110011

            //transmission start
            clock.Tick();

            //address transmission
            for (int i = 0; i < 8; i++)
            {
                clock.Tick();
                clock.Tick(); //byte is transmitted every 2nd tick
            }

            //go to ack sending clock
            clock.Tick();
            clock.Tick();

            //check if ack was sent
            Assert.AreEqual(PinState.Low, bus.CurrSDA);

            //till this moment its same situation that in test before 

            //data transmission //8 bits
            for (int i = 0; i < 8; i++)
            {
                clock.Tick();
                clock.Tick();
            }

            //ack byte
            clock.Tick();
            clock.Tick();

            //end of msg byte should came here
            Assert.AreEqual(null, slaveDev.lastFrameReceived);
            clock.Tick();
            clock.Tick();
            Assert.AreNotEqual(null, slaveDev.lastFrameReceived); //frame should be received exactly in this momoent

            Assert.AreEqual(1, slaveDev.lastFrameReceived.data.Count); //exactly 1 byte should be received
            Assert.AreEqual(msgToSend, slaveDev.lastFrameReceived.data[0]);
        }
    }
}
