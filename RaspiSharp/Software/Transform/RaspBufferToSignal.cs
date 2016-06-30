using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Transformation")]
	public class RaspBufferToSignal : RaspElement
	{
		public int offset;
		[RaspProperty]
		public int Offset
		{
			get { return offset; }
			set { offset = value; }
		}

		private byte[] value;

		[RaspProperty]
		public byte[] Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

        bool valueIsHigh = false;

        [RaspProperty]
        public bool ValueIsHigh
        {

            get { return valueIsHigh; }
            set { this.valueIsHigh = value; }
        }

        bool outputState;

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		[RaspInput(InputType = IOType.Buffer)]
		public void Input(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				bool newState = true;

				if (e.Buffer.Size - offset >= value.Length)
				{
					for (int buc = offset; buc < offset + value.Length; buc++)
					{
						if (e.Buffer.buffer[buc] != value[buc - offset])
						{

							newState = false;
							break;
						
						}
					}
				}
				else
					newState = false;

                newState = valueIsHigh ? newState : !newState;

				if (newState != outputState && Output != null)
				{
					outputState = newState;
					Output(this, new SignalEventArgs { Signal = newState });
				}
			});
		}
	}
}
