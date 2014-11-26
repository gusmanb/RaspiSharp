using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category="Fixed signals")]
	public class RaspFixedSignal : RaspElement
	{
		bool high;

		[RaspProperty]
		public bool High
		{
			get { return high; }
			set { high = value; }
		}

		[RaspOutput(OutputType=IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		public void Init()
		{

			Runner.AddTask((w) => {

				if (Output != null)
					Output(this, new SignalEventArgs { Signal = high });

			
			});

		
		}
	}
}
