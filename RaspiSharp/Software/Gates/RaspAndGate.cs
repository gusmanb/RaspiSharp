using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category="Virtual gates")]
	public class RaspAndGate : RaspElement
	{

        int usedInputs = 2;

        [RaspProperty]
        public int UsedInputs
        {
            get { return usedInputs; }
            set { usedInputs = value; }
        }

		bool[] signals = new bool[8];

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		bool outputState = false;

		[RaspInput(InputType = IOType.Signal)]
		public void Input0(object sender, SignalEventArgs e)
		{

			Input(e.Signal, 1);

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input1(object sender, SignalEventArgs e)
		{

			Input(e.Signal, 2);

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input2(object sender, SignalEventArgs e)
		{

			Input(e.Signal, 3);

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input3(object sender, SignalEventArgs e)
		{

			Input(e.Signal, 4);

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input4(object sender, SignalEventArgs e)
		{

			Input(e.Signal, 5);

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input5(object sender, SignalEventArgs e)
		{

			Input(e.Signal, 6);

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input6(object sender, SignalEventArgs e)
		{

			Input(e.Signal, 7);

		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input7(object sender, SignalEventArgs e)
		{

			Input(e.Signal, 8);

		}
        
		private void Input(bool Signal, int Channel)
		{
			Runner.AddTask((o) =>
			{
				signals[Channel] = Signal;

				if (Output == null)
					return;

				bool outp = true;

				for (int buc = 0; buc < usedInputs; buc++)
				{

					if (!signals[buc])
					{

						outp = false;
						break;

					}

				}

				if (outp != outputState)
				{

					outputState = outp;

					if (Output != null)
						Output(this, new SignalEventArgs { Signal = outputState });

				}
			});
		}
	}
}
