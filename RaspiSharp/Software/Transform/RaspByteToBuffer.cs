using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Transformation")]
	public class RaspByteToBuffer : RaspElement
	{

        byte outputState = 0;

		public RaspBuffer buffer;
		
		[RaspProperty]
		public RaspBuffer Buffer
		{
			get { return buffer; }
			set { buffer = value; }
		}

		
		public int offset;
		[RaspProperty]
		public int Offset
		{
			get { return offset; }
			set { offset = value; }
		}

		
		public int length;
		[RaspProperty]
		public int Length
		{
			get { return length; }
			set { length = value; }
		}

		[RaspOutput(OutputType=IOType.Buffer)]
		public event EventHandler<BufferEventArgs> Output;

		[RaspInput(InputType = IOType.Byte)]
		public void Input(object sender, ByteEventArgs e)
		{
			Runner.AddTask((o) =>
			{

                if (outputState != e.Value)
                {
                    outputState = e.Value;

                    for (int buc = offset; buc < offset + length; buc++)
                        buffer.buffer[buc] = e.Value;

                    if (Output != null)
                        Output(this, new BufferEventArgs { Buffer = buffer, Offset = offset, Length = length });
                }
			});
		}
	}
}
