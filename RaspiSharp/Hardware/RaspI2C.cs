using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BCM2835;
using static BCM2835.BCM2835Managed;

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
                BCM2835Managed.bcm2835_i2c_setSlaveAddress(value);

            
            }
        }

        uint baudRate;
        public uint BaudRate
        {

            get { return baudRate; }
            set
            {

                baudRate = value;
                BCM2835Managed.bcm2835_i2c_set_baudrate(value);
            
            }
        
        }

        public RaspI2C(byte SlaveAddress, uint BaudRate)
        {

            BCM2835Managed.bcm2835_i2c_begin(false);

            this.SlaveAddress = SlaveAddress;
            this.BaudRate = BaudRate;
        
        }

        public void Dispose()
        {
            BCM2835Managed.bcm2835_i2c_end();
        }

        public unsafe byte[] Read(int Length)
        {

            byte[] data = new byte[Length];

            BCM2835Managed.bcm2835_i2c_read(data, Length);

            return data;
        
        }

        public unsafe void Write(byte[] Data)
        {

            BCM2835Managed.bcm2835_i2c_write(Data, Data.Length);
        
        }

        public unsafe byte[] WriteAndRead(byte[] Data, byte LengthToRead)
        {

            byte[] data = new byte[LengthToRead];

            BCM2835Managed.bcm2835_i2c_write_read_rs(Data, Data.Length, data, LengthToRead);

            return data;
        
        }

    }
}
