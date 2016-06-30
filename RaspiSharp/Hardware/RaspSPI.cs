using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using BCM2835;
using static BCM2835.BCM2835Managed;

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
            set { dMode = value; BCM2835Managed.bcm2835_spi_setDataMode(value); }
        }

        bcm2835SPIClockDivider cDivider;
        public bcm2835SPIClockDivider ClockDivider
        {
            get { return cDivider; }
            set { cDivider = value; BCM2835Managed.bcm2835_spi_setClockDivider(value); }
        }

        bcm2835SPIChipSelect cSelect;
        public bcm2835SPIChipSelect ChipSelect
        {
            get { return cSelect; }
            set { cSelect = value; BCM2835Managed.bcm2835_spi_chipSelect(value); }
        }

        bool cSelPol;
        public bool ChipSelectPolarity
        {
            get { return cSelPol; }
            set { cSelPol = value; BCM2835Managed.bcm2835_spi_setChipSelectPolarity(ChipSelect, value); }
        }

        public RaspSPI(bcm2835SPIMode DataMode,
            bcm2835SPIClockDivider ClockDivider,bcm2835SPIChipSelect ChipSelect, 
            bool ChipSelectPolarity)
        {

            BCM2835Managed.bcm2835_spi_begin();

            this.DataMode = DataMode;
            this.ClockDivider = ClockDivider;
            this.ChipSelect = ChipSelect;
            this.ChipSelectPolarity = ChipSelectPolarity;

        }

        public byte TransferByte(byte Value)
        {

            return BCM2835Managed.bcm2835_spi_transfer(Value);
        
        }

        public byte[] TransferBufferPreserve(byte[] Data)
        {
            byte[] read = new byte[Data.Length];

            BCM2835Managed.bcm2835_spi_transfernb(Data, read, Data.Length);

            return read;

        }

        public void TransferBuffer(byte[] Data)
        {
            BCM2835Managed.bcm2835_spi_transfern(Data, Data.Length);
            
        }

        public unsafe void WriteBuffer(byte[] Data)
        {
            BCM2835Managed.bcm2835_spi_writenb(Data, Data.Length);            
        }

        public unsafe byte[] ReadBuffer(int Length)
        {
            byte[] data = new byte[Length];

            for (int buc = 0; buc < Length; buc++)
                data[buc] = 255;

           BCM2835Managed.bcm2835_spi_transfern(data, data.Length);

            return data;

        }

        public void Dispose()
        {
            BCM2835Managed.bcm2835_spi_end();
        }
    }
}
