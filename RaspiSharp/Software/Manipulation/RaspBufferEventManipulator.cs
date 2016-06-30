using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Manipulation")]
	public class RaspBufferEventManipulator : RaspElement
	{
		private int offset;
		[RaspProperty]
		public int Offset
		{
			get { return offset; }
			set { offset = value; }
		}

		private int length;
		[RaspProperty]
		public int Length
		{
			get { return length; }
			set { length = value; }
		}

		[RaspOutput(OutputType = IOType.Buffer)]
		public event EventHandler<BufferEventArgs> Output;

		[RaspInput(InputType=IOType.Buffer)]
		public void Input(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				e.Offset = offset;
				e.Length = length;

				if (Output != null)
					Output(this, e);
			});
		}
	}
}
