using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using I2C_Communication_Simulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace I2C_Comminicator_Unit_Tests
{
    [TestClass]
    public class AddressAndRWInfoTests
    {
        [TestMethod]
        public void AddressAndRWInfoTest_convertFromBitMsgAndConvertBackToBitMsg()
        {
            bool[] testMsg = new bool[]{true, false, false, true, false, true, true, true};
            AddressAndRWInfo addr = new AddressAndRWInfo(testMsg);
            bool[] recreatedMsg = addr.ConvertToBoolArr();

            for (int i = 0; i < 8; i++)
			{
                Assert.AreEqual(testMsg[i], recreatedMsg[i]);
			}
        }

        [TestMethod]
        public void AddressAndRWInfoTest_createFromBitMsg()
        {
            bool[] testMsg = new bool[] { true, false, false, true, false, true, true, true };
            AddressAndRWInfo addr = new AddressAndRWInfo(testMsg);

            Assert.AreEqual(0x4B, addr.address);
            Assert.AreEqual(ModeRW.Write, addr.mode);
        }
    }
}
