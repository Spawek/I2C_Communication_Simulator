using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public class I2cBusWorkingImplementation : I2cBus
    {
        public PinState CurrSDA { get; private set; }
        public PinState LastSDA { get; private set; }

        public PinState CurrSCL { get; private set; }
        public PinState LastSCL { get; private set; }

        public ClockGenerator ClockGenerator { get; private set; }

        public I2cBusWorkingImplementation(ClockGenerator clock)
        {
            CurrSDA = PinState.High;
            CurrSCL = PinState.High;
            LastSDA = PinState.High;
            LastSCL = PinState.High;

            ClockGenerator = clock;
            clock.I2cResetTick += Reset;
        }

        void Reset(object sender, EventArgs e)
        {
            LastSDA = CurrSDA;
            LastSCL = CurrSCL;

            CurrSDA = PinState.High;
            CurrSCL = PinState.High;
        }

        public void GruondSCLForThisCycle()
        {
            CurrSCL = PinState.Low;
        }

        public void GroundSDAForThisCycle()
        {
            CurrSDA = PinState.Low;
        }
    }
}
