using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public interface I2cBus
    {
        PinState SDA { get; }
        PinState SCL { get; }
        ClockGenerator ClockGenerator { get; }

        void GruondSCLForThisCycle();
        void GroundSDAForThisCycle();
    }
}
