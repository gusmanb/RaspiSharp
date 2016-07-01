using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Transformation")]
	public class RaspByteToSignal : RaspElement
	{
        byte value = 0;
        

		bool outputStatus = false;

		[RaspProperty]
		public byte Value
		{

			get { return value; }
			set { this.value = value; }
		}

        bool valueIsHigh = false;

        [RaspProperty]
        public bool ValueIsHigh
        {

            get { return valueIsHigh; }
            set { this.valueIsHigh = value; }
        }
        
        [RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		[RaspInput(InputType = IOType.Byte)]
		public void Input(object sender, ByteEventArgs e)
		{
			Runner.AddTask((o) =>
			{
                bool newOut = valueIsHigh ? e.Value == value : e.Value != value;

                if (newOut != outputStatus)
				{
					outputStatus = newOut;
					Output(this, new SignalEventArgs { Signal = outputStatus });
				}

			});

		}
	}
}
