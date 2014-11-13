using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{
    public class RaspInterface : IDisposable
    {
        static object locker = new object();

        volatile static int initCount = 0;

        volatile static bool debug = false;

        public bool Debug
        {

            get { return debug; }
            set 
            { 
                debug = value;

                if (debug)
                    RaspExtern.Management.bcm2835_set_debug(1);
                else
                    RaspExtern.Management.bcm2835_set_debug(0);
            }
        }

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

        public static void Init()
        {
            lock (locker)
            {
                if (initCount == 0)
                {
                    if (RaspExtern.Management.bcm2835_init() == 0)
                        throw new Exception("Cannot initialize library");
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
                    RaspExtern.Management.bcm2835_close();
            }
        }

        public RaspInterface(RaspberryModel Model)
        {

            Init();
            model = Model;
            gpio = new RaspGPIO(model);
        }

        public void EnableSPI(bcm2835SPIMode DataMode = bcm2835SPIMode.BCM2835_SPI_MODE1,
            bcm2835SPIBitOrder BitOrder = bcm2835SPIBitOrder.BCM2835_SPI_BIT_ORDER_MSBFIRST,
            bcm2835SPIClockDivider ClockDivider = bcm2835SPIClockDivider.BCM2835_SPI_CLOCK_DIVIDER_256,
            bcm2835SPIChipSelect ChipSelect = bcm2835SPIChipSelect.BCM2835_SPI_CS0,
            bool ChipSelectPolarity = false)
        {

            if (spi != null)
                spi.Dispose();

            spi = new RaspSPI(DataMode, BitOrder, ClockDivider, ChipSelect, ChipSelectPolarity);
        
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

        public void EnablePWM(bcm2835PWMClockDivider Clock = bcm2835PWMClockDivider.BCM2835_PWM_CLOCK_DIVIDER_16384, 
            uint Range = 65535, uint Data = 32767, bool MarkSpace = false, bool Enabled = false)
        {

            if (pwm != null)
                pwm.Dispose();

            var pin = gpio[RPiGPIOPin.RPI_GPIO_P1_12];

            if (pwm != null)
                prevFunction = pin.Function;

            pin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT5;

            pwm = new RaspPWM(Clock, Range, Data,MarkSpace, Enabled);

        }

        public void DisablePWM()
        {
            if (pwm == null)
                return;

            pwm.Dispose();
            pwm = null;

            gpio[RPiGPIOPin.RPI_GPIO_P1_12].Function = prevFunction;
        }

        public void Dispose()
        {
            DisableSPI();
            DisableI2C();
            DisablePWM();
            Deinit();
        }


    }
}
