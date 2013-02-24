using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I2C_Communication_Simulator
{
    public enum ModeRW
    {
        Read,
        Write
    }

    public class AddressAndRWInfo
    {
        public byte address;
        public ModeRW mode;

        /// <summary>
        /// first 7 bits are address
        /// last bit:
        ///     0 -> read
        ///     1 -> write
        /// </summary>
        /// <param name="msg"></param>
        public AddressAndRWInfo(bool[] msg)
        {
            if (msg.Length != 8)
                throw new ApplicationException("wrong msg length!");

            address = 0;
            for (int i = 0; i <= 6; i++)
            {
                if (msg[i])
                {
                    address++;
                }
                address *= 2;
            }

            if (msg[7])
            {
                mode = ModeRW.Write;
            }
            else
            {
                mode = ModeRW.Read;
            }
        }
    }
}
