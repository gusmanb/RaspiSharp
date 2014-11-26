using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RaspiSharp
{
    public class RaspI2C : IDisposable
    {
        byte slaveAddres;
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
        public uint BaudRate
        {

            get { return baudRate; }
            set
            {

                baudRate = value;
                RaspExtern.I2C.bcm2835_i2c_set_baudrate(value);
            
            }
        
        }

        public RaspI2C(byte SlaveAddress, uint BaudRate)
        {

            RaspExtern.I2C.bcm2835_i2c_begin();

            this.SlaveAddress = SlaveAddress;
            this.BaudRate = BaudRate;
        
        }

        public void Dispose()
        {
            RaspExtern.I2C.bcm2835_i2c_end();
        }

        public unsafe byte[] Read(int Length)
        {

            byte[] data = new byte[Length];

            fixed (byte* bData = data)
                RaspExtern.I2C.bcm2835_i2c_read(bData, (uint)Length);

            return data;
        
        }

        public unsafe void Write(byte[] Data)
        {

            fixed (byte* bData = Data)
                RaspExtern.I2C.bcm2835_i2c_write(bData, (uint)Data.Length);
        
        }

        public unsafe byte[] WriteAndRead(byte[] Data, byte LengthToRead)
        {

            byte[] data = new byte[LengthToRead];

            fixed (byte* wData = Data, rData = data)
                RaspExtern.I2C.bcm2835_i2c_write_read_rs(wData, (uint)Data.Length, rData, (uint)LengthToRead);

            return data;
        
        }

    }
}
