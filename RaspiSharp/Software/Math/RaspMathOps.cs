using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
    [RaspElementCategory(Category = "Math operations")]
    public class RaspByteSum : RaspElement
    {
        byte valueA;
        byte valueB;

        bool clockPolarity = false;
        [RaspProperty]
        public bool ClockPolarity
        {
            get { return clockPolarity; }
            set { clockPolarity = value; }
        }

        [RaspOutput(OutputType = IOType.Byte)]
        public event EventHandler<ByteEventArgs> Output;

        [RaspOutput(OutputType = IOType.Signal)]
        public event EventHandler<SignalEventArgs> Overflow;

        [RaspInput(InputType = IOType.Byte)]
        public void InputA(object sender, ByteEventArgs e)
        {

#if DEBUG
            Console.WriteLine("RaspSum received input A " + e.Value);
#endif
            valueA = e.Value;
        }

        [RaspInput(InputType = IOType.Byte)]
        public void InputB(object sender, ByteEventArgs e)
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
                    int val = valueA + valueB;
                    byte fVal = (byte)(val & 0xFF);

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
                        Output(e, new ByteEventArgs { Value = fVal });
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
    public class RaspByteSub : RaspElement
    {
        byte valueA;
        byte valueB;

        bool clockPolarity = false;
        [RaspProperty]
        public bool ClockPolarity
        {
            get { return clockPolarity; }
            set { clockPolarity = value; }
        }

        [RaspOutput(OutputType = IOType.Byte)]
        public event EventHandler<ByteEventArgs> Output;

        [RaspOutput(OutputType = IOType.Signal)]
        public event EventHandler<SignalEventArgs> Overflow;

        [RaspInput(InputType = IOType.Byte)]
        public void InputA(object sender, ByteEventArgs e)
        {

#if DEBUG
            Console.WriteLine("RaspSub received input A " + e.Value);
#endif

            valueA = e.Value;
        }

        [RaspInput(InputType = IOType.Byte)]
        public void InputB(object sender, ByteEventArgs e)
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
                    int val = valueA - valueB;
                    byte fVal = val < 0 ? (byte)(val + 255) : (byte)val;

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
                        Output(e, new ByteEventArgs { Value = fVal });
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
    public class RaspByteMul : RaspElement
    {
        byte valueA;
        byte valueB;

        bool clockPolarity = false;
        [RaspProperty]
        public bool ClockPolarity
        {
            get { return clockPolarity; }
            set { clockPolarity = value; }
        }

        [RaspOutput(OutputType = IOType.Byte)]
        public event EventHandler<ByteEventArgs> Output;

        [RaspOutput(OutputType = IOType.Signal)]
        public event EventHandler<SignalEventArgs> Overflow;

        [RaspInput(InputType = IOType.Byte)]
        public void InputA(object sender, ByteEventArgs e)
        {
            valueA = e.Value;
        }

        [RaspInput(InputType = IOType.Byte)]
        public void InputB(object sender, ByteEventArgs e)
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
                    int val = valueA * valueB;
                    byte fVal = (byte)(val & 0xFF);

                    bool overflow = val != fVal;

                    if (overflow && Overflow != null)
                        Overflow(e, new SignalEventArgs { Signal = clockPolarity });

                    if (Output != null)
                        Output(e, new ByteEventArgs { Value = fVal });

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
    public class RaspByteDiv : RaspElement
    {
        byte valueA;
        byte valueB;

        bool clockPolarity = false;
        [RaspProperty]
        public bool ClockPolarity
        {
            get { return clockPolarity; }
            set { clockPolarity = value; }
        }

        [RaspOutput(OutputType = IOType.Byte)]
        public event EventHandler<ByteEventArgs> Output;

        [RaspOutput(OutputType = IOType.Signal)]
        public event EventHandler<SignalEventArgs> Error;

        [RaspInput(InputType = IOType.Byte)]
        public void InputA(object sender, ByteEventArgs e)
        {
            valueA = e.Value;
        }

        [RaspInput(InputType = IOType.Byte)]
        public void InputB(object sender, ByteEventArgs e)
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
                        if (Error != null)
                            Error(e, new SignalEventArgs { Signal = clockPolarity });
                        if (Output != null)
                            Output(e, new ByteEventArgs { Value = 0xFF });
                        if (Error != null)
                            Error(e, new SignalEventArgs { Signal = !clockPolarity });
                    }
                    else if (Output != null)
                    {
                        byte fVal = (byte)(valueA / valueB);
                        Output(e, new ByteEventArgs { Value = fVal });

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


    [RaspElementCategory(Category = "Math operations")]
    public class RaspByteAnd : RaspElement
    {
        byte valueA;
        byte valueB;

        bool clockPolarity = false;
        [RaspProperty]
        public bool ClockPolarity
        {
            get { return clockPolarity; }
            set { clockPolarity = value; }
        }

        [RaspOutput(OutputType = IOType.Byte)]
        public event EventHandler<ByteEventArgs> Output;
        
        [RaspInput(InputType = IOType.Byte)]
        public void InputA(object sender, ByteEventArgs e)
        {
            valueA = e.Value;
        }

        [RaspInput(InputType = IOType.Byte)]
        public void InputB(object sender, ByteEventArgs e)
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
                    if (Output != null)
                       Output(e, new ByteEventArgs { Value = (byte)(valueA & valueB) });
                    
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
    public class RaspByteOr : RaspElement
    {
        byte valueA;
        byte valueB;

        bool clockPolarity = false;
        [RaspProperty]
        public bool ClockPolarity
        {
            get { return clockPolarity; }
            set { clockPolarity = value; }
        }

        [RaspOutput(OutputType = IOType.Byte)]
        public event EventHandler<ByteEventArgs> Output;

        [RaspInput(InputType = IOType.Byte)]
        public void InputA(object sender, ByteEventArgs e)
        {
            valueA = e.Value;
        }

        [RaspInput(InputType = IOType.Byte)]
        public void InputB(object sender, ByteEventArgs e)
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
                    if (Output != null)
                        Output(e, new ByteEventArgs { Value = (byte)(valueA | valueB) });

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
    public class RaspByteXor : RaspElement
    {
        byte valueA;
        byte valueB;

        bool clockPolarity = false;
        [RaspProperty]
        public bool ClockPolarity
        {
            get { return clockPolarity; }
            set { clockPolarity = value; }
        }

        [RaspOutput(OutputType = IOType.Byte)]
        public event EventHandler<ByteEventArgs> Output;

        [RaspInput(InputType = IOType.Byte)]
        public void InputA(object sender, ByteEventArgs e)
        {
            valueA = e.Value;
        }

        [RaspInput(InputType = IOType.Byte)]
        public void InputB(object sender, ByteEventArgs e)
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
                    if (Output != null)
                        Output(e, new ByteEventArgs { Value = (byte)(valueA ^ valueB) });

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
    
    [RaspElementCategory(Category="Math operations")]
	public class RaspIntegerSum : RaspElement
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
	public class RaspIntegerSub : RaspElement
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
	public class RaspIntegerMul : RaspElement
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
	public class RaspIntegerDiv : RaspElement
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

    [RaspElementCategory(Category = "Math operations")]
    public class RaspIntegerAnd : RaspElement
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
                    if (Output != null)
                        Output(e, new IntegerEventArgs { Value = valueA & valueB });
                    
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
    public class RaspIntegerOr : RaspElement
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
                    if (Output != null)
                        Output(e, new IntegerEventArgs { Value = valueA | valueB });

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
    public class RaspIntegerXor : RaspElement
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
                    if (Output != null)
                        Output(e, new IntegerEventArgs { Value = valueA ^ valueB });

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
