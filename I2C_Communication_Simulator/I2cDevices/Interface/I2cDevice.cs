using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public abstract class I2cDevice
    {
        protected event EventHandler<Frame> FrameReceived;

        protected I2cBus bus { get; private set; }
        protected ClockGenerator clockGenerator { get; private set; }

        public byte address { get; private set; }

        protected Queue<byte> outputMsgQueue = new Queue<byte>();
        protected Queue<bool> outputQueue = new Queue<bool>();
        private List<bool> inputBuffer = new List<bool>();
        protected Dictionary<int, byte> memory = new Dictionary<int, byte>();

        public I2cDevice(I2cBus _bus, ClockGenerator clock, byte _address)
        {
            bus = _bus;
            clockGenerator = clock;
            address = _address;

            clock.I2cReadTick += clock_I2cReadTick;
            clock.I2cWriteTick += clock_I2cWriteTick;
        }

        void clock_I2cWriteTick(object sender, EventArgs e)
        {
            if (outputQueue.Count > 0)
            {
                if (bus.LastSCL == PinState.Low && bus.LastSCL == PinState.High)
                {
                    if (!outputQueue.Dequeue())
                    {
                        bus.GruondSCLForThisCycle();
                    }
                }
            }
        }

        private bool frameOngoing = false;
        private bool addressWithRWOngoing = false;
        private AddressAndRWInfo addressWithRWReceived = null;
        private bool imAddresee = false;
        private ModeRW mode = ModeRW.Read;
        void clock_I2cReadTick(object sender, EventArgs e)
        {
            if (StartSqOngoing())
            {
                frameOngoing = true;
                addressWithRWOngoing = true;
            }
            else if (EndSqOngoing())
            {
                if (frameOngoing)
                {
                    EventHandler<Frame> temp = FrameReceived;
                    if (temp != null)
                    {
                        temp(this, new Frame(addressWithRWReceived, inputBuffer));
                        inputBuffer.Clear();
                    }
                }
                frameOngoing = false;
            }

            if (frameOngoing)
            {
                ReadBit();
            }


            if (addressWithRWOngoing && inputBuffer.Count == 8)
            {
                addressWithRWOngoing = false;
                addressWithRWReceived = new AddressAndRWInfo(inputBuffer.ToArray());
                mode = addressWithRWReceived.mode;
                inputBuffer.Clear();

                if (addressWithRWReceived.address == this.address)
                {
                    imAddresee = true;
                    SendAck();

                    if (mode == ModeRW.Write)
                    {
                        BitArray arr = new BitArray(outputMsgQueue.Dequeue());
                        if(arr.Count == 0)
                        {
                            for (int i = 0; i < 8; i++)
			                {
			                    outputQueue.Enqueue(true); //so 11111111 is msg in case of err
			                }
                        }
                        else
                        {
                            foreach(bool bit in arr)
                            {
                                outputQueue.Enqueue(bit);
                            }
                        }
                    }
                }
                else
                {
                    frameOngoing = false; //frame is ongoing but not to this object
                }
            }

            if (imAddresee) //receiving data
            {
                if (inputBuffer.Count % 8 == 0)
                {
                    SendAck();
                }
            }
        }

        private void SendAck()
        {
            outputQueue.Enqueue(false);
        }

        private void ReadBit()
        {
            if (bus.LastSCL == PinState.Low &&
                bus.CurrSCL == PinState.High)
            {
                if (bus.LastSDA == PinState.Low)
                {
                    inputBuffer.Add(false);
                }
                else
                {
                    inputBuffer.Add(true);
                }
            }
        }

        private bool StartSqOngoing()
        {
            return bus.LastSDA == PinState.High &&
                bus.CurrSDA == PinState.Low &&
                bus.LastSCL == PinState.High &&
                bus.CurrSCL == PinState.High;
        }

        private bool EndSqOngoing()
        {
            return frameOngoing &&
                bus.LastSDA == PinState.Low && 
                bus.CurrSDA == PinState.High &&
                bus.LastSCL == PinState.High &&
                bus.CurrSCL == PinState.High;
        }

    }
}
