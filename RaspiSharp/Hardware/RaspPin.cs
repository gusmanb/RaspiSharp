using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.IO;
using RaspiSharp.Software;
using BCM2835;
using static BCM2835.BCM2835Managed;

namespace RaspiSharp
{
    public class RaspPin : IDisposable
    {
        internal RPiGPIOPin currentPin;

		//public RaspPinEvents Events;

		RPiDetectorEdge edge = RPiDetectorEdge.Rising;
		public RPiDetectorEdge Edge
		{

			get { return edge; }
			set
			{

				edge = value;
                BCM2835Managed.GPIOExtras.set_detect_edge(currentPin, edge);

			}
		}

		private EventHandler<SignalEventArgs> eventDetected;

		public event EventHandler<SignalEventArgs> EventDetected
		{

			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				if (eventDetected == null)
					enableEvents();

				eventDetected = (EventHandler<SignalEventArgs>)Delegate.Combine(eventDetected, value);
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				eventDetected = (EventHandler<SignalEventArgs>)Delegate.Remove(eventDetected, value);

				if (eventDetected == null)
					disableEvents();
			}

		}
        
		private void enableEvents()
		{

            BCM2835Managed.GPIOExtras.set_event_detector(currentPin, edge, eventCallback);

		}

		private void disableEvents()
		{

            BCM2835Managed.GPIOExtras.remove_event_detector(currentPin);
		}

		private void eventCallback(RPiGPIOPin pin, short value)
		{
			try
			{
				if (eventDetected != null)
					eventDetected(this, new SignalEventArgs { Signal = value == 0 ? false : true });
			}
			catch (Exception e)
			{

				Console.WriteLine(e.Message);

			}
		}
		
		public void Dispose()
		{
			disableEvents();
		}

        public bool Signal
        {

            get { return BCM2835Managed.bcm2835_gpio_lev(currentPin); }
            set
            {
                if (value)
                    BCM2835Managed.bcm2835_gpio_set(currentPin);
                else
                    BCM2835Managed.bcm2835_gpio_clr(currentPin);
            }

        }

        bcm2835FunctionSelect currentFunction = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT;

        public bcm2835FunctionSelect Function
        {

            get { return currentFunction; }
            set 
            { 
                currentFunction = value;
                BCM2835Managed.bcm2835_gpio_fsel(currentPin, currentFunction);
            }
        }

        bcm2835PUDControl currentPullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF;

        public bcm2835PUDControl PullUpDown
        {

            get { return currentPullUpDown; }
            set 
            {

                currentPullUpDown = value;
                BCM2835Managed.bcm2835_gpio_set_pud(currentPin, currentPullUpDown);
            
            }
        
        }

        public RaspPin(RPiGPIOPin PhysicalPin, bcm2835FunctionSelect InitialFunction = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT, bcm2835PUDControl InitialPullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF)
        {

            currentPin = PhysicalPin;
            Function = InitialFunction;
            PullUpDown = InitialPullUpDown;

        }

    }

}
