using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCM2835;
using static BCM2835.BCM2835Managed;

namespace RaspiSharp
{
    public class RaspLowLevel
    {
        public uint this[uint address]
        {

            get { return BCM2835Managed.bcm2835_peri_read(address); }
            set { BCM2835Managed.bcm2835_peri_write(address, value); }
        }

        public void WriteWithMask(uint Address, uint Value, uint Mask)
        {

            BCM2835Managed.bcm2835_peri_set_bits(Address, Value, Mask);
        
        }
    }
}
