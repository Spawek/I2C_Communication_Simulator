using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using I2C_Communication_Simulator;
using System.Timers;

namespace AirConditioningSimulator
{
    public class I2cThermometer : I2cDevice
    {
        private RoomModel model;
        private Timer timer = new Timer();
        private const int TEMP_ACTUALIZATION_TIME_IN_MS = 100;

        public I2cThermometer(I2cBus _bus, ClockGenerator clock, byte _address, RoomModel _model)
            : base(_bus, clock, _address, "I2C Thermometer")
        {
            model = _model;
            timer.Interval = TEMP_ACTUALIZATION_TIME_IN_MS;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.outputMsgQueue.Clear(); //theoreticly it can crash if different thread uses it here, but its ultra-low chance crash
            this.outputMsgQueue.Enqueue(Convert.ToByte(model.InsideTemp));
        }
    }

    public class I2cHeater : I2cDevice
    {
        private RoomModel model;
        private int __HEATER_TEMP__;
        private int HeaterTemp
        {
            get { return __HEATER_TEMP__; }
            set
            {
                __HEATER_TEMP__ = value;
                model.HeaterTemp = value;
            }
        }

        public I2cHeater(I2cBus _bus, ClockGenerator clock, byte _address, RoomModel _model)
            : base(_bus, clock, _address, "I2C Heater")
        {
            model = _model;
            this.FrameReceived += I2cHeater_FrameReceived;
        }

        void I2cHeater_FrameReceived(object sender, Frame e)
        {
            HeaterTemp = Convert.ToInt32(e.data);
        }
    }

    public class I2cAirConditioningController : I2cMaster
    {
        public int RoomTemp = 0;
        public int WantedTemp = 0;
        public int HeaterTemp = 0;

        private const int TEMP_ACTUALIZING_INTERVAL_IN_MS = 1000;
        private byte theromotherI2cAddress;
        private byte heaterI2cAddress;

        private System.Timers.Timer timer = new System.Timers.Timer();
        public I2cAirConditioningController(I2cBus _bus, ClockGenerator clock, byte _address, RoomModel _model, byte _heaterAddress, byte _theromometerAddress)
            : base(_bus, clock, _address, "I2C Air Conditioning Controller")
        {
            theromotherI2cAddress = _theromometerAddress;
            heaterI2cAddress = _heaterAddress;

            timer.Interval = TEMP_ACTUALIZING_INTERVAL_IN_MS;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            this.FrameReceived += I2cAirConditioningController_FrameReceived;
        }

        void I2cAirConditioningController_FrameReceived(object sender, Frame e)
        {
            RoomTemp = Convert.ToInt32(e.data);
            if (RoomTemp < WantedTemp) HeaterTemp++;
            else HeaterTemp--;
        }

        bool isItReadCycle = true;
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (outputQueue.Count == 0)
            {
                if (isItReadCycle)
                {
                    this.ReadI2cSlave(theromotherI2cAddress);
                    isItReadCycle = false;
                }
                else
                {
                    this.WriteI2cMsg(heaterI2cAddress, new List<byte>() { Convert.ToByte(HeaterTemp) });
                    isItReadCycle = true;
                }
            }
        }

    }
}
