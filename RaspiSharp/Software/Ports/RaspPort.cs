using RaspiSharp.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BCM2835;
using static BCM2835.BCM2835Managed;

namespace RaspiSharp.Software
{
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
    public class RaspBitbangPort : RaspPort
    {

        bitbang_port port = new bitbang_port();
        
        [RaspProperty]
        public RPiGPIOPin DataInputPin
        {
            get { return port.input_pin; }
            set { port.input_pin = value; port.needs_update = true; }
        }
        
        [RaspProperty]
        public RPiGPIOPin DataOutputPin
        {
            get { return port.output_pin; }
            set { port.output_pin = value; port.needs_update = true; }

        }

        [RaspProperty]
        public RPiGPIOPin ClockPin
        {
            get { return port.clock_pin; }
            set { port.clock_pin = value; port.needs_update = true; }
        }
        
        [RaspProperty]
        public bool Polarity
        {
            get { return port.positive_polarity; }
            set { port.positive_polarity = value; }
        }
        
        [RaspProperty]
        public UInt32 LowCycle
        {
            get { return port.low_delay; }
            set { port.low_delay = value; }
        }
        
        [RaspProperty]
        public UInt32 HighCycle
        {
            get { return port.high_delay; }
            set { port.high_delay = value; }
        }

        public override event EventHandler<BufferEventArgs> ReadBegin;
        public override event EventHandler<BufferEventArgs> ReadEnd;
        public override event EventHandler<BufferEventArgs> WriteBegin;
        public override event EventHandler<BufferEventArgs> WriteEnd;
        public override event EventHandler<BufferEventArgs> TransferBegin;
        public override event EventHandler<BufferEventArgs> TransferEnd;

        public override void ReadBuffer(object sender, BufferEventArgs e)
        {

            Runner.AddTask((o) =>
            {
                if (ReadBegin != null)
                    ReadBegin(this, e);

                if (e.Length == 1)
                    e.Buffer.buffer[e.Offset] = BCM2835Managed.GPIOExtras.read_bitbang_byte(port);
                else
                {
                    var segment = new ArraySegment<byte>(e.Buffer.buffer, e.Offset, e.Length);
                    BCM2835Managed.GPIOExtras.read_bitbang_buffer(port, segment);
                }

                if (ReadEnd != null)
                    ReadEnd(this, e);
            });
        }

