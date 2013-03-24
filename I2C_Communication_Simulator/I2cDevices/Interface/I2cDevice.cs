using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace I2C_Communication_Simulator
{
    public abstract class I2cDevice
    {
        public event EventHandler<Frame> FrameReceived;

        protected string deviceName;

        protected I2cBus bus { get; private set; }
        protected ClockGenerator clockGenerator { get; private set; }

        public byte address { get; private set; }

        protected Queue<byte> outputMsgQueue = new Queue<byte>();
        protected Queue<bool> outputQueue = new Queue<bool>();
        private List<bool> inputBuffer = new List<bool>();
        private List<byte> inputBytesBuffer = new List<byte>();
        protected Dictionary<int, byte> memory = new Dictionary<int, byte>();

        public I2cDevice(I2cBus _bus, ClockGenerator clock, byte _address, string devName)
        {
            bus = _bus;
            clockGenerator = clock;
            address = _address;
            deviceName = devName;

            clock.I2cReadTick += clock_I2cReadTick;
            clock.I2cWriteTick += clock_I2cWriteTick;
        }

        void clock_I2cWriteTick(object sender, EventArgs e)
        {
            if (outputQueue.Count > 0)
            {
                if (bus.LastSCL == PinState.Low && bus.CurrSCL == PinState.High)
                {
                    if (!outputQueue.Dequeue())
                    {
                        bus.GroundSDAForThisCycle();
                    }
                }
            }
        }

        private bool imAddresseAndDataToMeOngoing = false;
        private bool addressWithRWOngoing = false;
        private AddressAndRWInfo addressWithRWReceived = null;
        private bool imSlaveWriteAdresee = false;
        private ModeRW mode = ModeRW.Read;
        private bool ackBitOngoing = false;
        protected bool imMasterReadingSlave = false;
        private bool frameOngoing = false;
        void clock_I2cReadTick(object sender, EventArgs e)
        {
            if (StartSqOngoing() && !frameOngoing)
            {
                imAddresseAndDataToMeOngoing = true;
                addressWithRWOngoing = true;
                frameOngoing = true;
                inputBuffer.Clear();
            }
            else if (EndSqOngoing() && frameOngoing)
            {
                if (imAddresseAndDataToMeOngoing || imMasterReadingSlave)
                {
                    EventHandler<Frame> temp = FrameReceived;
                    if (temp != null)
                    {
                        temp(this, new Frame(addressWithRWReceived, new List<byte>(inputBytesBuffer)));
                        inputBytesBuffer.Clear();
                    }
                }
                imAddresseAndDataToMeOngoing = false;
                imSlaveWriteAdresee = false;
                imMasterReadingSlave = false;
                frameOngoing = false;
                ResetInternalData(); //its a ltl workarround but it helps
            }

            if (addressWithRWOngoing || imSlaveWriteAdresee || imMasterReadingSlave)
            {
                if (bus.LastSCL == PinState.Low && //only write cycles are important
                    bus.CurrSCL == PinState.High)
                {
                    if (!ackBitOngoing)
                    {
                        ReadBit();
                    }
                    else
                    {
                        ackBitOngoing = false;
                    }
                }
            }

            if (addressWithRWOngoing && inputBuffer.Count == 8)
            {
                addressWithRWOngoing = false;
                ackBitOngoing = true;
                addressWithRWReceived = new AddressAndRWInfo(inputBuffer.ToArray());
                mode = addressWithRWReceived.mode;
                inputBuffer.Clear();

                if (addressWithRWReceived.address == this.address)
                {
                    SendAck();

                    if (mode == ModeRW.Read) //so this obj will be writing (its master mode, this obj is slave (it is adresee))
                    {
                        EnqueueBitsFromOuputMsgQueue();
                    }
                    else
                    {
                        imSlaveWriteAdresee = true;
                    }
                }
                else
                {
                    imAddresseAndDataToMeOngoing = false; //frame is ongoing but not to this object
                }
            }

            if (imSlaveWriteAdresee || imMasterReadingSlave) //receiving data
            {
                if (inputBuffer.Count == 8 && !ackBitOngoing)
                {
                    GetByteOutOfInputBuffer();
                    SendAck();

                    //if (imMasterReadingSlave) //its a but fix - TODO: try doing it in some better way
                    //{
                    //    ackBitOngoing = true;
                    //}
                }
            }

            if (inputBuffer.Count > 0 && inputBuffer.Count % 8 == 0)
            {
                ackBitOngoing = true;
            }

        }

        private void ResetInternalData()
        {
            imAddresseAndDataToMeOngoing = false;
            addressWithRWOngoing = false;
            addressWithRWReceived = null;
            imSlaveWriteAdresee = false;
            mode = ModeRW.Read;
            ackBitOngoing = false;
            imMasterReadingSlave = false;
            inputBuffer.Clear();
            outputQueue.Clear();
        }

        private void EnqueueBitsFromOuputMsgQueue()
        {
            if (outputMsgQueue.Count == 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    outputQueue.Enqueue(true); //so 11111111 is msg in case of err
                }
            }
            else
            {
                byte msg = outputMsgQueue.Dequeue();
                for (int i = 0; i < 8; i++)
                {
                    this.outputQueue.Enqueue(((int)(msg / Math.Pow(2, i))) % 2 == 1); //mby slow but working
                }
            }
        }

        private void GetByteOutOfInputBuffer()
        {
            byte tmp = 0;
            for (int i = 0; i < 8; i++)
            {
                if (inputBuffer[7-i])
                {
                    tmp++;
                }

                if (i != 7) //multiply x2 on all bytes but last one
                {
                    tmp *= 2;
                }
            }
            inputBytesBuffer.Add(tmp);
            inputBuffer.Clear();
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
                if (bus.CurrSDA == PinState.Low)
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
            return bus.LastSDA == PinState.Low && 
                bus.CurrSDA == PinState.High &&
                bus.LastSCL == PinState.High &&
                bus.CurrSCL == PinState.High;
        }

    }
}
