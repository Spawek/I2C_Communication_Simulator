using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public interface I2cDevice
    {
        I2cBus I2cBus { get; }
        Dictionary<int, byte> Memory { get; }
        byte Address { get; }
        ClockGenerator ClockGenerator { get; }
    }
}
