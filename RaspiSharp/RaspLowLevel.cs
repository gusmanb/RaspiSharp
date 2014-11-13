using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{
    public class RaspLowLevel
    {
        public uint this[uint address]
        {

            get { return RaspExtern.LowLevel.bcm2835_peri_read(address); }
            set { RaspExtern.LowLevel.bcm2835_peri_write(address, value); }
        }

        public void WriteWithMask(uint Address, uint Value, uint Mask)
        {

            RaspExtern.LowLevel.bcm2835_peri_set_bits(Address, Value, Mask);
        
        }
    }
}
