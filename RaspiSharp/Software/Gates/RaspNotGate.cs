using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Virtual gates")]
	public class RaspNotGate : RaspElement
	{
        bool outputState = false;

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		[RaspInput(InputType = IOType.Signal)]
		public void Input(object Sender, SignalEventArgs e)
		{
			Runner.AddTask((o) =>
			{
                if (outputState == e.Signal)
                {
                    outputState = !e.Signal;

                    if (Output != null)
                        Output(this, new SignalEventArgs { Signal = outputState });
                }
			});
		}
	}
}
