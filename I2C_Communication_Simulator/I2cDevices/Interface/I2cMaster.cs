using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public abstract class I2cMaster : I2cDevice
    {
        public void WriteI2cMsg(byte address, List<byte> data)
        {
            bitsToSend = 9 + 9 * data.Count; //8+ack per each byte and address (with R/W)
            AddressAndRWInfo firstFrame = new AddressAndRWInfo(address, ModeRW.Write);
            bool[] firstFrameBitArr = firstFrame.ConvertToBoolArr();

            for (int i = 0; i < 8; i++)
            {
                this.outputQueue.Enqueue(firstFrameBitArr[i]);
            }
            this.outputQueue.Enqueue(true); //wait for ACK

            foreach (byte item in data)
            {
                for (int i = 0; i < 8; i++)
                {
                    this.outputQueue.Enqueue(((int)(item/Math.Pow(2,i)))%2 == 1); //mby slow but working
                }
            }
            this.outputQueue.Enqueue(true); //wait for ACK
        }

        public void ReadI2cSlave(byte address)
        {
            bitsToSend = 18; //address(7) + RW(1) + ACK(1) + MSG(8) + ACK(1) //in this order
            AddressAndRWInfo firstFrame = new AddressAndRWInfo(address, ModeRW.Read);
            bool[] firstFrameBitArr = firstFrame.ConvertToBoolArr();

            for (int i = 0; i < 8; i++)
            {
                this.outputQueue.Enqueue(firstFrameBitArr[i]);
            }
            this.outputQueue.Enqueue(true); //wait for ACK

            //TODO: not sure if that part is needed at all
            for (int i = 0; i < 8; i++)
            {
                this.outputQueue.Enqueue(true); //msg will be received in here
            }
            this.outputQueue.Enqueue(true); //wait for ACK

            this.imMasterReadingSlave = true;
        }

        public I2cMaster(I2cBus _bus, ClockGenerator clock, byte _address, string devName)
            : base(_bus, clock, _address, devName)
        {
            clockGenerator.I2cWriteTick += keepTransmisionOngoingIfThereAreBitsToSend;
        }

        bool transmisionStarted = false;
        int bitsToSend = 0;
        void keepTransmisionOngoingIfThereAreBitsToSend(object sender, EventArgs e)
        {
            if (!transmisionStarted && bitsToSend > 0)
            {   //starting transmision
                bus.GroundSDAForThisCycle();
                transmisionStarted = true;
            }
            else if(transmisionStarted && bitsToSend > 0)
            {   //keeping SCL for transmission
                if (bus.LastSCL == PinState.High)
                {
                    bus.GruondSCLForThisCycle();
                }
                else
                {
                    bitsToSend--;
                }
            }
            else if (transmisionStarted && bitsToSend <= 0)
            {   //ending transmission
                bus.GroundSDAForThisCycle();
                transmisionStarted = false;
            }
            //else //if (transmissionStarted && bitsToSend <= 0) {//do nothing}
        }
    }
}                                                                       
