using System;
using System.Collections;
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
                address *= 2;
                if (msg[i])
                {
                    address++;
                }
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

        public AddressAndRWInfo(byte _address, ModeRW _mode)
        {
            if (_address > 0x7F)
                throw new ArgumentException("address cannot be bigger than 0x7F (127)", "_address");

            address = _address;
            mode = _mode;
        }

        public bool[] ConvertToBoolArr()
        {
            bool[] boolArr = Converter.ByteToBoolArr((byte)(address << 1));

            if (mode == ModeRW.Write)
            {
                boolArr[7] = true;
            }
            else
            {
                boolArr[7] = false;
            }

            return boolArr;
        }
    }
}
