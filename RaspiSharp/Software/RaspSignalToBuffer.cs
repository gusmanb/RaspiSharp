using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Transformation")]
	public class RaspSignalToBuffer : RaspElement
	{
		byte[] highValue = new byte[0];
		byte[] lowValue = new byte[0];

		[RaspProperty]
		public byte[] HighValue
		{

			get { return highValue; }
			set { this.highValue = value; }
		}

		[RaspProperty]
		public byte[] LowValue
		{

			get { return lowValue; }
			set { this.lowValue = value; }
		}

		RaspBuffer buffer;
		[RaspProperty]
		public RaspBuffer Buffer
		{
			get { return buffer; }
			set { buffer = value; }
		}

		int offset;
		[RaspProperty]
		public int Offset
		{
			get { return offset; }
			set { offset = value; }
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

		bool processBuffer = false;
		[RaspProperty]
		public bool ProcessBuffer
		{
			get { return processBuffer; }
			set { processBuffer = value; }
		}


		[RaspOutput(OutputType = IOType.Buffer)]
		public event EventHandler<BufferEventArgs> Output;

		[RaspInput(InputType = IOType.Signal)]
		public void Input(object sender, SignalEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (e.Signal && enableHigh)
				{
					if(processBuffer)
						buffer.Load(highValue, offset, highValue.Length);

					if (Output != null)
						Output(this.Output, new BufferEventArgs { Buffer = buffer, Offset = offset, Length = highValue.Length });
				}
				else if (!e.Signal && enableLow)
				{
					if(processBuffer)
						buffer.Load(lowValue, offset, lowValue.Length);

					if (Output != null)
						Output(this.Output, new BufferEventArgs { Buffer = buffer, Offset = offset, Length = lowValue.Length });
				
				}
			});
		}
	}
}
