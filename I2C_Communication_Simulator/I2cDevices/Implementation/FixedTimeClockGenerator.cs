using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace I2C_Communication_Simulator
{
    public class FixedTimeClockGenerator : ClockGenerator
    {
        public event EventHandler I2cWriteTick;
        public event EventHandler I2cReadTick;
        public event EventHandler I2cResetTick;

        private Thread tickerTimer;
        private int sleepBetwTicksInMs;
        private bool started;

        public FixedTimeClockGenerator(int _sleepBetwTicksInMs = 10)
        {
            sleepBetwTicksInMs = _sleepBetwTicksInMs;
            tickerTimer = new Thread(new ThreadStart(Ticker));
            tickerTimer.Start();
        }

        private void Ticker()
        {
            while (true)
            {
                if (started)
                {
                    Tick();
                    Thread.Sleep(sleepBetwTicksInMs);
                }
            }
        }

        private void Tick()
        {
            //Console.WriteLine("Tick!");

            //invoke reset
            EventHandler temp = I2cResetTick;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }

            //invoke write
            temp = I2cWriteTick;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }

            //invoke read
            temp = I2cReadTick;
            if (temp != null)
            {
                temp(this, new EventArgs());
            }
        }

        public void Start()
        {
            started = true;
        }

        public void Stop()
        {
            started = false;
        }
    }
}
