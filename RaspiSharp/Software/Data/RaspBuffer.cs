using RaspiSharp.Software;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Data transfer")]
	public class RaspBuffer : RaspElement
	{
		internal byte[] buffer = new byte[0];
		
		[RaspProperty]
		public int Size { get { return buffer.Length; } set { Resize(value, true); } }

        [RaspProperty]
        public byte[] Data { get { return buffer; } set { buffer = value; } }

        [RaspOutput(OutputType = IOType.Buffer)]
		public event EventHandler<BufferEventArgs> CopiedFrom;
		[RaspOutput(OutputType = IOType.Buffer)]
		public event EventHandler<BufferEventArgs> CopiedTo;

		[RaspMethod]
		public void Load(byte[] Data, int Offset, int Length)
		{

			Buffer.BlockCopy(Data, 0, buffer, Offset, Math.Min(buffer.Length - Offset, Data.Length));
		
		}

		[RaspInput(InputType = IOType.Buffer)]
		public void CopyFrom(object sender, BufferEventArgs e)
		{
			Buffer.BlockCopy(e.Buffer.buffer, 0, buffer, e.Offset, Math.Min((int)(buffer.Length - e.Offset), e.Length));

			if (CopiedFrom != null)
				CopiedFrom(this, new BufferEventArgs { Buffer = this, Length = e.Length, Offset = e.Offset });

		}

		[RaspInput(InputType = IOType.Buffer)]
		public void CopyTo(object sender, BufferEventArgs e)
		{
			Buffer.BlockCopy(buffer, 0, e.Buffer.buffer, e.Offset, Math.Min((int)(buffer.Length - e.Offset), e.Length));

			if (CopiedTo != null)
				CopiedTo(this, e);
		}

		[RaspMethod]
		public void Load(string Data, int Offset)
		{

			byte[] data = Encoding.ASCII.GetBytes(Data);
			Buffer.BlockCopy(data, 0, buffer, Offset, Math.Min(buffer.Length - Offset, data.Length));
		
		}

		[RaspMethod]
		public void Fill(byte Value, int Offset, int Length)
		{

			for (int buc = Offset; buc < Offset + Length; buc++)
				buffer[buc] = Value;
		
		}

		[RaspMethod]
		public void Clear()
		{

			Array.Clear(buffer, 0, buffer.Length);
		
		}

		[RaspMethod]
		public void Resize(int NewLength, bool PreserveData)
		{

			if (!PreserveData)
				buffer = new byte[NewLength];
			else
			{

				byte[] tmpBuffer = new byte[NewLength];
				Buffer.BlockCopy(buffer, 0, tmpBuffer, 0, Math.Min(buffer.Length, tmpBuffer.Length));
				buffer = tmpBuffer;
			}
		
		}

		[RaspMethod]
		public byte[] GetRange(int Start, int Length)
		{

			byte[] data = new byte[Length];
			Buffer.BlockCopy(buffer, Start, data, 0, Length);
			return data;
		
		}
	}

}