        public override void WriteBuffer(object sender, BufferEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                if (WriteBegin != null)
                    WriteBegin(this, e);

                if (e.Length == 1)
                    BCM2835Managed.GPIOExtras.write_bitbang_byte(port, e.Buffer.buffer[e.Offset]);
                else
                {
                    var segment = new ArraySegment<byte>(e.Buffer.buffer, e.Offset, e.Length);
                    BCM2835Managed.GPIOExtras.write_bitbang_buffer(port, segment);
                }

                if (WriteEnd != null)
                    WriteEnd(this, e); ;
            });
        }

        public override void TransferBuffer(object sender, BufferEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                if (TransferBegin != null)
                    TransferBegin(this, e);

                if (e.Length == 1)
                {
                    BCM2835Managed.GPIOExtras.write_bitbang_byte(port, e.Buffer.buffer[e.Offset]);
                    e.Buffer.buffer[e.Offset] = BCM2835Managed.GPIOExtras.read_bitbang_byte(port);
                }
                else
                {

                    var segment = new ArraySegment<byte>(e.Buffer.buffer, e.Offset, e.Length);
                    BCM2835Managed.GPIOExtras.write_bitbang_buffer(port, segment);
                    BCM2835Managed.GPIOExtras.read_bitbang_buffer(port, segment);

                }

                if (TransferEnd != null)
                    TransferEnd(this, e);
            });
        }

    }

    [RaspElementCategory(Category = "Ports")]
    public class RaspNibblePort : RaspPort
    {
        nibble_port port = new nibble_port();
        
        [RaspProperty]
        public RPiGPIOPin DB4Pin
        {
            get { return port.db4_pin; }
            set { port.db4_pin = value; port.needs_update = true; }
        }

        [RaspProperty]
        public RPiGPIOPin DB5Pin
        {
            get { return port.db5_pin; }
            set { port.db5_pin = value; port.needs_update = true; }
        }

        [RaspProperty]
        public RPiGPIOPin DB6Pin
        {
            get { return port.db6_pin; }
            set { port.db6_pin = value; port.needs_update = true; }
        }

        [RaspProperty]
        public RPiGPIOPin DB7Pin
        {
            get { return port.db7_pin; }
            set { port.db7_pin = value; port.needs_update = true; }
        }

        [RaspProperty]
        public RPiGPIOPin EPin
        {
            get { return port.e_pin; }
            set { port.e_pin = value; port.needs_update = true; }
        }

        [RaspProperty]
        public RPiGPIOPin RsPin
        {
            get { return port.rs_pin; }
            set { port.rs_pin = value; port.needs_update = true; }
        }

        UInt32 lowCycle;
        [RaspProperty]
        public UInt32 LowCycle
        {
            get { return port.low_delay; }
            set { port.high_delay = value; }
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

        bool rs = false;

        [RaspInput(InputType = IOType.Signal)]
        public void Rs(object sender, SignalEventArgs e)
        {
            rs = e.Signal;
        }

        public override void ReadBuffer(object sender, BufferEventArgs e)
        {

            Runner.AddTask((o) =>
            {
                if (ReadBegin != null)
                    ReadBegin(this, e);

                for (int buc = e.Offset; buc < e.Offset + e.Length; buc++)
                    e.Buffer.buffer[buc] = BCM2835Managed.GPIOExtras.read_nibble_byte(port, rs);

                if (ReadEnd != null)
                    ReadEnd(this, e);
            });
        }

        public override void WriteBuffer(object sender, BufferEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                if (WriteBegin != null)
                    WriteBegin(this, e);

                for (int buc = e.Offset; buc < e.Offset + e.Length; buc++)
                    BCM2835Managed.GPIOExtras.write_nibble_byte(port, rs, e.Buffer.buffer[buc], false);

                if (WriteEnd != null)
                    WriteEnd(this, e);
            });
        }

        [RaspInput(InputType = IOType.Buffer)]
        public void WriteHalfBuffer(object sender, BufferEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                if (WriteBegin != null)
                    WriteBegin(this, e);

                for (int buc = e.Offset; buc < e.Offset + e.Length; buc++)
                    BCM2835Managed.GPIOExtras.write_nibble_byte(port, rs, e.Buffer.buffer[buc], true);

                if (WriteEnd != null)
                    WriteEnd(this, e);
            });
        }

        public override void TransferBuffer(object sender, BufferEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                if (TransferBegin != null)
                    TransferBegin(this, e);

                for (int buc = e.Offset; buc < e.Offset + e.Length; buc++)
                    BCM2835Managed.GPIOExtras.write_nibble_byte(port, rs, e.Buffer.buffer[buc], false);

                for (int buc = e.Offset; buc < e.Offset + e.Length; buc++)
                    e.Buffer.buffer[buc] = BCM2835Managed.GPIOExtras.read_nibble_byte(port, rs);

                if (TransferEnd != null)
                    TransferEnd(this, e);
            });
        }

    }
    
    [RaspElementCategory(Category = "Ports")]
    public class RaspSPIPort : RaspPort
    {

        bcm2835SPIMode dMode;
        [RaspProperty]
        public bcm2835SPIMode DataMode
        {
            get { return dMode; }
            set { dMode = value; BCM2835Managed.bcm2835_spi_setDataMode(value); }
        }

        bcm2835SPIClockDivider cDivider;
        [RaspProperty]
        public bcm2835SPIClockDivider ClockDivider
        {
            get { return cDivider; }
            set { cDivider = value; BCM2835Managed.bcm2835_spi_setClockDivider(value); }
        }

        bcm2835SPIChipSelect cSelect;
        [RaspProperty]
        public bcm2835SPIChipSelect ChipSelect
        {
            get { return cSelect; }
            set { cSelect = value; BCM2835Managed.bcm2835_spi_chipSelect(value); }
        }

        bool cSelPol;
        [RaspProperty]
        public bool ChipSelectPolarity
        {
            get { return cSelPol; }
            set { cSelPol = value; BCM2835Managed.bcm2835_spi_setChipSelectPolarity(ChipSelect, value); }
        }

        public RaspSPIPort()
        {

            BCM2835Managed.bcm2835_spi_begin();

        }

        public override unsafe void ReadBuffer(object sender, BufferEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                if (ReadBegin != null)
                    ReadBegin(this, e);

                e.Buffer.Fill(0xFF, e.Offset, e.Length);

                ArraySegment<byte> seg = new ArraySegment<byte>(e.Buffer.buffer, e.Offset, e.Length);
                BCM2835Managed.bcm2835_spi_transfern(seg);

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

                ArraySegment<byte> seg = new ArraySegment<byte>(e.Buffer.buffer, e.Offset, e.Length);
                BCM2835Managed.bcm2835_spi_writenb(seg);

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

                ArraySegment<byte> seg = new ArraySegment<byte>(e.Buffer.buffer, e.Offset, e.Length);
                BCM2835Managed.bcm2835_spi_transfern(seg);

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
            BCM2835Managed.bcm2835_spi_end();
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
                BCM2835Managed.bcm2835_i2c_setSlaveAddress(value);


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
                BCM2835Managed.bcm2835_i2c_set_baudrate(value);

            }

        }

        public RaspI2CPort()
        {
            BCM2835Managed.bcm2835_i2c_begin(false);
        }

        public override unsafe void ReadBuffer(object sender, BufferEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                if (ReadBegin != null)
                    ReadBegin(this, e);

                ArraySegment<byte> seg = new ArraySegment<byte>(e.Buffer.buffer, e.Offset, e.Length);
                BCM2835Managed.bcm2835_i2c_read(seg);

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

                ArraySegment<byte> seg = new ArraySegment<byte>(e.Buffer.buffer, e.Offset, e.Length);
                BCM2835Managed.bcm2835_i2c_write(seg);

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

                ArraySegment<byte> seg = new ArraySegment<byte>(e.Buffer.buffer, e.Offset, e.Length);

                BCM2835Managed.bcm2835_i2c_write(seg);
                BCM2835Managed.bcm2835_i2c_read(seg);


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
