using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public interface ClockGenerator
    {
        event EventHandler I2cWriteTick;
        event EventHandler I2cReadTick;
        event EventHandler I2cResetTick;

        void Start();
        void Stop();
    }
}
