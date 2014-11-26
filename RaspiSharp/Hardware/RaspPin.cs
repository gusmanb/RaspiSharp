using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.IO;
using RaspiSharp.Software;

namespace RaspiSharp
{
    public class RaspPin : IDisposable
    {
        internal RPiGPIOPin currentPin;

		//public RaspPinEvents Events;

		DetectEdge edge = DetectEdge.Off;
		public DetectEdge Edge
		{

			get { return edge; }
			set
			{

				edge = value;
				RaspExtern.GPIO.setDetectEdge(currentPin, edge);

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

		Thread evtThread;
		FileStream fevt;

		private void enableEvents()
		{
			//var data = RaspExtern.GPIO.setPinEventDetector(currentPin, eventCallback);
			evtThread = new Thread(() =>
			{

				if (RaspExtern.GPIO.exportPin(currentPin) != 0)
					throw new InvalidOperationException("Cannot export pin");

				if (RaspExtern.GPIO.prepareManualEventDetector(currentPin) != 0)
					throw new InvalidOperationException("Cannot enable event detection");

				byte value;

				while (true)
				{
					value = RaspExtern.GPIO.manualEventDetect(currentPin);

					if (value == 0xFF)
						throw new InvalidOperationException("Error reading events");

					if (eventDetected != null)
						eventDetected(this, new SignalEventArgs { Signal = value == 0 ? false : true });

				}

			});
			evtThread.Start();
		}

		private void disableEvents()
		{

			RaspExtern.GPIO.clearPinEventDetector(currentPin);
			//fevt.Close();
			evtThread.Abort();
		}

		private void eventCallback(byte Value)
		{
			try
			{
				if (eventDetected != null)
					eventDetected(this, new SignalEventArgs { Signal = Value == 0 ? false : true });
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

            get { return RaspExtern.GPIO.bcm2835_gpio_lev(currentPin) > 0; }
            set
            {
                if (value)
                    RaspExtern.GPIO.bcm2835_gpio_set(currentPin);
                else
                    RaspExtern.GPIO.bcm2835_gpio_clr(currentPin);
            }

        }

        GPIOFunctionSelect currentFunction = GPIOFunctionSelect.Function_INPT;

        public GPIOFunctionSelect Function
        {

            get { return currentFunction; }
            set 
            { 
                currentFunction = value;
                RaspExtern.GPIO.bcm2835_gpio_fsel(currentPin, currentFunction);
            }
        }

        PullUpDownControl currentPullUpDown = PullUpDownControl.Pull_DOWN;

        public PullUpDownControl PullUpDown
        {

            get { return currentPullUpDown; }
            set 
            {

                currentPullUpDown = value;
                RaspExtern.GPIO.bcm2835_gpio_set_pud(currentPin, currentPullUpDown);
            
            }
        
        }

        public RaspPin(RPiGPIOPin PhysicalPin, GPIOFunctionSelect InitialFunction = GPIOFunctionSelect.Function_INPT, PullUpDownControl InitialPullUpDown = PullUpDownControl.Pull_OFF)
        {

            currentPin = PhysicalPin;
            Function = InitialFunction;
            PullUpDown = InitialPullUpDown;

        }

    }

}
