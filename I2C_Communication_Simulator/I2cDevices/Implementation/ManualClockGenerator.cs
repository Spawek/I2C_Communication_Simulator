using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I2C_Communication_Simulator
{
    public class ManualClockGenerator : ClockGenerator
    {
        public event EventHandler I2cWriteTick;
        public event EventHandler I2cReadTick;
        public event EventHandler I2cResetTick;

        public ManualClockGenerator()
        {

        }

        public void Tick()
        {
            Console.WriteLine("Tick!");

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
            throw new ApplicationException("DO NOT USE THAT! - its manual clock generator");
        }

        public void Stop()
        {
            throw new ApplicationException("DO NOT USE THAT! - its manual clock generator");
        }

    }
}
