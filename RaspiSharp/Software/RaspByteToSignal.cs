using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Transformation")]
	public class RaspByteToSignal : RaspElement
	{
		byte[] highValues = new byte[0];

		bool outputStatus = false;

		[RaspProperty]
		public byte[] HighValues
		{

			get { return highValues; }
			set { this.highValues = value; }
		}

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		[RaspInput(InputType = IOType.Byte)]
		public void Input(object sender, ByteEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				bool newOut = HighValues.Contains(e.Value);

				if (newOut != outputStatus)
				{
					outputStatus = newOut;
					Output(this, new SignalEventArgs { Signal = outputStatus });
				}

			});

		}
	}
}
