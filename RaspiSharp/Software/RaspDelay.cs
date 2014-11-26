using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category="Delay")]
	public class RaspSignalDelay : RaspElement
	{
		private ulong delay;
		[RaspProperty]
		public ulong Delay
		{
			get { return delay; }
			set { delay = value; }
		}

		[RaspOutput(OutputType=IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		[RaspInput(InputType=IOType.Signal)]
		public void Input(object sender, SignalEventArgs e)
		{
			ulong start = RaspExtern.Timers.bcm2835_st_read();
            
			Runner.AddTask((o) =>
			{
				RaspExtern.Timers.bcm2835_st_delay(start, delay);

				if (Output != null)
					Output(this, e);
			});
		
		}

	}

	[RaspElementCategory(Category = "Delay")]
	public class RaspByteDelay : RaspElement
	{
		private ulong delay;
		[RaspProperty]
		public ulong Delay
		{
			get { return delay; }
			set { delay = value; }
		}

		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output;

		[RaspInput(InputType = IOType.Byte)]
		public void Input(object sender, ByteEventArgs e)
		{
			ulong start = RaspExtern.Timers.bcm2835_st_read();

			Runner.AddTask((o) =>
			{
				RaspExtern.Timers.bcm2835_st_delay(start, delay);

				if (Output != null)
					Output(this, e);
			});

		}

	}

	[RaspElementCategory(Category = "Delay")]
	public class RaspBufferDelay : RaspElement
	{
		private ulong delay;
		[RaspProperty]
		public ulong Delay
		{
			get { return delay; }
			set { delay = value; }
		}

		[RaspOutput(OutputType = IOType.Buffer)]
		public event EventHandler<BufferEventArgs> Output;

		[RaspInput(InputType = IOType.Buffer)]
		public void Input(object sender, BufferEventArgs e)
		{
			ulong start = RaspExtern.Timers.bcm2835_st_read();

			Runner.AddTask((o) =>
			{
				RaspExtern.Timers.bcm2835_st_delay(start, delay);

				if (Output != null)
					Output(this, e);
			});

		}

	}
}
