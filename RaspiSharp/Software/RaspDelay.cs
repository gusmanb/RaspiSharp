using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCM2835;
using static BCM2835.BCM2835Managed;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category="Delay")]
	public class RaspSignalDelay : RaspElement
	{
		private long delay;
		[RaspProperty]
		public long Delay
		{
			get { return delay; }
			set { delay = value; }
		}

		[RaspOutput(OutputType=IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		[RaspInput(InputType=IOType.Signal)]
		public void Input(object sender, SignalEventArgs e)
		{
			Runner.AddTask((o) =>
			{
                BCM2835Managed.bcm2835_delayMicroseconds(delay);

				if (Output != null)
					Output(this, e);
			});
		
		}

	}

	[RaspElementCategory(Category = "Delay")]
	public class RaspByteDelay : RaspElement
	{
		private long delay;
		[RaspProperty]
		public long Delay
		{
			get { return delay; }
			set { delay = value; }
		}

		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output;

		[RaspInput(InputType = IOType.Byte)]
		public void Input(object sender, ByteEventArgs e)
		{
			Runner.AddTask((o) =>
			{
                BCM2835Managed.bcm2835_delayMicroseconds(delay);

                if (Output != null)
					Output(this, e);
			});

		}

	}

	[RaspElementCategory(Category = "Delay")]
	public class RaspBufferDelay : RaspElement
	{
		private long delay;
		[RaspProperty]
		public long Delay
		{
			get { return delay; }
			set { delay = value; }
		}

		[RaspOutput(OutputType = IOType.Buffer)]
		public event EventHandler<BufferEventArgs> Output;

		[RaspInput(InputType = IOType.Buffer)]
		public void Input(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
                BCM2835Managed.bcm2835_delayMicroseconds(delay);

                if (Output != null)
					Output(this, e);
			});

		}

	}
}
