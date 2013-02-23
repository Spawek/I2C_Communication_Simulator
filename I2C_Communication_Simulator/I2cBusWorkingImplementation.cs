using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public class I2cBusWorkingImplementation : I2cBus
    {
        public PinState SDA
        {
            get { throw new NotImplementedException(); }
        }

        public PinState SCL
        {
            get { throw new NotImplementedException(); }
        }

        public ClockGenerator ClockGenerator
        {
            get { throw new NotImplementedException(); }
        }

        public void GruondSCLForThisCycle()
        {
            throw new NotImplementedException();
        }

        public void GroundSDAForThisCycle()
        {
            throw new NotImplementedException();
        }
    }
}
