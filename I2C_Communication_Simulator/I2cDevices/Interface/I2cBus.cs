using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public interface I2cBus
    {
        PinState CurrSDA { get; }
        PinState LastSDA { get; }

        PinState CurrSCL { get; }
        PinState LastSCL { get; }

        ClockGenerator ClockGenerator { get; }

        void GruondSCLForThisCycle();
        void GroundSDAForThisCycle();
    }
}
