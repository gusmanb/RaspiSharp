using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCM2835;

namespace RaspiSharp
{
    public static class RaspDelay
    {
        public static void uSDelay(long uSecs)
        {
            BCM2835Managed.bcm2835_delayMicroseconds((long)uSecs);            
        }
    }
}
