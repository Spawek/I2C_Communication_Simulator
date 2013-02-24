using System;
using I2C_Communication_Simulator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace I2C_Comminicator_Unit_Tests
{
    [TestClass]
    public class I2cBusWorkingImplementationTests
    {
        static MockRepository mocks;
        static ManualClockGenerator manualClock;
        static I2cBusWorkingImplementation bus;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            mocks = new MockRepository();
            manualClock = new ManualClockGenerator();
            bus = new I2cBusWorkingImplementation(manualClock);
        }

        [TestMethod]
        public void I2cBusWorkingImplementation_initialPinStatesAreHigh()
        {
            Assert.AreEqual(PinState.High, bus.CurrSCL);
            Assert.AreEqual(PinState.High, bus.CurrSDA);
            Assert.AreEqual(PinState.High, bus.LastSCL);
            Assert.AreEqual(PinState.High, bus.LastSDA);
        }

        [TestMethod]
        public void I2cBusWorkingImplementation_statesChangesAreCorrect()
        {
            bus.GroundSDAForThisCycle();
            bus.GruondSCLForThisCycle();

            Assert.AreEqual(PinState.Low, bus.CurrSCL);
            Assert.AreEqual(PinState.Low, bus.CurrSDA);
        }

        [TestMethod]
        public void I2cBusWorkingImplementation_statesBackToHighOnResetAndOldStatesAreSaved()
        {
            bus.GroundSDAForThisCycle();
            bus.GruondSCLForThisCycle();

            manualClock.Tick();

            Assert.AreEqual(PinState.High, bus.CurrSCL);
            Assert.AreEqual(PinState.High, bus.CurrSDA);
            Assert.AreEqual(PinState.Low, bus.LastSCL);
            Assert.AreEqual(PinState.Low, bus.LastSDA);
        }
    }
}
