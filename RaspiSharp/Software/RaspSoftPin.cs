using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
				connectorPin = value;
				if (internalPin != null)
					internalPin.Dispose();

				internalPin = new RaspPin(connectorPin, GPIOFunctionSelect.Function_INPT, PullUpDownControl.Pull_OFF);
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

					internalPin.EventDetected -= internalPin_EventDetected;
					internalPin.Signal = inputState;
				}
				else
				{

					internalPin.EventDetected += internalPin_EventDetected;
					var val = internalPin.Signal;

					if (val != outputState)
					{

						outputState = val;

						if (Output != null)
							Output(this, new SignalEventArgs { Signal = outputState });
					
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

				if (Output != null)
					Output(this, e);
			
			}
		}

		[RaspInput(InputType = IOType.Signal)]
		public void EnablePullUps(object sender, SignalEventArgs e)
		{

			if (e.Signal != pullUpsEnabled)
			{

				pullUpsEnabled = e.Signal;
				SetPullUps();
			}
		
		}

		[RaspInput(InputType = IOType.Signal)]
		public void SetPullDown(object sender, SignalEventArgs e)
		{

			if (e.Signal != isPullDown)
			{

				isPullDown = e.Signal;

				if (pullUpsEnabled)
					SetPullUps();
			
			}
		
		}

		private void SetPullUps()
		{
			if (!pullUpsEnabled)
				internalPin.PullUpDown = PullUpDownControl.Pull_OFF;
			else if (!isPullDown)
				internalPin.PullUpDown = PullUpDownControl.Pull_UP;
			else
				internalPin.PullUpDown = PullUpDownControl.Pull_DOWN;
		}

	}
}
