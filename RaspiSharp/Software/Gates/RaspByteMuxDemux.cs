using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Virtual gates")]
	public class RaspByteMultiplexer : RaspElement
	{
		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output0;
		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output1;
		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output2;
		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output3;
		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output4;
		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output5;
		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output6;
		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output7;

		EventHandler<ByteEventArgs> currentOutput;

		[RaspInput(InputType = IOType.Byte)]
		public void EnableOutputs(object sender, ByteEventArgs e)
		{

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

		}

		[RaspInput(InputType = IOType.Byte)]
		public void Input(object sender, ByteEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (currentOutput != null)
					currentOutput(this, e);
			});
		}
	}

	[RaspElementCategory(Category = "Virtual gates")]
	public class RaspByteDemultiplexer : RaspElement
	{
		[RaspOutput(OutputType = IOType.Byte)]
		public event EventHandler<ByteEventArgs> Output;

		byte currentInput = 0;

		[RaspInput(InputType = IOType.Byte)]
		public void SelectInput(object sender, ByteEventArgs e)
		{

			currentInput = e.Value;

		}

		[RaspInput(InputType = IOType.Byte)]
		public void Input0(object sender, ByteEventArgs e)
		{

			Input(e, 1);

		}
		[RaspInput(InputType = IOType.Byte)]
		public void Input1(object sender, ByteEventArgs e)
		{

			Input(e, 2);

		}
		[RaspInput(InputType = IOType.Byte)]
		public void Input2(object sender, ByteEventArgs e)
		{

			Input(e, 3);

		}
		[RaspInput(InputType = IOType.Byte)]
		public void Input3(object sender, ByteEventArgs e)
		{

			Input(e, 4);

		}
		[RaspInput(InputType = IOType.Byte)]
		public void Input4(object sender, ByteEventArgs e)
		{

			Input(e, 5);

		}
		[RaspInput(InputType = IOType.Byte)]
		public void Input5(object sender, ByteEventArgs e)
		{

			Input(e, 6);

		}
		[RaspInput(InputType = IOType.Byte)]
		public void Input6(object sender, ByteEventArgs e)
		{

			Input(e, 7);

		}
		[RaspInput(InputType = IOType.Byte)]
		public void Input7(object sender, ByteEventArgs e)
		{

			Input(e, 8);

		}

		private void Input(ByteEventArgs e, byte ChannelNumber)
		{
			Runner.AddTask((o) =>
			{
				if (currentInput == ChannelNumber)
					Output(this, e);
			});
		}
	}
}
