using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Virtual gates")]
	public class RaspCounter : RaspElement
	{
		byte minValue;

		public byte MinValue
		{
			get { return minValue; }
			set { minValue = value; }
		}
		byte maxValue;

		public byte MaxValue
		{
			get { return maxValue; }
			set { maxValue = value; }
		}

		byte currentValue = 0;

		bool outputEnabled = false;

		[RaspOutput(OutputType=IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output;

		[RaspInput(InputType=IOType.Signal)]
		public void EnableOutput(object sender, SignalEventArgs e)
		{
			Runner.AddTask((es) =>
			{
				if (outputEnabled != e.Signal)
				{
					outputEnabled = e.Signal;

					if (outputEnabled && Output != null)
						Output(this, new ByteEventArgs { Value = currentValue });

				}
			});
		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input(object sender, SignalEventArgs e)
		{
			Runner.AddTask((o) => {

				if (!e.Signal)
					return;

				currentValue++;

				if (currentValue > maxValue)
					currentValue = minValue;

				if (currentValue < minValue)
					currentValue = minValue;

				if (outputEnabled && Output != null)
					Output(this, new ByteEventArgs {  Value = currentValue });
			
			}); 
		
		}
	}
}
