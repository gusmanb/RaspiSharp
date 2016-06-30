using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	public class BufferEventArgs : EventArgs
	{

		public RaspBuffer Buffer { get; internal set; }
		public int Offset { get; internal set; }
		public int Length { get; internal set; }
	}

	public class SignalEventArgs : EventArgs
	{

		public bool Signal { get; set; }

	}

	public class ByteEventArgs : EventArgs
	{

		public byte Value { get; set; }
	
	}

    public class IntegerEventArgs : EventArgs
    {

        public int Value { get; set; }

    }
}
