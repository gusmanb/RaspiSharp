using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Transformation")]
	public class RaspSignalToByte : RaspElement
	{

		byte highValue = 0;
		[RaspProperty]
		public byte HighValue
		{
			get { return highValue; }
			set { highValue = value; }
		}


		byte lowValue = 0;
		[RaspProperty]
		public byte LowValue
		{
			get { return lowValue; }
			set { lowValue = value; }
		}

		bool enableHigh = false;
		[RaspProperty]
		public bool EnableHigh
		{
			get { return enableHigh; }
			set { enableHigh = value; }
		}
		bool enableLow = false;
		[RaspProperty]
		public bool EnableLow
		{
			get { return enableLow; }
			set { enableLow = value; }
		}

		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output;

		[RaspInput(InputType = IOType.Signal)]
		public void Input(object sender, SignalEventArgs e)
		{

			if (Output != null)
			{
				if(e.Signal && enableHigh)
					Output(this, new ByteEventArgs { Value = highValue });
				else if(!e.Signal && enableLow)
					Output(this, new ByteEventArgs { Value = lowValue });
			}
		
		}
	}
}
