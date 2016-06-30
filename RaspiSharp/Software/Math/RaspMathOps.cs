using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category="Math operations")]
	public class RaspSum : RaspElement
	{
		int valueA;
        int valueB;

		bool clockPolarity = false;
		[RaspProperty]
		public bool ClockPolarity
		{
			get { return clockPolarity; }
			set { clockPolarity = value; }
		}

		[RaspOutput(OutputType=IOType.Integer)]
		public event EventHandler<IntegerEventArgs> Output;

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Overflow;

		[RaspInput(InputType = IOType.Integer)]
		public void InputA(object sender, IntegerEventArgs e)
		{

#if DEBUG
			Console.WriteLine("RaspSum received input A " + e.Value);
#endif
			valueA = e.Value;
		}

		[RaspInput(InputType = IOType.Integer)]
		public void InputB(object sender, IntegerEventArgs e)
		{
#if DEBUG
			Console.WriteLine("RaspSum received input B " + e.Value);
#endif
			valueB = e.Value;
		
		}

		[RaspInput(InputType = IOType.Signal)]
		public void Clock(object sender, SignalEventArgs e)
		{
#if DEBUG
			Console.WriteLine("RaspSum received clock " + e.Signal);
#endif
			Runner.AddTask((o) =>
			{
				if (e.Signal == clockPolarity)
				{
					long val = valueA + valueB;
					int fVal = (int)(val & int.MaxValue);

					bool overflow = val != fVal;

					if (overflow && Overflow != null)
					{
#if DEBUG
						Console.WriteLine("RaspSum overflow set");
#endif
						Overflow(e, new SignalEventArgs { Signal = clockPolarity });
					}

					if (Output != null)
					{
#if DEBUG
						Console.WriteLine("RaspSum write output " + fVal);
#endif
						Output(e, new IntegerEventArgs { Value = fVal });
					}

					if (overflow && Overflow != null)
					{
#if DEBUG
						Console.WriteLine("RaspSum overflow clear");
#endif
						Overflow(e, new SignalEventArgs { Signal = !clockPolarity });
					}
				}
			});
		}

		[RaspInput(InputType = IOType.Signal)]
		public void Reset(object sender, SignalEventArgs e)
		{

			if (e.Signal == clockPolarity)
			{
#if DEBUG
				Console.WriteLine("RaspSum Reset received");
#endif
				valueA = 0;
				valueB = 0;
			
			}
		
		}
	}

	[RaspElementCategory(Category = "Math operations")]
	public class RaspSub : RaspElement
	{
		int valueA;
        int valueB;

		bool clockPolarity = false;
		[RaspProperty]
		public bool ClockPolarity
		{
			get { return clockPolarity; }
			set { clockPolarity = value; }
		}

		[RaspOutput(OutputType = IOType.Integer)]
		public event EventHandler<IntegerEventArgs> Output;

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Overflow;

		[RaspInput(InputType = IOType.Integer)]
		public void InputA(object sender, IntegerEventArgs e)
		{

#if DEBUG
			Console.WriteLine("RaspSub received input A " + e.Value);
#endif

			valueA = e.Value;
		}

		[RaspInput(InputType = IOType.Integer)]
		public void InputB(object sender, IntegerEventArgs e)
		{
#if DEBUG
			Console.WriteLine("RaspSub received input B " + e.Value);
#endif
			valueB = e.Value;

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Clock(object sender, SignalEventArgs e)
		{

			Runner.AddTask((o) =>
			{
				if (e.Signal == clockPolarity && Output != null)
				{
					long val = valueA - valueB;
					int fVal = val < 0 ? (int)(val + int.MaxValue) : (int)val;

					bool overflow = val != fVal;

					if (overflow && Overflow != null)
					{
#if DEBUG
						Console.WriteLine("RaspSub overflow set");
#endif
						Overflow(e, new SignalEventArgs { Signal = clockPolarity });
					}

					if (Output != null)
					{
#if DEBUG
						Console.WriteLine("RaspSub write output " + fVal);
#endif
						Output(e, new IntegerEventArgs { Value = fVal });
					}

					if (overflow && Overflow != null)
					{
#if DEBUG
						Console.WriteLine("RaspSub overflow clear");
#endif
						Overflow(e, new SignalEventArgs { Signal = !clockPolarity });
					}
				}
			});
		}

		[RaspInput(InputType = IOType.Signal)]
		public void Reset(object sender, SignalEventArgs e)
		{

			if (e.Signal == clockPolarity)
			{
#if DEBUG
				Console.WriteLine("RaspSub Reset received");
#endif
				valueA = 0;
				valueB = 0;

			}

		}
	}

	[RaspElementCategory(Category = "Math operations")]
	public class RaspMul : RaspElement
	{
		int valueA;
        int valueB;

		bool clockPolarity = false;
		[RaspProperty]
		public bool ClockPolarity
		{
			get { return clockPolarity; }
			set { clockPolarity = value; }
		}

		[RaspOutput(OutputType = IOType.Integer)]
		public event EventHandler<IntegerEventArgs> Output;

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Overflow;

		[RaspInput(InputType = IOType.Integer)]
		public void InputA(object sender, IntegerEventArgs e)
		{
			valueA = e.Value;
		}

		[RaspInput(InputType = IOType.Integer)]
		public void InputB(object sender, IntegerEventArgs e)
		{

			valueB = e.Value;

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Clock(object sender, SignalEventArgs e)
		{

			Runner.AddTask((o) =>
			{
				if (e.Signal == clockPolarity)
				{
					long val = valueA * valueB;
                    int fVal = (int)(val & int.MaxValue);

					bool overflow = val != fVal;

					if (overflow && Overflow != null)
						Overflow(e, new SignalEventArgs { Signal = clockPolarity });

					if (Output != null)
						Output(e, new IntegerEventArgs { Value = fVal });

					if (overflow && Overflow != null)
						Overflow(e, new SignalEventArgs { Signal = !clockPolarity });
				}
			});
		}

		[RaspInput(InputType = IOType.Signal)]
		public void Reset(object sender, SignalEventArgs e)
		{

			if (e.Signal == clockPolarity)
			{

				valueA = 0;
				valueB = 0;

			}

		}
	}

	[RaspElementCategory(Category = "Math operations")]
	public class RaspDiv : RaspElement
	{
		int valueA;
        int valueB;

		bool clockPolarity = false;
		[RaspProperty]
		public bool ClockPolarity
		{
			get { return clockPolarity; }
			set { clockPolarity = value; }
		}

		[RaspOutput(OutputType = IOType.Integer)]
		public event EventHandler<IntegerEventArgs> Output;

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Error;

		[RaspInput(InputType = IOType.Integer)]
		public void InputA(object sender, IntegerEventArgs e)
		{
			valueA = e.Value;
		}

		[RaspInput(InputType = IOType.Integer)]
		public void InputB(object sender, IntegerEventArgs e)
		{

			valueB = e.Value;

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Clock(object sender, SignalEventArgs e)
		{

			Runner.AddTask((o) =>
			{
				if (e.Signal == clockPolarity)
				{
					if (valueB == 0)
					{
						if(Error != null)
							Error(e, new SignalEventArgs { Signal = clockPolarity });
						if(Output != null)
							Output(e, new IntegerEventArgs { Value = 0xFF });
						if(Error != null)
							Error(e, new SignalEventArgs { Signal = !clockPolarity });
					}
					else if(Output != null)
					{
						int fVal = (int)(valueA / valueB);
						Output(e, new IntegerEventArgs { Value = fVal });
					
					}
					
				}
			});
		}

		[RaspInput(InputType = IOType.Signal)]
		public void Reset(object sender, SignalEventArgs e)
		{

			if (e.Signal == clockPolarity)
			{

				valueA = 0;
				valueB = 0;

			}

		}
	}

}
