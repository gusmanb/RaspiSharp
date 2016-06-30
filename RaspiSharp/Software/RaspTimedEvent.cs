using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BCM2835;
using static BCM2835.BCM2835Managed;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category="Timed events")]
	public class RaspTimedSignalEvent : RaspElement
	{
		bool repeat;
		[RaspProperty]
		public bool Repeat
		{
			get { return repeat; }
			set { repeat = value; }
		}
		long halfCycleLength;
		[RaspProperty]
		public long HalfCycleLength
		{
			get { return halfCycleLength; }
			set { halfCycleLength = value; }
		}
		bool highValue;
		[RaspProperty]
		public bool HighValue
		{
			get { return highValue; }
			set { highValue = value; }
		}

		bool outputEnabled = false;

		[RaspOutput(OutputType=IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		Thread th;

		[RaspInput(InputType = IOType.Signal)]
		public void EnableOutput(object sender, SignalEventArgs e)
		{

			outputEnabled = e.Signal;

			if (outputEnabled)
			{

				th = new Thread(() =>
				{
					while (outputEnabled)
					{


						Runner.AddTask((w) =>
						{
							if (Output != null)
								Output(this, new SignalEventArgs { Signal = highValue });
						});

                        BCM2835Managed.bcm2835_delayMicroseconds(halfCycleLength);

						if (!outputEnabled)
							return;

						Runner.AddTask((w) =>
						{
							if (Output != null)
								Output(this, new SignalEventArgs { Signal = !highValue });
						});

                        BCM2835Managed.bcm2835_delayMicroseconds(halfCycleLength);

                        if (!repeat)
							return;
					}

				});

				th.Start();

			}
			else
			{

				if (th != null)
					th.Abort();
			
			}
			
		}

	}

	[RaspElementCategory(Category = "Timed events")]
	public class RaspTimedByteEvent : RaspElement
	{
		bool repeat;
		[RaspProperty]
		public bool Repeat
		{
			get { return repeat; }
			set { repeat = value; }
		}
		long halfCycleLength;
		[RaspProperty]
		public long HalfCycleLength
		{
			get { return halfCycleLength; }
			set { halfCycleLength = value; }
		}

		byte highValue;
		[RaspProperty]
		public byte HighValue
		{
			get { return highValue; }
			set { highValue = value; }
		}

		byte lowValue;
		[RaspProperty]
		public byte LowValue
		{
			get { return lowValue; }
			set { lowValue = value; }
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

		bool outputEnabled = false;

		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output;

		Thread th;

		[RaspInput(InputType = IOType.Signal)]
		public void EnableOutput(object sender, SignalEventArgs e)
		{
			var wasEnabled = outputEnabled;

			outputEnabled = e.Signal;

			if (!wasEnabled && outputEnabled)
			{
				th = new Thread(() =>
				{

					ulong start;

					while (outputEnabled)
					{
						if (enableHigh)
						{
							Runner.AddTask((w) =>
							{
								Output(this, new ByteEventArgs { Value = highValue });
							});
						}

                        BCM2835Managed.bcm2835_delayMicroseconds(halfCycleLength);

                        if (!outputEnabled)
							return;

						if (enableLow)
						{
							Runner.AddTask((w) =>
							{
								Output(this, new ByteEventArgs { Value = lowValue });
							});
						}

                        BCM2835Managed.bcm2835_delayMicroseconds(halfCycleLength);

                        if (!repeat)
							return;
					}

				});

				th.Start();

			}
			else
			{

				if (th != null)
					th.Abort();

			}

		}

	}

	[RaspElementCategory(Category = "Timed events")]
	public class RaspTimedBufferEvent : RaspElement
	{
		bool repeat;
		[RaspProperty]
		public bool Repeat
		{
			get { return repeat; }
			set { repeat = value; }
		}
		long halfCycleLength;
		[RaspProperty]
		public long HalfCycleLength
		{
			get { return halfCycleLength; }
			set { halfCycleLength = value; }
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

		byte[] highValue = new byte[0];
		byte[] lowValue = new byte[0];

		[RaspProperty]
		public byte[] HighValue
		{

			get { return highValue; }
			set { highValue = value; }
		}

		[RaspProperty]
		public byte[] LowValue
		{

			get { return lowValue; }
			set { lowValue = value; }
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

		bool processBuffer = false;
		[RaspProperty]
		public bool ProcessBuffer
		{
			get { return processBuffer; }
			set { processBuffer = value; }
		}

		bool outputEnabled = false;

		[RaspOutput(OutputType = IOType.Buffer)]
		public event EventHandler<BufferEventArgs> Output;

		Thread th;

		[RaspInput(InputType = IOType.Signal)]
		public void EnableOutput(object sender, SignalEventArgs e)
		{
			var wasEnabled = outputEnabled;

			outputEnabled = e.Signal;

			if (!wasEnabled && outputEnabled)
			{
				th = new Thread(() =>
				{

					ulong start;

					while (outputEnabled)
					{

						if (enableHigh)
						{
							Runner.AddTask((w) =>
							{
								if (processBuffer)
									buffer.Load(highValue, offset, highValue.Length);

								if (Output != null)
									Output(this.Output, new BufferEventArgs { Buffer = buffer, Offset = offset, Length = highValue.Length });
							});
						}

                        BCM2835Managed.bcm2835_delayMicroseconds(halfCycleLength);

                        if (!outputEnabled)
							return;

						if (enableLow)
						{
							Runner.AddTask((w) =>
							{
								if (processBuffer)
									buffer.Load(lowValue, offset, lowValue.Length);

								if (Output != null)
									Output(this.Output, new BufferEventArgs { Buffer = buffer, Offset = offset, Length = highValue.Length });
							});
						}

                        BCM2835Managed.bcm2835_delayMicroseconds(halfCycleLength);

                        if (!repeat)
							return;
					}

				});

				th.Start();

			}
			else
			{

				if (th != null)
					th.Abort();

			}

		}

	}
}
