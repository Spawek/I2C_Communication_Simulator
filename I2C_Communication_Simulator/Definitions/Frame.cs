using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I2C_Communication_Simulator
{
    public class Frame
    {
        public AddressAndRWInfo addressAndRW;
        public List<byte> data;

        public Frame(AddressAndRWInfo _addressAndRW, List<byte> _data)
        {
            addressAndRW = _addressAndRW;
            data = _data;
        }

        ////TODO: dunno if it used at all (and its not teted!) - so remove it
        //public Frame(AddressAndRWInfo _addressAndRW, List<bool> boolData)
        //{
        //    if (boolData.Count % 8 != 0)
        //        throw new ApplicationException("wrong data size");

        //    addressAndRW = _addressAndRW;

        //    data = new List<byte>();
        //    for (int i = 0; i < boolData.Count;)
        //    {
        //        byte tmp = 0;
        //        bool firstBoolRead = false;
        //        for (; i % 8 != 0 || !firstBoolRead; i++)
        //        {
        //            firstBoolRead = true;
        //            if (boolData[7-i])
        //            {
        //                tmp++;
        //            }

        //            if ((i + 1) % 8 != 0) //multiply x2 on all bytes but last one
        //            {
        //                tmp *= 2;
        //            }
        //        }
        //        data.Add(tmp);

        //    }
        //}

    }
}
