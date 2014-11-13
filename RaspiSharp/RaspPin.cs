using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Runtime.CompilerServices;

namespace RaspiSharp
{
    public class RaspPin
    {
        internal RPiGPIOPin currentPin;

        public RaspPinEvents Events;

        public bool Status
        {

            get { return RaspExtern.GPIO.bcm2835_gpio_lev(currentPin) > 0; }
            set
            {
                if (value)
                    RaspExtern.GPIO.bcm2835_gpio_set(currentPin);
                else
                    RaspExtern.GPIO.bcm2835_gpio_clr(currentPin);
            }

        }

        bcm2835FunctionSelect currentFunction = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT;

        public bcm2835FunctionSelect Function
        {

            get { return currentFunction; }
            set 
            { 
                currentFunction = value;
                RaspExtern.GPIO.bcm2835_gpio_fsel(currentPin, currentFunction);
            }
        }

        bcm2835PUDControl currentPullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_DOWN;

        public bcm2835PUDControl PullUpDown
        {

            get { return currentPullUpDown; }
            set 
            {

                currentPullUpDown = value;
                RaspExtern.GPIO.bcm2835_gpio_set_pud(currentPin, currentPullUpDown);
            
            }
        
        }

        public RaspPin(RPiGPIOPin PhysicalPin, bcm2835FunctionSelect InitialFunction = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT, bcm2835PUDControl InitialPullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF)
        {

            currentPin = PhysicalPin;
            Events = new RaspPinEvents(currentPin);
            Function = InitialFunction;
            PullUpDown = InitialPullUpDown;

        }

        public class RaspPinEvents : IDisposable
        {

            RPiGPIOPin currentPin;

            internal RaspPinEvents(RPiGPIOPin PhysicalPin)
            {

                currentPin = PhysicalPin;

            }

            bool afe = false;
            public bool DetectAsynchronousFallingEdge 
            { 
                get { return afe; } 
                set 
                {

                    afe = value;

                    if (afe)
                        RaspExtern.GPIO.bcm2835_gpio_afen(currentPin);
                    else
                        RaspExtern.GPIO.bcm2835_gpio_clr_afen(currentPin);
                
                } 
            }

            bool are = false;
            public bool DetectAsynchronousRisingEdge
            {
                get { return are; }
                set
                {

                    are = value;

                    if (are)
                        RaspExtern.GPIO.bcm2835_gpio_aren(currentPin);
                    else
                        RaspExtern.GPIO.bcm2835_gpio_clr_aren(currentPin);

                }
            }

            bool fe = false;
            public bool DetectFallingEdge
            {
                get { return fe; }
                set
                {

                    fe = value;

                    if (fe)
                        RaspExtern.GPIO.bcm2835_gpio_fen(currentPin);
                    else
                        RaspExtern.GPIO.bcm2835_gpio_clr_fen(currentPin);

                }
            }

            bool re = false;
            public bool DetectRisingEdge
            {
                get { return re; }
                set
                {

                    re = value;

                    if (re)
                        RaspExtern.GPIO.bcm2835_gpio_ren(currentPin);
                    else
                        RaspExtern.GPIO.bcm2835_gpio_clr_ren(currentPin);

                }
            }

            bool low = false;
            public bool DetectLow
            {
                get { return low; }
                set
                {

                    low = value;

                    if (low)
                        RaspExtern.GPIO.bcm2835_gpio_len(currentPin);
                    else
                        RaspExtern.GPIO.bcm2835_gpio_clr_len(currentPin);

                }
            }

            bool high = false;
            public bool DetectHigh
            {
                get { return high; }
                set
                {

                    high = value;

                    if (high)
                        RaspExtern.GPIO.bcm2835_gpio_hen(currentPin);
                    else
                        RaspExtern.GPIO.bcm2835_gpio_clr_hen(currentPin);

                }
            }

            public bool EventDetected
            {
                get { return RaspExtern.GPIO.bcm2835_gpio_eds(currentPin) == 1; }
                set
                {
                    if (!value)
                        RaspExtern.GPIO.bcm2835_gpio_set_eds(currentPin);
                }
            }

            int pollInterval = 50;

            public int DetectionPollInterval 
            { 
                get { return pollInterval; } 
                set 
                { 
                    pollInterval = value; 
                    if (pollTimer != null)
                        pollTimer.Interval = pollInterval; 
                } 
            }

            private EventHandler eventDetect;

            public event EventHandler EventDetect 
            {

                [MethodImpl(MethodImplOptions.Synchronized)]
                add 
                { 
                    if(eventDetect == null)
                        beginEventPoll();

                    eventDetect = (EventHandler)Delegate.Combine(eventDetect, value);
                }

                [MethodImpl(MethodImplOptions.Synchronized)]
                remove 
                { 
                    eventDetect = (EventHandler)Delegate.Remove(eventDetect, value);

                    if (eventDetect == null)
                        endEventPoll();
                }
            
            }

            private Timer pollTimer;

            private void endEventPoll()
            {
                if (pollTimer == null)
                    return;

                pollTimer.Stop();
                pollTimer.Dispose();
                pollTimer = null;
            }

            private void beginEventPoll()
            {
                if (pollTimer != null)
                {
                    pollTimer.Interval = pollInterval;
                    pollTimer.Start();
                    return;
                
                }

                pollTimer = new Timer(pollInterval);
                pollTimer.Elapsed += new ElapsedEventHandler(pollTimer_Elapsed);
                pollTimer.Start();
            }

            void pollTimer_Elapsed(object sender, ElapsedEventArgs e)
            {
                if (EventDetected && eventDetect != null)
                {
                    EventDetected = false;
                    eventDetect(this, e);
                    
                }
            }

            public void Dispose()
            {
                endEventPoll();
            }
        }
    }
}
