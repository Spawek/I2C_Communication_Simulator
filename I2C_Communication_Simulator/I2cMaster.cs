﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public abstract class I2cMaster : I2cDevice
    {
        public void WriteI2cMsg()
        {
            throw new System.NotImplementedException();
        }

        public void ReadI2cSlave()
        {
            throw new System.NotImplementedException();
        }

        public I2cBus I2cBus
        {
            get { throw new NotImplementedException(); }
        }

        public Dictionary<int, byte> Memory
        {
            get { throw new NotImplementedException(); }
        }

        public byte Address
        {
            get { throw new NotImplementedException(); }
        }

        public ClockGenerator ClockGenerator
        {
            get { throw new NotImplementedException(); }
        }
    }
}