using System;
using System.Runtime.InteropServices;

namespace RaspiSharp
{
    public static class RaspExtern
    {
        public static class Management
        {

            [DllImport("libbcm2835.so", EntryPoint = "bcm2835_init")]
            public static extern int bcm2835_init();

            [DllImport("libbcm2835.so")]
            public static extern int bcm2835_close();

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_set_debug(byte debug);

        }

        public static class LowLevel
        {

            [DllImport("libbcm2835.so")]
            public static extern uint bcm2835_peri_read(uint paddr);

            [DllImport("libbcm2835.so")]
            public static extern uint bcm2835_peri_read_nb(uint paddr);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_peri_write(uint paddr, uint value);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_peri_write_nb(uint paddr, uint value);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_peri_set_bits(uint paddr, uint value, uint mask);
        
        }

        public static class GPIO
        {

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_fsel(RPiGPIOPin pin, bcm2835FunctionSelect mode);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_set(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_clr(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_set_multi(uint mask);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_clr_multi(uint mask);

            [DllImport("libbcm2835.so")]
            public static extern byte bcm2835_gpio_lev(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern byte bcm2835_gpio_eds(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_set_eds(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_ren(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_clr_ren(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_fen(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_clr_fen(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_hen(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_clr_hen(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_len(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_clr_len(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_aren(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_clr_aren(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_afen(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_clr_afen(RPiGPIOPin pin);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_pud(bcm2835PUDControl pud);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_pudclk(RPiGPIOPin pin, byte on);

            [DllImport("libbcm2835.so")]
            public static extern uint bcm2835_gpio_pad(byte group);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_set_pad(byte group, uint control);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_delay(ushort millis);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_delayMicroseconds(ulong micros);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_write(RPiGPIOPin pin, byte on);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_write_multi(uint mask, byte on);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_write_mask(uint value, uint mask);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_gpio_set_pud(RPiGPIOPin pin, bcm2835PUDControl pud);

        }
        
        public static class SPI
        {

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_spi_begin();

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_spi_end();

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_spi_setBitOrder(bcm2835SPIBitOrder order);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_spi_setClockDivider(bcm2835SPIClockDivider divider);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_spi_setDataMode(bcm2835SPIMode mode);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_spi_chipSelect(bcm2835SPIChipSelect cs);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_spi_setChipSelectPolarity(bcm2835SPIChipSelect cs, byte active);

            [DllImport("libbcm2835.so")]
            public static extern byte bcm2835_spi_transfer(byte value);

            [DllImport("libbcm2835.so")]
            public unsafe static extern void bcm2835_spi_transfernb(byte* tbuf, byte* rbuf, uint len);

            [DllImport("libbcm2835.so")]
            public unsafe static extern void bcm2835_spi_transfern(byte* buf, uint len);

            [DllImport("libbcm2835.so")]
            public unsafe static extern void bcm2835_spi_writenb(byte* buf, uint len);

        }

        public static class I2C
        {

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_i2c_begin();

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_i2c_end();

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_i2c_setSlaveAddress(byte addr);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_i2c_setClockDivider(bcm2835I2CClockDivider divider);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_i2c_set_baudrate(uint baudrate);

            [DllImport("libbcm2835.so")]
            public unsafe static extern bcm2835I2CReasonCodes bcm2835_i2c_write(byte* buf, uint len);

            [DllImport("libbcm2835.so")]
            public unsafe static extern bcm2835I2CReasonCodes bcm2835_i2c_read(byte* buf, uint len);

            [DllImport("libbcm2835.so")]
            public unsafe static extern bcm2835I2CReasonCodes bcm2835_i2c_read_register_rs(byte* regaddr, byte* buf, uint len);

            [DllImport("libbcm2835.so")]
            public unsafe static extern bcm2835I2CReasonCodes bcm2835_i2c_write_read_rs(byte* cmds, uint cmds_len, byte* buf, uint buf_len);
        
        }

        public static class PWM
        {

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_pwm_set_clock(bcm2835PWMClockDivider divisor);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_pwm_set_mode(byte channel, bool markspace, bool enabled);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_pwm_set_range(byte channel, uint range);

            [DllImport("libbcm2835.so")]
            public static extern void bcm2835_pwm_set_data(byte channel, uint data);
        
        }
    }

}

