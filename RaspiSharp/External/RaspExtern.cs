using System;
using System.Runtime.InteropServices;

namespace RaspiSharp
{
    public static class RaspExtern
    {
        public static class Management
        {

            [DllImport("libbcm_mod.so", EntryPoint = "bcm2835_init")]
            public static extern int bcm2835_init();

            [DllImport("libbcm_mod.so")]
            public static extern int bcm2835_close();

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_set_debug(byte debug);

        }

        public static class LowLevel
        {

            [DllImport("libbcm_mod.so")]
            public static extern uint bcm2835_peri_read(uint paddr);

            [DllImport("libbcm_mod.so")]
            public static extern uint bcm2835_peri_read_nb(uint paddr);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_peri_write(uint paddr, uint value);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_peri_write_nb(uint paddr, uint value);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_peri_set_bits(uint paddr, uint value, uint mask);
        
        }

        public static class GPIO
        {

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_fsel(RPiGPIOPin pin, GPIOFunctionSelect mode);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_set(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_clr(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_set_multi(uint mask);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_clr_multi(uint mask);

            [DllImport("libbcm_mod.so")]
            public static extern byte bcm2835_gpio_lev(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern byte bcm2835_gpio_eds(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_set_eds(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_ren(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_clr_ren(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_fen(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_clr_fen(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_hen(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_clr_hen(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_len(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_clr_len(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_aren(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_clr_aren(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_afen(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_clr_afen(RPiGPIOPin pin);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_pud(PullUpDownControl pud);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_pudclk(RPiGPIOPin pin, byte on);

            [DllImport("libbcm_mod.so")]
            public static extern uint bcm2835_gpio_pad(byte group);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_set_pad(byte group, uint control);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_delay(ushort millis);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_delayMicroseconds(ulong micros);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_write(RPiGPIOPin pin, byte on);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_write_multi(uint mask, byte on);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_write_mask(uint value, uint mask);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_gpio_set_pud(RPiGPIOPin pin, PullUpDownControl pud);

			public delegate void PinEventDelegate(byte Value);

			[DllImport("libbcm_mod.so")]
			public static extern byte setPinEventDetector(RPiGPIOPin pin, PinEventDelegate callback);

			[DllImport("libbcm_mod.so")]
			public static extern void clearPinEventDetector(RPiGPIOPin pin);

			[DllImport("libbcm_mod.so")]
			public static extern void setDetectEdge(RPiGPIOPin pin, DetectEdge Edge);

			[DllImport("libbcm_mod.so")]
			public static extern byte exportPin(RPiGPIOPin pin);

			[DllImport("libbcm_mod.so")]
			public static extern byte prepareManualEventDetector(RPiGPIOPin pin);

			[DllImport("libbcm_mod.so")]
			public static extern byte manualEventDetect(RPiGPIOPin pin);
        }
        
        public static class SPI
        {

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_spi_begin();

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_spi_end();

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_spi_setBitOrder(SPIBitOrder order);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_spi_setClockDivider(SPIClockDivider divider);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_spi_setDataMode(SPIMode mode);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_spi_chipSelect(ChipSelect cs);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_spi_setChipSelectPolarity(ChipSelect cs, byte active);

            [DllImport("libbcm_mod.so")]
            public static extern byte bcm2835_spi_transfer(byte value);

            [DllImport("libbcm_mod.so")]
            public unsafe static extern void bcm2835_spi_transfernb(byte* tbuf, byte* rbuf, uint len);

            [DllImport("libbcm_mod.so")]
            public unsafe static extern void bcm2835_spi_transfern(byte* buf, uint len);

            [DllImport("libbcm_mod.so")]
            public unsafe static extern void bcm2835_spi_writenb(byte* buf, uint len);

        }

        public static class I2C
        {

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_i2c_begin();

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_i2c_end();

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_i2c_setSlaveAddress(byte addr);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_i2c_setClockDivider(I2CClockDivider divider);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_i2c_set_baudrate(uint baudrate);

            [DllImport("libbcm_mod.so")]
            public unsafe static extern I2CReasonCodes bcm2835_i2c_write(byte* buf, uint len);

            [DllImport("libbcm_mod.so")]
            public unsafe static extern I2CReasonCodes bcm2835_i2c_read(byte* buf, uint len);

            [DllImport("libbcm_mod.so")]
            public unsafe static extern I2CReasonCodes bcm2835_i2c_read_register_rs(byte* regaddr, byte* buf, uint len);

            [DllImport("libbcm_mod.so")]
            public unsafe static extern I2CReasonCodes bcm2835_i2c_write_read_rs(byte* cmds, uint cmds_len, byte* buf, uint buf_len);
        
        }

        public static class PWM
        {

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_pwm_set_clock(bcm2835PWMClockDivider divisor);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_pwm_set_mode(byte channel, bool markspace, bool enabled);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_pwm_set_range(byte channel, uint range);

            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_pwm_set_data(byte channel, uint data);
        
        }

        public static class Timers
        {

            [DllImport("libbcm_mod.so")]
            public static extern ulong bcm2835_st_read ();
            [DllImport("libbcm_mod.so")]
            public static extern void bcm2835_st_delay(ulong offset_micros, ulong micros);
        
        }

		public static class Ports
		{

			[DllImport("libbcm_mod.so")]
			public static extern byte readBitBangByte(RPiGPIOPin DataPin, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);
			[DllImport("libbcm_mod.so")]
			public static unsafe extern void readBitBangBuffer(byte* Buffer, UInt32 Length, RPiGPIOPin DataPin, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);

			[DllImport("libbcm_mod.so")]
			public static extern void writeBitBangByte(byte Data, RPiGPIOPin DataPin, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);
			[DllImport("libbcm_mod.so")]
			public static unsafe extern void writeBitBangBuffer(byte* Buffer, UInt32 Length, RPiGPIOPin DataPin, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);


			[DllImport("libbcm_mod.so")]
			public static extern byte readNibbleByte(RPiGPIOPin DataPin0, RPiGPIOPin DataPin1, RPiGPIOPin DataPin2, RPiGPIOPin DataPin3, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);
			[DllImport("libbcm_mod.so")]
			public static unsafe extern void readNibbleBuffer(byte* Buffer, UInt32 Length, RPiGPIOPin DataPin0, RPiGPIOPin DataPin1, RPiGPIOPin DataPin2, RPiGPIOPin DataPin3, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);

			[DllImport("libbcm_mod.so")]
			public static extern void writeNibbleByte(byte Data, RPiGPIOPin DataPin0, RPiGPIOPin DataPin1, RPiGPIOPin DataPin2, RPiGPIOPin DataPin3, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);
			[DllImport("libbcm_mod.so")]
			public static extern unsafe void writeNibbleBuffer(byte* Buffer, UInt32 Length, RPiGPIOPin DataPin0, RPiGPIOPin DataPin1, RPiGPIOPin DataPin2, RPiGPIOPin DataPin3, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);

			[DllImport("libbcm_mod.so")]
			public static extern byte readOctetByte(RPiGPIOPin[] DataPins, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);
			[DllImport("libbcm_mod.so")]
			public static unsafe extern void readOctetBuffer(byte* Buffer, UInt32 Length, RPiGPIOPin[] DataPins, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);

			[DllImport("libbcm_mod.so")]
			public static extern void writeOctetByte(byte Data, RPiGPIOPin[] DataPins, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);
			[DllImport("libbcm_mod.so")]
			public static unsafe extern void writeOctetBuffer(byte* Buffer, UInt32 Length, RPiGPIOPin[] DataPins, RPiGPIOPin ClockPin, bool Polarity, UInt32 LowDelay, UInt32 HighDelay);
		}
    }

}

