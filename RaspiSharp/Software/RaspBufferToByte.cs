using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Transformation")]
	public class RaspBufferToByte : RaspElement
	{
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
				if (Output != null)
					Output(this, new ByteEventArgs { Value = e.Buffer.buffer[offset] });
			});
		}
	}
}
