using RaspiSharp.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCM2835;
using static BCM2835.BCM2835Managed;

namespace RaspiSharp
{
    public class RaspInterface : IDisposable
    {
        static object locker = new object();

        volatile static int initCount = 0;

        RaspberryModel model;

        RaspGPIO gpio;

        public RaspGPIO GPIO
        {

            get { return gpio; }
        
        }

        RaspSPI spi;

        public RaspSPI SPI { get { return spi; } }

        RaspI2C i2c;

        public RaspI2C I2C { get { return i2c; } }

        RaspPWM pwm;

        public RaspPWM PWM { get { return pwm; } }

        RaspLowLevel peripherals = new RaspLowLevel();

        public RaspLowLevel Peripherals { get { return peripherals; } }

		Dictionary<string, RaspPort> internalPorts = new Dictionary<string, RaspPort>();

		public Dictionary<string, RaspPort> InternalPorts
		{
			get { return internalPorts; }
		}

        public static void Init()
        {
            lock (locker)
            {
                if (initCount == 0)
                {
                    BCM2835Managed.bcm2835_init();
                }

                initCount++;
            }
        }
        
        public static void Deinit()
        {
            lock (locker)
            {
                initCount--;

                if (initCount == 0)
                    BCM2835Managed.bcm2835_close();
            }
        }

        public RaspInterface(RaspberryModel Model)
        {

            Init();
            model = Model;
            gpio = new RaspGPIO(model);
        }

        public void EnableSPI(bcm2835SPIMode DataMode = bcm2835SPIMode.BCM2835_SPI_MODE1,
            bcm2835SPIClockDivider ClockDivider = bcm2835SPIClockDivider.BCM2835_SPI_CLOCK_DIVIDER_256,
            bcm2835SPIChipSelect ChipSelect = bcm2835SPIChipSelect.BCM2835_SPI_CS0,
            bool ChipSelectPolarity = false)
        {

            if (spi != null)
                spi.Dispose();

            spi = new RaspSPI(DataMode, ClockDivider, ChipSelect, ChipSelectPolarity);
        
        }

        public void DisableSPI()
        {
            if (spi == null)
                return;

            spi.Dispose();
            spi = null;
        
        }

        public void EnableI2C(byte SlaveAddress = 0, uint BaudRate = 100000)
        {

            if (i2c != null)
                i2c.Dispose();

            i2c = new RaspI2C(SlaveAddress, BaudRate);

        }

        public void DisableI2C()
        {
            if (i2c == null)
                return;

            i2c.Dispose();
            i2c = null;

        }

        bcm2835FunctionSelect prevFunction;

        public void EnablePWM(bcm2835PWMClockDivider Clock = bcm2835PWMClockDivider.BCM2835_PWM_CLOCK_DIVIDER_2048, 
            uint Range = 65535, uint Data = 32767, bool MarkSpace = false, bool Enabled = false)
        {

            if (pwm != null)
                pwm.Dispose();

            pwm = new RaspPWM(Clock, Range, Data,MarkSpace, Enabled);

        }

        public void DisablePWM()
        {
            if (pwm == null)
                return;

            pwm.Dispose();
            pwm = null;
        }

        public void Wait(long uSecs)
        {

            RaspDelay.uSDelay(uSecs);
        
        }

        public void Dispose()
        {
            DisableSPI();
            DisableI2C();
            DisablePWM();
            Deinit();
        }

    }

    public enum RaspberryModel
    {

        V1,
        V2,
        V2BPlus

    }
}
