using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCM2835;
using static BCM2835.BCM2835Managed;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Ports")]
	public class RaspSoftPin : RaspElement
	{
	
		RaspPin internalPin;

		RPiGPIOPin connectorPin;

		[RaspProperty]
		public RPiGPIOPin ConnectorPin
		{
			get { return connectorPin; }
			set 
			{ 
				if (internalPin != null)
				{
#if DEBUG
					Console.WriteLine(connectorPin.ToString() + " destroy old pin");
#endif
					internalPin.Dispose();
				}

				connectorPin = value;
#if DEBUG
				Console.WriteLine(connectorPin.ToString() + " create new pin");
#endif
				internalPin = new RaspPin(connectorPin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT, bcm2835PUDControl.BCM2835_GPIO_PUD_OFF);
			}
		}

		bool pullUpsEnabled = false;
		bool isPullDown = false;

		bool isOutput = false;

		bool outputState = false;
		bool inputState = false;

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		[RaspInput(InputType = IOType.Signal)]
		public void Input(object sender, SignalEventArgs e)
		{

			inputState = e.Signal;
#if DEBUG
			Console.WriteLine(connectorPin.ToString() + " Input received: " + e.Signal.ToString());
#endif
			internalPin.Signal = e.Signal;
		
		}

		[RaspInput(InputType = IOType.Signal)]
		public void EnableOutput(object sender, SignalEventArgs e)
		{

			if (e.Signal != isOutput)
			{ 
				isOutput = e.Signal;

				if (e.Signal)
				{
#if DEBUG
					Console.WriteLine(connectorPin.ToString() + " Switching to output mode");
#endif
                    internalPin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;
					internalPin.EventDetected -= internalPin_EventDetected;
					internalPin.Signal = inputState;
				}
				else
				{
#if DEBUG
					Console.WriteLine(connectorPin.ToString() + " Switching to input mode");
#endif

					internalPin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT;
					internalPin.EventDetected += internalPin_EventDetected;
					var val = internalPin.Signal;

					if (val != outputState)
					{

						outputState = val;

						if (Output != null)
						{
#if DEBUG
							Console.WriteLine(connectorPin.ToString() + " Writting output value");
#endif
							Output(this, new SignalEventArgs { Signal = outputState });

						}
					
					}
				
				}
			}
		
		}

		[RaspInput(InputType = IOType.Signal)]
		void internalPin_EventDetected(object sender, SignalEventArgs e)
		{
			if (e.Signal != outputState)
			{
				outputState = e.Signal;
#if DEBUG
				Console.WriteLine(connectorPin.ToString() + " Event detected: " + outputState);
#endif
				if (Output != null)
					Output(this, e);
			
			}
		}

		[RaspInput(InputType = IOType.Signal)]
		public void EnablePullUps(object sender, SignalEventArgs e)
		{

			if (e.Signal != pullUpsEnabled)
			{
#if DEBUG
				Console.WriteLine(connectorPin.ToString() + " Setting pull ups: " + e.Signal);
#endif
				pullUpsEnabled = e.Signal;
				SetPullUps();
			}
		
		}

		[RaspInput(InputType = IOType.Signal)]
		public void SetPullDown(object sender, SignalEventArgs e)
		{

			if (e.Signal != isPullDown)
			{

#if DEBUG
				Console.WriteLine(connectorPin.ToString() + " Pull ups are pull down: " + e.Signal);
#endif

				isPullDown = e.Signal;

				if (pullUpsEnabled)
					SetPullUps();
			
			}
		
		}

		private void SetPullUps()
		{
			if (!pullUpsEnabled)
				internalPin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF;
			else if (!isPullDown)
				internalPin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_UP;
			else
				internalPin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_DOWN;
		}

	}
}
