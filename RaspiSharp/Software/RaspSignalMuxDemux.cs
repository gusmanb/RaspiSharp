using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Virtual gates")]
	public class RaspSignalMultiplexer : RaspElement
	{

		bool inputState = false;
		bool[] outputState = new bool[8];

		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output0;
		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output1;
		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output2;
		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output3;
		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output4;
		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output5;
		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output6;
		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output7;

		EventHandler<SignalEventArgs> currentOutput;
		byte currentOutputNum = 0;

		[RaspInput(InputType=IOType.Byte)]
		public void SelectOutput(object sender, ByteEventArgs e)
		{
			Runner.AddTask((es) =>
			{
				if (e.Value == currentOutputNum)
					return;

				if (currentOutput != null && outputState[currentOutputNum] != false)
				{
					outputState[currentOutputNum] = false;
					currentOutput(this, new SignalEventArgs { Signal = false });
				}

				switch (e.Value)
				{

					case 0:

						currentOutput = null;
						break;

					case 1:

						currentOutput = Output0;
						break;

					case 2:

						currentOutput = Output1;
						break;

					case 4:

						currentOutput = Output2;
						break;

					case 8:

						currentOutput = Output3;
						break;

					case 16:

						currentOutput = Output4;
						break;

					case 32:

						currentOutput = Output5;
						break;

					case 64:

						currentOutput = Output6;
						break;

					case 128:

						currentOutput = Output7;
						break;

					default:

						currentOutput = null;
						break;

				}

				currentOutputNum = e.Value;

				if (currentOutput != null && outputState[currentOutputNum] != inputState)
				{

					outputState[currentOutputNum] = inputState;
					currentOutput(this, new SignalEventArgs { Signal = inputState });

				}
			});
		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input(object sender, SignalEventArgs e)
		{
			Runner.AddTask((es) =>
			{
				inputState = e.Signal;

				if (currentOutput != null && outputState[currentOutputNum] != e.Signal)
				{
					outputState[currentOutputNum] = e.Signal;
					currentOutput(this, e);
				}
			});
		}
	}

	[RaspElementCategory(Category = "Virtual gates")]
	public class RaspSignalDemultiplexer : RaspElement
	{
		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;

		byte currentInput = 0;

		bool outputState = false;
		bool[] inputStates = new bool[8];

		[RaspInput(InputType = IOType.Byte)]
		public void SelectInput(object sender, ByteEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				currentInput = e.Value;
				if (outputState != inputStates[e.Value])
				{
					outputState = inputStates[e.Value];
					Output(this, new SignalEventArgs { Signal = outputState });
				}
			});
		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input0(object sender, SignalEventArgs e)
		{

			Input(e, 1);

		}
		[RaspInput(InputType = IOType.Signal)]
		public void Input1(object sender, SignalEventArgs e)
		{

			Input(e, 2);

		}
		[RaspInput(InputType = IOType.Signal)]
		public void Input2(object sender, SignalEventArgs e)
		{

			Input(e, 3);

		}
		[RaspInput(InputType = IOType.Signal)]
		public void Input3(object sender, SignalEventArgs e)
		{

			Input(e, 4);

		}
		[RaspInput(InputType = IOType.Signal)]
		public void Input4(object sender, SignalEventArgs e)
		{

			Input(e, 5);

		}
		[RaspInput(InputType = IOType.Signal)]
		public void Input5(object sender, SignalEventArgs e)
		{

			Input(e, 6);

		}
		[RaspInput(InputType = IOType.Signal)]
		public void Input6(object sender, SignalEventArgs e)
		{

			Input(e, 7);

		}
		[RaspInput(InputType = IOType.Signal)]
		public void Input7(object sender, SignalEventArgs e)
		{

			Input(e, 8);

		}

		private void Input(SignalEventArgs e, byte ChannelNumber)
		{
			Runner.AddTask((o) =>
			{
				if (inputStates[ChannelNumber] != e.Signal)
				{
					inputStates[ChannelNumber] = e.Signal;

					if (currentInput == ChannelNumber)
					{
						outputState = e.Signal;
						Output(this, e);

					}
				}
			});
		}
	}
}
