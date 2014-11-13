using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{
    public static class RaspDelay
    {
        public static void uSDelay(ulong uSecs)
        {

            ulong start = RaspExtern.Timers.bcm2835_st_read();
            RaspExtern.Timers.bcm2835_st_delay(start, uSecs);

        }
    }
}
