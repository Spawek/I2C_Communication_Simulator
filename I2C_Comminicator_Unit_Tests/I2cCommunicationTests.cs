using System;
using System.Collections.Generic;
using I2C_Communication_Simulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace I2C_Comminicator_Unit_Tests
{
    [TestClass]
    public class I2cCommunicationTests
    {
        ManualClockGenerator clock;
        I2cBus bus;
        FakeI2cDevice slaveDev;
        const byte slavDevAddr = 0x70;
        FakeI2cMasterDevice masterDev;
        const byte masterDevAddr = 0x01;

        const int nearlyInfiniteNoOfTicks = 1000; //:D

        public class FakeI2cDevice : I2cDevice
        {
            public Frame lastFrameReceived = null; //TODO: remove it
            public List<Frame> receivedFrames = new List<Frame>();
            public FakeI2cDevice(I2cBus _bus, ClockGenerator clock, byte _address, string devName)
                : base(_bus, clock, _address, devName)
            {
                FrameReceived += FakeI2cDevice_FrameReceived;
            }

            void FakeI2cDevice_FrameReceived(object sender, Frame frame)
            {
                lastFrameReceived = frame;
                receivedFrames.Add(frame);
            }

            public void EnqueueOutputMsg(byte msg)
            {
                outputMsgQueue.Enqueue(msg);
            }

        }

        public class FakeI2cMasterDevice : I2cMaster
        {
            public Frame lastFrameReceived;
            public List<Frame> receivedFrames = new List<Frame>();
            public FakeI2cMasterDevice(I2cBus _bus, ClockGenerator clock, byte _address, string devName)
                : base(_bus, clock, _address, devName)
            {
                FrameReceived += FakeI2cDevice_FrameReceived;
            }

            void FakeI2cDevice_FrameReceived(object sender, Frame frame)
            {
                lastFrameReceived = frame;
                receivedFrames.Add(frame);
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

        [TestMethod]
        public void MasterSends0xFF_1ByteMsgToSlaveTest()
        {
            List<byte> msgContent = new List<byte> { 0xFF };
            masterDev.WriteI2cMsg(slavDevAddr, msgContent);

            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            Assert.AreEqual(msgContent.Count, slaveDev.lastFrameReceived.data.Count);
            for (int i = 0; i < slaveDev.lastFrameReceived.data.Count; i++)
			{
                Assert.AreEqual(msgContent[i], slaveDev.lastFrameReceived.data[i]);
			}
        }

        [TestMethod]
        public void MasterSends0x00_1ByteMsgToSlaveTest()
        {
            List<byte> msgContent = new List<byte> { 0x00 };
            masterDev.WriteI2cMsg(slavDevAddr, msgContent);

            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++) 
            {
                clock.Tick();
            }

            Assert.AreEqual(msgContent.Count, slaveDev.lastFrameReceived.data.Count);
            for (int i = 0; i < slaveDev.lastFrameReceived.data.Count; i++)
            {
                Assert.AreEqual(msgContent[i], slaveDev.lastFrameReceived.data[i]);
            }
        }

        [TestMethod]
        public void MasterSends0x53_1ByteMsgToSlaveTest()
        {
            List<byte> msgContent = new List<byte> { 0x53 };
            masterDev.WriteI2cMsg(slavDevAddr, msgContent);

            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            Assert.AreEqual(msgContent.Count, slaveDev.lastFrameReceived.data.Count);
            for (int i = 0; i < slaveDev.lastFrameReceived.data.Count; i++)
            {
                Assert.AreEqual(msgContent[i], slaveDev.lastFrameReceived.data[i]);
            }
        }

        [TestMethod]
        public void MasterSends2MessagesToSlaveTest()
        {
            byte msg1 = 0xFF;
            byte msg2 = 0x00;

            masterDev.WriteI2cMsg(slavDevAddr, new List<byte>() { msg1 });
            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            masterDev.WriteI2cMsg(slavDevAddr, new List<byte>() { msg2 });
            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            Assert.AreEqual(2, slaveDev.receivedFrames.Count);

            for (int i = 0; i < slaveDev.receivedFrames[0].data.Count; i++)
            {
                Assert.AreEqual(msg1, slaveDev.receivedFrames[0].data[i]);
            }

            for (int i = 0; i < slaveDev.receivedFrames[1].data.Count; i++)
            {
                Assert.AreEqual(msg2, slaveDev.receivedFrames[1].data[i]);
            }


        }

        [TestMethod]
        public void MasterSends2ByteMsgToSlaveTest()
        {
            List<byte> msgContent = new List<byte> { 0xFF, 0x00 };
            masterDev.WriteI2cMsg(slavDevAddr, msgContent);

            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++) 
            {
                clock.Tick();
            }

            Assert.AreEqual(msgContent.Count, slaveDev.lastFrameReceived.data.Count);
            for (int i = 0; i < slaveDev.lastFrameReceived.data.Count; i++)
            {
                Assert.AreEqual(msgContent[i], slaveDev.lastFrameReceived.data[i]);
            }
        }

        [TestMethod]
        public void MasterReads1ByteFromSlaveTest()
        {
            byte msgContent = 0x53;
            slaveDev.EnqueueOutputMsg(msgContent);
            masterDev.ReadI2cSlave(slavDevAddr);

            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            Assert.AreEqual(1, masterDev.lastFrameReceived.data.Count);
            for (int i = 0; i < masterDev.lastFrameReceived.data.Count; i++)
            {
                Assert.AreEqual(msgContent, masterDev.lastFrameReceived.data[i]);
            }
        }

        [TestMethod]
        public void MasterReads1ByteFromSlave3TimesTest()
        {
            byte[] msgs = new byte[3] { 0x47, 0xFF, 0x00 };

            foreach (byte item in msgs)
	        {
                slaveDev.EnqueueOutputMsg(item);
	        }
            
            masterDev.ReadI2cSlave(slavDevAddr);
            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            masterDev.ReadI2cSlave(slavDevAddr);
            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            masterDev.ReadI2cSlave(slavDevAddr);
            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            Assert.AreEqual(3, masterDev.receivedFrames.Count);
            for (int i = 0; i < masterDev.receivedFrames.Count; i++)
            {
                Assert.AreEqual(msgs[i], masterDev.receivedFrames[i].data[0]);
            }
        }

        [TestMethod]
        public void OnlyAdreseeReceivesSentMsgTest()
        {
            byte msg = 0x47;

            FakeI2cDevice slaveNo2 = new FakeI2cDevice(bus, clock, 0x15, "slave no 2");

            masterDev.WriteI2cMsg(slavDevAddr, new List<byte> { msg });
            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            Assert.AreEqual(msg, slaveDev.lastFrameReceived.data[0]); //just to check if test works

            Assert.AreEqual(null, slaveNo2.lastFrameReceived);
        }

        [TestMethod]
        public void OnlyMasterReceivesMsgOnMasterReadTest()
        {
            byte msg = 0x47;
            slaveDev.EnqueueOutputMsg(msg);
            FakeI2cDevice slaveNo2 = new FakeI2cDevice(bus, clock, 0x15, "slave no 2");

            masterDev.ReadI2cSlave(slavDevAddr);
            for (int i = 0; i < nearlyInfiniteNoOfTicks; i++)
            {
                clock.Tick();
            }

            Assert.AreEqual(msg, masterDev.lastFrameReceived.data[0]); //just to check if test works
            Assert.AreEqual(null, slaveNo2.lastFrameReceived);
        }

        [TestMethod]
        public void MasterReadsFromOneSlaveAndWRitesTo2ndOneFor5TimesTest()
        {
            byte writeMsg = 0x63;
            byte readMsg = 0x42;

            const byte slave2Addr = 0x15;
            FakeI2cDevice slaveNo2 = new FakeI2cDevice(bus, clock, slave2Addr, "slave no 2");

            for (int i = 0; i < 5; i++)
            {
                slaveDev.EnqueueOutputMsg(readMsg);
                masterDev.ReadI2cSlave(slavDevAddr);
                for (int j = 0; j < nearlyInfiniteNoOfTicks; j++)
                {
                    clock.Tick();
                }
                masterDev.WriteI2cMsg(slave2Addr, new List<byte>() { writeMsg });
                for (int j = 0; j < nearlyInfiniteNoOfTicks; j++)
                {
                    clock.Tick();
                }

                Assert.AreEqual(readMsg, masterDev.lastFrameReceived.data[0]);
                Assert.AreEqual(writeMsg, slaveNo2.lastFrameReceived.data[0]);
            }
        }


    }
}
