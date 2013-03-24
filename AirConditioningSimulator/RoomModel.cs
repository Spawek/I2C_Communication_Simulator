using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
namespace AirConditioningSimulator
{
    public class RoomModel
    {
        public double InsideTemp { get; set; }
        public double HeaterTemp { get; set; }
        public double OutsideTemp { get; set; }
        public double OutsideTransmissionCoefficient = 0.02;
        public double HeaterTransmissionCoefficient = 0.004;

        private Timer timer = new Timer();
        private const int TEMP_ACUALIZING_INTERVAL_IN_MS = 200;

        public RoomModel()
        {
            InsideTemp = 20;
            HeaterTemp = 20;
            OutsideTemp = 20;

            timer.Interval = TEMP_ACUALIZING_INTERVAL_IN_MS;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            double OutsideTempChange = (OutsideTemp - InsideTemp) * OutsideTransmissionCoefficient * 1000 / TEMP_ACUALIZING_INTERVAL_IN_MS;
            double HeaterTempChange = (HeaterTemp - InsideTemp) * HeaterTransmissionCoefficient * 1000 / TEMP_ACUALIZING_INTERVAL_IN_MS;

            InsideTemp += OutsideTempChange + HeaterTempChange;
        }

    }
}
