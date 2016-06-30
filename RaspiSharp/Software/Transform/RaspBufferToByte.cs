using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Transformation")]
	public class RaspBufferToByte : RaspElement
	{
        byte outputState = 0;

		public int offset;
		[RaspProperty]
		public int Offset
		{
			get { return offset; }
			set { offset = value; }
		}

		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output;

		[RaspInput(InputType = IOType.Buffer)]
		public void Input(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{

                var val = e.Buffer.buffer[offset];

                if (val != outputState)
                {
                    outputState = val;

                    if (Output != null)
                        Output(this, new ByteEventArgs { Value = val });
                }
			});
		}
	}
}
