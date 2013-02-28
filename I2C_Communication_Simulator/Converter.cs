using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I2C_Communication_Simulator
{
    public class Converter
    {
        public static bool[] ByteToBoolArr(byte data)
        {
            bool[] output = new bool[8];

            for (int i = 7; i >= 0; i--)
            {
                output[i] = (data % 2 == 1) ? true : false;
                data /= 2;
            }

            return output;
        }
    }
}
