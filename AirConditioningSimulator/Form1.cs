using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using I2C_Communication_Simulator;
using System.Timers;

namespace AirConditioningSimulator
{
    public partial class Form1 : Form
    {
        private I2cBus bus;
        private ClockGenerator clock;
        private RoomModel model;
        private I2cAirConditioningController controller;
        private I2cHeater heater;
        private I2cThermometer thermometer;

        private int I2C_CLOCK_INTERVAL_IN_MS = 10;
        private byte HEATER_I2C_ADDRESS = 0x53;
        private byte THERMOMETER_I2C_ADDRESS = 0x32;
        private byte CONTROLLER_I2C_ADDRESS = 0x01;

        private System.Timers.Timer timer = new System.Timers.Timer();
        private int FORM_UPDATE_INTERVAL_IN_MS = 50;

        public Form1()
        {
            clock = new FixedTimeClockGenerator(I2C_CLOCK_INTERVAL_IN_MS);
            bus = new I2cBusWorkingImplementation(clock);
            model = new RoomModel();
            heater = new I2cHeater(bus, clock, HEATER_I2C_ADDRESS, model);
            thermometer = new I2cThermometer(bus, clock, THERMOMETER_I2C_ADDRESS, model);
            controller = new I2cAirConditioningController(bus, clock, CONTROLLER_I2C_ADDRESS, model, HEATER_I2C_ADDRESS, THERMOMETER_I2C_ADDRESS);

            timer.Interval = FORM_UPDATE_INTERVAL_IN_MS;
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            clock.Start();

            InitializeComponent();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateForm();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            controller.WantedTemp = trackBar1.Value + 20;
            label_TargetTemp.Text = controller.WantedTemp.ToString();
        }

        delegate void UpdateFormDelegate();
        private void UpdateForm()
        {
            try
            {
                if (label1.InvokeRequired)
                { //workers thread
                    this.Invoke(new UpdateFormDelegate(UpdateForm));
                }
                else
                { //window thread
                    label_InsideTemp.Text = model.InsideTemp.ToString();
                    label_OutsideTemp.Text = model.OutsideTemp.ToString();
                    label_HeaterTemp.Text = model.HeaterTemp.ToString();
                }
            }
            catch(Exception)
            {
                //just dont care about it
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            model.OutsideTemp = trackBar2.Value;
            label_OutsideTemp.Text = trackBar2.Value.ToString();
        }

    }
}
