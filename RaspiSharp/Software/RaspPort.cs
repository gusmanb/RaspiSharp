using RaspiSharp.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Ports")]
    public abstract class RaspPort : RaspElement, IDisposable
    {

		[RaspInput(InputType = IOType.Buffer)]
		public abstract void ReadBuffer(object sender, BufferEventArgs e);
		[RaspInput(InputType = IOType.Buffer)]
		public abstract void WriteBuffer(object sender, BufferEventArgs e);
		[RaspInput(InputType = IOType.Buffer)]
		public abstract void TransferBuffer(object sender, BufferEventArgs e);

		[RaspOutput(OutputType = IOType.Buffer)]
		public abstract event EventHandler<BufferEventArgs> ReadBegin;
		[RaspOutput(OutputType = IOType.Buffer)]
		public abstract event EventHandler<BufferEventArgs> ReadEnd;
		[RaspOutput(OutputType = IOType.Buffer)]
		public abstract event EventHandler<BufferEventArgs> WriteBegin;
		[RaspOutput(OutputType = IOType.Buffer)]
		public abstract event EventHandler<BufferEventArgs> WriteEnd;
		[RaspOutput(OutputType = IOType.Buffer)]
		public abstract event EventHandler<BufferEventArgs> TransferBegin;
		[RaspOutput(OutputType = IOType.Buffer)]
		public abstract event EventHandler<BufferEventArgs> TransferEnd;

		public virtual void Dispose() { }
    }

	[RaspElementCategory(Category = "Ports")]
	public class RaspBitPort : RaspPort
	{
		RPiGPIOPin dataPin;

		[RaspProperty]
		public RPiGPIOPin DataPin
		{
			get { return dataPin; }
			set { dataPin = value; }
		}

		RPiGPIOPin clockPin;

		[RaspProperty]
		public RPiGPIOPin ClockPin
		{
			get { return clockPin; }
			set { clockPin = value; }
		}

		bool polarity;

		[RaspProperty]
		public bool Polarity
		{
			get { return polarity; }
			set { polarity = value; }
		}

		UInt32 lowCycle;

		[RaspProperty]
		public UInt32 LowCycle
		{
			get { return lowCycle; }
			set { lowCycle = value; }
		}

		UInt32 highCycle;

		[RaspProperty]
		public UInt32 HighCycle
		{
			get { return highCycle; }
			set { highCycle = value; }
		}

		public override event EventHandler<BufferEventArgs> ReadBegin;
		public override event EventHandler<BufferEventArgs> ReadEnd;
		public override event EventHandler<BufferEventArgs> WriteBegin;
		public override event EventHandler<BufferEventArgs> WriteEnd;
		public override event EventHandler<BufferEventArgs> TransferBegin;
		public override event EventHandler<BufferEventArgs> TransferEnd;

		public unsafe override void ReadBuffer(object sender, BufferEventArgs e)
		{

			Runner.AddTask((o) =>
			{
				if (ReadBegin != null)
					ReadBegin(this, e);

				if (e.Length == 1)
					e.Buffer.buffer[e.Offset] = RaspExtern.Ports.readBitBangByte(dataPin, clockPin, polarity, lowCycle, highCycle);
				else
				{

					fixed (byte* bData = e.Buffer.buffer)
						RaspExtern.Ports.readBitBangBuffer(bData + e.Offset, (uint)e.Length, dataPin, clockPin, polarity, lowCycle, highCycle);

				}

				if (ReadEnd != null)
					ReadEnd(this, e);
			});
		}

		public unsafe override void WriteBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (WriteBegin != null)
					WriteBegin(this, e);

				if (e.Length == 1)
					RaspExtern.Ports.writeBitBangByte(e.Buffer.buffer[e.Offset], dataPin, clockPin, polarity, lowCycle, highCycle);
				else
				{

					fixed (byte* bData = e.Buffer.buffer)
						RaspExtern.Ports.writeBitBangBuffer(bData + e.Offset, (uint)e.Length, dataPin, clockPin, polarity, lowCycle, highCycle);

				}

				if (WriteEnd != null)
					WriteEnd(this, e); ;
			});
		}

		public unsafe override void TransferBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (TransferBegin != null)
					TransferBegin(this, e);

				if (e.Length == 1)
				{
					RaspExtern.Ports.writeBitBangByte(e.Buffer.buffer[e.Offset], dataPin, clockPin, polarity, lowCycle, highCycle);
					e.Buffer.buffer[e.Offset] = RaspExtern.Ports.readBitBangByte(dataPin, clockPin, polarity, lowCycle, highCycle);
				}
				else
				{

					fixed (byte* bData = e.Buffer.buffer)
					{
						RaspExtern.Ports.writeBitBangBuffer(bData + e.Offset, (uint)e.Length, dataPin, clockPin, polarity, lowCycle, highCycle);
						RaspExtern.Ports.readBitBangBuffer(bData + e.Offset, (uint)e.Length, dataPin, clockPin, polarity, lowCycle, highCycle);
					}
				}

				if (TransferEnd != null)
					TransferEnd(this, e);
			});
		}

	}

	[RaspElementCategory(Category = "Ports")]
	public class RaspNibblePort : RaspPort
	{
		RPiGPIOPin dataPin0;
		[RaspProperty]
		public RPiGPIOPin DataPin0
		{
			get { return dataPin0; }
			set { dataPin0 = value; }
		}
		RPiGPIOPin dataPin1;
		[RaspProperty]
		public RPiGPIOPin DataPin1
		{
			get { return dataPin1; }
			set { dataPin1 = value; }
		}
		RPiGPIOPin dataPin2;
		[RaspProperty]
		public RPiGPIOPin DataPin2
		{
			get { return dataPin2; }
			set { dataPin2 = value; }
		}
		RPiGPIOPin dataPin3;
		[RaspProperty]
		public RPiGPIOPin DataPin3
		{
			get { return dataPin3; }
			set { dataPin3 = value; }
		}
		RPiGPIOPin clockPin;
		[RaspProperty]
		public RPiGPIOPin ClockPin
		{
			get { return clockPin; }
			set { clockPin = value; }
		}

		bool polarity;
		[RaspProperty]
		public bool Polarity
		{
			get { return polarity; }
			set { polarity = value; }
		}

		UInt32 lowCycle;
		[RaspProperty]
		public UInt32 LowCycle
		{
			get { return lowCycle; }
			set { lowCycle = value; }
		}

		UInt32 highCycle;
		[RaspProperty]
		public UInt32 HighCycle
		{
			get { return highCycle; }
			set { highCycle = value; }
		}

		public override event EventHandler<BufferEventArgs> ReadBegin;
		public override event EventHandler<BufferEventArgs> ReadEnd;
		public override event EventHandler<BufferEventArgs> WriteBegin;
		public override event EventHandler<BufferEventArgs> WriteEnd;
		public override event EventHandler<BufferEventArgs> TransferBegin;
		public override event EventHandler<BufferEventArgs> TransferEnd;

		public unsafe override void ReadBuffer(object sender, BufferEventArgs e)
		{

			Runner.AddTask((o) =>
			{
				if (ReadBegin != null)
					ReadBegin(this, e);

				if (e.Length == 1)
					e.Buffer.buffer[e.Offset] = RaspExtern.Ports.readNibbleByte(dataPin0, dataPin1, dataPin2, dataPin3, clockPin, polarity, lowCycle, highCycle);
				else
				{

					fixed (byte* bData = e.Buffer.buffer)
						RaspExtern.Ports.readNibbleBuffer(bData + e.Offset, (uint)e.Length, dataPin0, dataPin1, dataPin2, dataPin3, clockPin, polarity, lowCycle, highCycle);

				}

				if (ReadEnd != null)
					ReadEnd(this, e);
			});
		}

		public unsafe override void WriteBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (WriteBegin != null)
					WriteBegin(this, e);

				if (e.Length == 1)
					RaspExtern.Ports.writeNibbleByte(e.Buffer.buffer[e.Offset], dataPin0, dataPin1, dataPin2, dataPin3, clockPin, polarity, lowCycle, highCycle);
				else
				{

					fixed (byte* bData = e.Buffer.buffer)
						RaspExtern.Ports.writeNibbleBuffer(bData + e.Offset, (uint)e.Length, dataPin0, dataPin1, dataPin2, dataPin3, clockPin, polarity, lowCycle, highCycle);

				}

				if (WriteEnd != null)
					WriteEnd(this, e);
			});
		}

		public unsafe override void TransferBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (TransferBegin != null)
					TransferBegin(this, e);

				if (e.Length == 1)
				{
					RaspExtern.Ports.writeNibbleByte(e.Buffer.buffer[e.Offset], dataPin0, dataPin1, dataPin2, dataPin3, clockPin, polarity, lowCycle, highCycle);
					e.Buffer.buffer[e.Offset] = RaspExtern.Ports.readNibbleByte(dataPin0, dataPin1, dataPin2, dataPin3, clockPin, polarity, lowCycle, highCycle);
				}
				else
				{

					fixed (byte* bData = e.Buffer.buffer)
					{
						RaspExtern.Ports.writeNibbleBuffer(bData + e.Offset, (uint)e.Length, dataPin0, dataPin1, dataPin2, dataPin3, clockPin, polarity, lowCycle, highCycle);
						RaspExtern.Ports.readNibbleBuffer(bData + e.Offset, (uint)e.Length, dataPin0, dataPin1, dataPin2, dataPin3, clockPin, polarity, lowCycle, highCycle);
					}
				}

				if (TransferEnd != null)
					TransferEnd(this, e);
			});
		}

	}

	[RaspElementCategory(Category = "Ports")]
	public class RaspOctetPort : RaspPort
	{
		RPiGPIOPin[] pinList;

		public RPiGPIOPin[] PinList
		{
			get { return pinList; }
			set { pinList = value; }
		}

		RPiGPIOPin clockPin;

		public RPiGPIOPin ClockPin
		{
			get { return clockPin; }
			set { clockPin = value; }
		}

		bool polarity;

		[RaspProperty]
		public bool Polarity
		{
			get { return polarity; }
			set { polarity = value; }
		}

		UInt32 lowCycle;

		[RaspProperty]
		public UInt32 LowCycle
		{
			get { return lowCycle; }
			set { lowCycle = value; }
		}

		UInt32 highCycle;

		[RaspProperty]
		public UInt32 HighCycle
		{
			get { return highCycle; }
			set { highCycle = value; }
		}

		public override event EventHandler<BufferEventArgs> ReadBegin;
		public override event EventHandler<BufferEventArgs> ReadEnd;
		public override event EventHandler<BufferEventArgs> WriteBegin;
		public override event EventHandler<BufferEventArgs> WriteEnd;
		public override event EventHandler<BufferEventArgs> TransferBegin;
		public override event EventHandler<BufferEventArgs> TransferEnd;

		public unsafe override void ReadBuffer(object sender, BufferEventArgs e)
		{

			Runner.AddTask((o) =>
			{
				if (ReadBegin != null)
					ReadBegin(this, e);

				if (e.Length == 1)
					e.Buffer.buffer[e.Offset] = RaspExtern.Ports.readOctetByte(pinList, clockPin, polarity, lowCycle, highCycle);
				else
				{

					fixed (byte* bData = e.Buffer.buffer)
						RaspExtern.Ports.readOctetBuffer(bData + e.Offset, (uint)e.Length, pinList, clockPin, polarity, lowCycle, highCycle);

				}

				if (ReadEnd != null)
					ReadEnd(this, e);
			});
		}

		public unsafe override void WriteBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (WriteBegin != null)
					WriteBegin(this, e);

				if (e.Length == 1)
					RaspExtern.Ports.writeOctetByte(e.Buffer.buffer[e.Offset], pinList, clockPin, polarity, lowCycle, highCycle);
				else
				{

					fixed (byte* bData = e.Buffer.buffer)
						RaspExtern.Ports.writeOctetBuffer(bData + e.Offset, (uint)e.Length, pinList, clockPin, polarity, lowCycle, highCycle);

				}

				if (WriteEnd != null)
					WriteEnd(this, e);
			});
		}

		public unsafe override void TransferBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (TransferBegin != null)
					TransferBegin(this, e);

				if (e.Length == 1)
				{
					RaspExtern.Ports.writeOctetByte(e.Buffer.buffer[e.Offset], pinList, clockPin, polarity, lowCycle, highCycle);
					e.Buffer.buffer[e.Offset] = RaspExtern.Ports.readOctetByte(pinList, clockPin, polarity, lowCycle, highCycle);
				}
				else
				{

					fixed (byte* bData = e.Buffer.buffer)
					{
						RaspExtern.Ports.writeOctetBuffer(bData + e.Offset, (uint)e.Length, pinList, clockPin, polarity, lowCycle, highCycle);
						RaspExtern.Ports.readOctetBuffer(bData + e.Offset, (uint)e.Length, pinList, clockPin, polarity, lowCycle, highCycle);
					}
				}

				if (TransferEnd != null)
					TransferEnd(this, e);
			});
		}

	}

	[RaspElementCategory(Category = "Ports")]
	public class RaspSPIPort : RaspPort
	{

		SPIMode dMode;
		[RaspProperty]
		public SPIMode DataMode
		{
			get { return dMode; }
			set { dMode = value; RaspExtern.SPI.bcm2835_spi_setDataMode(value); }
		}

		SPIBitOrder bOrder;
		[RaspProperty]
		public SPIBitOrder BitOrder
		{
			get { return bOrder; }
			set { bOrder = value; RaspExtern.SPI.bcm2835_spi_setBitOrder(value); }
		}

		SPIClockDivider cDivider;
		[RaspProperty]
		public SPIClockDivider ClockDivider
		{
			get { return cDivider; }
			set { cDivider = value; RaspExtern.SPI.bcm2835_spi_setClockDivider(value); }
		}

		ChipSelect cSelect;
		[RaspProperty]
		public ChipSelect ChipSelect
		{
			get { return cSelect; }
			set { cSelect = value; RaspExtern.SPI.bcm2835_spi_chipSelect(value); }
		}

		bool cSelPol;
		[RaspProperty]
		public bool ChipSelectPolarity
		{
			get { return cSelPol; }
			set { cSelPol = value; RaspExtern.SPI.bcm2835_spi_setChipSelectPolarity(ChipSelect, (byte)(value ? 1 : 0)); }
		}

		public RaspSPIPort()
		{

			RaspExtern.SPI.bcm2835_spi_begin();

		}

		public override unsafe void ReadBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (ReadBegin != null)
					ReadBegin(this, e);

				e.Buffer.Fill(0xFF, e.Offset, e.Length);

				fixed (byte* bData = &e.Buffer.buffer[e.Offset])
					RaspExtern.SPI.bcm2835_spi_transfern(bData, (uint)e.Length);

				if (ReadEnd != null)
					ReadEnd(this, e);

			});
		}

		public override unsafe void WriteBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (WriteBegin != null)
					WriteBegin(this, e);

				fixed (byte* wData = &e.Buffer.buffer[e.Offset])
					RaspExtern.SPI.bcm2835_spi_writenb(wData, (uint)e.Length);  

				if (WriteEnd != null)
					WriteEnd(this, e);
			});
		}

		public override unsafe void TransferBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (TransferBegin != null)
					TransferBegin(this, e);

				fixed (byte* bData = &e.Buffer.buffer[e.Offset])
					RaspExtern.SPI.bcm2835_spi_transfern(bData, (uint)e.Length);

				if (TransferEnd != null)
					TransferEnd(this, e);
			});
		}

		public override event EventHandler<BufferEventArgs> ReadBegin;
		public override event EventHandler<BufferEventArgs> ReadEnd;
		public override event EventHandler<BufferEventArgs> WriteBegin;
		public override event EventHandler<BufferEventArgs> WriteEnd;
		public override event EventHandler<BufferEventArgs> TransferBegin;
		public override event EventHandler<BufferEventArgs> TransferEnd;

		public override void Dispose()
		{
			RaspExtern.SPI.bcm2835_spi_end();
			base.Dispose();
		}
	}

	[RaspElementCategory(Category = "Ports")]
	public class RaspI2CPort : RaspPort
	{

		byte slaveAddres;
		[RaspProperty]
		public byte SlaveAddress
		{

			get { return slaveAddres; }
			set
			{

				slaveAddres = value;
				RaspExtern.I2C.bcm2835_i2c_setSlaveAddress(value);


			}
		}

		uint baudRate;
		[RaspProperty]
		public uint BaudRate
		{

			get { return baudRate; }
			set
			{

				baudRate = value;
				RaspExtern.I2C.bcm2835_i2c_set_baudrate(value);

			}

		}

		public RaspI2CPort()
		{

			RaspExtern.I2C.bcm2835_i2c_begin();
		
		}

		public override unsafe void ReadBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (ReadBegin != null)
					ReadBegin(this, e);

				fixed (byte* bData = &e.Buffer.buffer[e.Offset])
					RaspExtern.I2C.bcm2835_i2c_read(bData, (uint)e.Length);

				if (ReadEnd != null)
					ReadEnd(this, e);

			});
		}

		public override unsafe void WriteBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (WriteBegin != null)
					WriteBegin(this, e);

				fixed (byte* bData = &e.Buffer.buffer[e.Offset])
					RaspExtern.I2C.bcm2835_i2c_write(bData, (uint)e.Length);

				if (WriteEnd != null)
					WriteEnd(this, e);
			});
		}

		public override unsafe void TransferBuffer(object sender, BufferEventArgs e)
		{
			Runner.AddTask((o) =>
			{
				if (TransferBegin != null)
					TransferBegin(this, e);

				fixed (byte* bData = &e.Buffer.buffer[e.Offset])
				{
					RaspExtern.I2C.bcm2835_i2c_write(bData, (uint)e.Length);
					RaspExtern.I2C.bcm2835_i2c_read(bData, (uint)e.Length);
				}

				if (TransferEnd != null)
					TransferEnd(this, e);
			});
		}

		public override event EventHandler<BufferEventArgs> ReadBegin;
		public override event EventHandler<BufferEventArgs> ReadEnd;
		public override event EventHandler<BufferEventArgs> WriteBegin;
		public override event EventHandler<BufferEventArgs> WriteEnd;
		public override event EventHandler<BufferEventArgs> TransferBegin;
		public override event EventHandler<BufferEventArgs> TransferEnd;
	}
}
