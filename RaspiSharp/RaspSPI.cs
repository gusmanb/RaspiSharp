using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace RaspiSharp
{
    public class RaspSPI : IDisposable
    {
        IntPtr readBuffer;
        IntPtr writeBuffer;

        bcm2835SPIMode dMode;

        public bcm2835SPIMode DataMode
        {
            get { return dMode; }
            set { dMode = value; RaspExtern.SPI.bcm2835_spi_setDataMode(value); }
        }
        bcm2835SPIBitOrder bOrder;

        public bcm2835SPIBitOrder BitOrder
        {
            get { return bOrder; }
            set { bOrder = value; RaspExtern.SPI.bcm2835_spi_setBitOrder(value); }
        }
        bcm2835SPIClockDivider cDivider;

        public bcm2835SPIClockDivider ClockDivider
        {
            get { return cDivider; }
            set { cDivider = value; RaspExtern.SPI.bcm2835_spi_setClockDivider(value); }
        }
        bcm2835SPIChipSelect cSelect;

        public bcm2835SPIChipSelect ChipSelect
        {
            get { return cSelect; }
            set { cSelect = value; RaspExtern.SPI.bcm2835_spi_chipSelect(value); }
        }
        bool cSelPol;

        public bool ChipSelectPolarity
        {
            get { return cSelPol; }
            set { cSelPol = value; RaspExtern.SPI.bcm2835_spi_setChipSelectPolarity(ChipSelect, (byte)(value ? 1 : 0)); }
        }

        public RaspSPI(bcm2835SPIMode DataMode, bcm2835SPIBitOrder BitOrder, 
            bcm2835SPIClockDivider ClockDivider,bcm2835SPIChipSelect ChipSelect, 
            bool ChipSelectPolarity)
        { 
        
            RaspExtern.SPI.bcm2835_spi_begin();

            this.DataMode = DataMode;
            this.BitOrder = BitOrder;
            this.ClockDivider = ClockDivider;
            this.ChipSelect = ChipSelect;
            this.ChipSelectPolarity = ChipSelectPolarity;

        }

        public byte TransferByte(byte Value)
        {

            return RaspExtern.SPI.bcm2835_spi_transfer(Value);
        
        }

        public unsafe byte[] TransferBuffer(byte[] Data)
        {
            byte[] read = new byte[Data.Length];

            fixed (byte* wData = Data, rData = read)
                RaspExtern.SPI.bcm2835_spi_transfernb(wData, rData, (uint)read.Length);

            return read;

        }

        public unsafe void WriteBuffer(byte[] Data)
        {
            fixed (byte* wData = Data)
                RaspExtern.SPI.bcm2835_spi_writenb(wData, (uint)Data.Length);            
        }

        public unsafe byte[] ReadBuffer(int Length)
        {
            byte[] read = new byte[Length];
            byte[] write = new byte[Length];

            for (int buc = 0; buc < Length; buc++)
                write[buc] = 255;

            fixed (byte* wData = write, rData = read)
                RaspExtern.SPI.bcm2835_spi_transfernb(wData, rData, (uint)read.Length);

            return read;

        }

        public void TransferByteAsync(byte Value, Action<byte> Callback)
        {

            ThreadPool.QueueUserWorkItem((o) => {

                Callback(TransferByte(Value));
            
            });
        
        }

        public void TransferBufferAsync(byte[] Data, Action<byte[]> Callback)
        {

            ThreadPool.QueueUserWorkItem((o) =>
            {

                Callback(TransferBuffer(Data));

            });

        }

        public void WriteBufferAsync(byte[] Data, Action Callback)
        {

            ThreadPool.QueueUserWorkItem((o) =>
            {

                WriteBuffer(Data);
                if (Callback != null)
                    Callback();

            });
        
        }

        public void ReadBufferAsync(int Length, Action<byte[]> Callback)
        {

            ThreadPool.QueueUserWorkItem((o) =>
            {

                Callback(ReadBuffer(Length));

            });

        }

        public void Dispose()
        {
            RaspExtern.SPI.bcm2835_spi_end();
        }
    }
}
