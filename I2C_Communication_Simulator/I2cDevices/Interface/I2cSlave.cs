using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public abstract class I2cSlave : I2cDevice
    {
        public event EventHandler I2cFrameAck;

        public void AddMsgForI2cMaster()
        {
            throw new System.NotImplementedException();
        }

        public I2cSlave(I2cBus _bus, ClockGenerator clock, byte _address, string devName)
            : base(_bus, clock, _address, devName)
        {

        }

    }
}
