using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{
    public class RaspPWM : IDisposable
    {
        bcm2835PWMClockDivider clock;

        public bcm2835PWMClockDivider Clock 
        {

            get { return clock; }
            set 
            {

                clock = value;
                RaspExtern.PWM.bcm2835_pwm_set_clock(value);
            
            }
        }

        bool markSpace;

        public bool MarkSpace
        {
            get { return markSpace; }
            set 
            { 
                markSpace = value;
                RaspExtern.PWM.bcm2835_pwm_set_mode(0, value, enabled);
            }
        }

        bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set 
            { 
                enabled = value;
                RaspExtern.PWM.bcm2835_pwm_set_mode(0, markSpace, value);
            }
        }

        uint range;

        public uint Range
        {
            get { return range; }
            set 
            { 
                range = value;
                RaspExtern.PWM.bcm2835_pwm_set_range(0, value);
            }
        }
        uint data;

        public uint Data
        {
            get { return data; }
            set 
            { 
                data = value;
                RaspExtern.PWM.bcm2835_pwm_set_data(0, value);
            }
        }

        public RaspPWM(bcm2835PWMClockDivider Clock, uint Range, uint Data, bool MarkSpace, bool Enabled)
        {

            this.Clock = Clock;
            this.Range = range;
            this.Data = data;

            markSpace = MarkSpace;
            enabled = Enabled;

            RaspExtern.PWM.bcm2835_pwm_set_mode(0, markSpace, enabled);
        
        }

        public void Dispose()
        {
            RaspExtern.PWM.bcm2835_pwm_set_mode(0, markSpace, false);
        }
    }
}
