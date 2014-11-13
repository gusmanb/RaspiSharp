using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{
    public enum bcm2835FunctionSelect : byte
    {
        BCM2835_GPIO_FSEL_INPT = 0, BCM2835_GPIO_FSEL_OUTP = 1, BCM2835_GPIO_FSEL_ALT0 = 4, BCM2835_GPIO_FSEL_ALT1 = 5,
        BCM2835_GPIO_FSEL_ALT2 = 6, BCM2835_GPIO_FSEL_ALT3 = 7, BCM2835_GPIO_FSEL_ALT4 = 3, BCM2835_GPIO_FSEL_ALT5 = 2,
        BCM2835_GPIO_FSEL_MASK = 7
    }

    public enum bcm2835PUDControl : byte { BCM2835_GPIO_PUD_OFF = 0, BCM2835_GPIO_PUD_DOWN = 1, BCM2835_GPIO_PUD_UP = 2 }

    public enum bcm2835PadGroup : byte { BCM2835_PAD_GROUP_GPIO_0_27 = 0, BCM2835_PAD_GROUP_GPIO_28_45 = 1, BCM2835_PAD_GROUP_GPIO_46_53 = 2 }

    public enum RPiGPIOPin : byte
    {
        RPI_GPIO_P1_03 = 0, 
        RPI_GPIO_P1_05 = 1, 
        RPI_GPIO_P1_07 = 4, 
        RPI_GPIO_P1_08 = 14,
        RPI_GPIO_P1_10 = 15, 
        RPI_GPIO_P1_11 = 17, 
        RPI_GPIO_P1_12 = 18, 
        RPI_GPIO_P1_13 = 21,
        RPI_GPIO_P1_15 = 22, 
        RPI_GPIO_P1_16 = 23, 
        RPI_GPIO_P1_18 = 24, 
        RPI_GPIO_P1_19 = 10,
        RPI_GPIO_P1_21 = 9, 
        RPI_GPIO_P1_22 = 25, 
        RPI_GPIO_P1_23 = 11, 
        RPI_GPIO_P1_24 = 8,
        RPI_GPIO_P1_26 = 7, 
        RPI_V2_GPIO_P1_03 = 2, 
        RPI_V2_GPIO_P1_05 = 3, 
        RPI_V2_GPIO_P1_07 = 4,
        RPI_V2_GPIO_P1_08 = 14, 
        RPI_V2_GPIO_P1_10 = 15, 
        RPI_V2_GPIO_P1_11 = 17, 
        RPI_V2_GPIO_P1_12 = 18,
        RPI_V2_GPIO_P1_13 = 27, 
        RPI_V2_GPIO_P1_15 = 22, 
        RPI_V2_GPIO_P1_16 = 23, 
        RPI_V2_GPIO_P1_18 = 24,
        RPI_V2_GPIO_P1_19 = 10, 
        RPI_V2_GPIO_P1_21 = 9, 
        RPI_V2_GPIO_P1_22 = 25, 
        RPI_V2_GPIO_P1_23 = 11,
        RPI_V2_GPIO_P1_24 = 8, 
        RPI_V2_GPIO_P1_26 = 7, 
        RPI_V2_GPIO_P5_03 = 28, 
        RPI_V2_GPIO_P5_04 = 29,
        RPI_V2_GPIO_P5_05 = 30, 
        RPI_V2_GPIO_P5_06 = 31, 
        RPI_BPLUS_GPIO_J8_03 = 2, 
        RPI_BPLUS_GPIO_J8_05 = 3,
        RPI_BPLUS_GPIO_J8_07 = 4, 
        RPI_BPLUS_GPIO_J8_08 = 14, 
        RPI_BPLUS_GPIO_J8_10 = 15, 
        RPI_BPLUS_GPIO_J8_11 = 17,
        RPI_BPLUS_GPIO_J8_12 = 18, 
        RPI_BPLUS_GPIO_J8_13 = 27, 
        RPI_BPLUS_GPIO_J8_15 = 22, 
        RPI_BPLUS_GPIO_J8_16 = 23,
        RPI_BPLUS_GPIO_J8_18 = 24, 
        RPI_BPLUS_GPIO_J8_19 = 10, 
        RPI_BPLUS_GPIO_J8_21 = 9, 
        RPI_BPLUS_GPIO_J8_22 = 25,
        RPI_BPLUS_GPIO_J8_23 = 11, 
        RPI_BPLUS_GPIO_J8_24 = 8, 
        RPI_BPLUS_GPIO_J8_26 = 7, 
        RPI_BPLUS_GPIO_J8_29 = 5,
        RPI_BPLUS_GPIO_J8_31 = 6, 
        RPI_BPLUS_GPIO_J8_32 = 12, 
        RPI_BPLUS_GPIO_J8_33 = 13, 
        RPI_BPLUS_GPIO_J8_35 = 19,
        RPI_BPLUS_GPIO_J8_36 = 16, 
        RPI_BPLUS_GPIO_J8_37 = 26, 
        RPI_BPLUS_GPIO_J8_38 = 20, 
        RPI_BPLUS_GPIO_J8_40 = 21
    }

    public enum bcm2835SPIBitOrder : byte { BCM2835_SPI_BIT_ORDER_LSBFIRST = 0, BCM2835_SPI_BIT_ORDER_MSBFIRST = 1 }

    public enum bcm2835SPIMode : byte { BCM2835_SPI_MODE0 = 0, BCM2835_SPI_MODE1 = 1, BCM2835_SPI_MODE2 = 2, BCM2835_SPI_MODE3 = 3 }

    public enum bcm2835SPIChipSelect : byte { BCM2835_SPI_CS0 = 0, BCM2835_SPI_CS1 = 1, BCM2835_SPI_CS2 = 2, BCM2835_SPI_CS_NONE = 3 }

    public enum bcm2835SPIClockDivider : ushort
    {
        BCM2835_SPI_CLOCK_DIVIDER_65536 = 0, BCM2835_SPI_CLOCK_DIVIDER_32768 = 32768, BCM2835_SPI_CLOCK_DIVIDER_16384 = 16384, BCM2835_SPI_CLOCK_DIVIDER_8192 = 8192,
        BCM2835_SPI_CLOCK_DIVIDER_4096 = 4096, BCM2835_SPI_CLOCK_DIVIDER_2048 = 2048, BCM2835_SPI_CLOCK_DIVIDER_1024 = 1024, BCM2835_SPI_CLOCK_DIVIDER_512 = 512,
        BCM2835_SPI_CLOCK_DIVIDER_256 = 256, BCM2835_SPI_CLOCK_DIVIDER_128 = 128, BCM2835_SPI_CLOCK_DIVIDER_64 = 64, BCM2835_SPI_CLOCK_DIVIDER_32 = 32,
        BCM2835_SPI_CLOCK_DIVIDER_16 = 16, BCM2835_SPI_CLOCK_DIVIDER_8 = 8, BCM2835_SPI_CLOCK_DIVIDER_4 = 4, BCM2835_SPI_CLOCK_DIVIDER_2 = 2,
        BCM2835_SPI_CLOCK_DIVIDER_1 = 1
    }

    public enum bcm2835I2CClockDivider : ushort { BCM2835_I2C_CLOCK_DIVIDER_2500 = 2500, BCM2835_I2C_CLOCK_DIVIDER_626 = 626, BCM2835_I2C_CLOCK_DIVIDER_150 = 150, BCM2835_I2C_CLOCK_DIVIDER_148 = 148 }

    public enum bcm2835I2CReasonCodes : byte { BCM2835_I2C_REASON_OK = 0x00, BCM2835_I2C_REASON_ERROR_NACK = 0x01, BCM2835_I2C_REASON_ERROR_CLKT = 0x02, BCM2835_I2C_REASON_ERROR_DATA = 0x04 }

    public enum bcm2835PWMClockDivider : uint
    {
        BCM2835_PWM_CLOCK_DIVIDER_32768 = 32768,
        BCM2835_PWM_CLOCK_DIVIDER_16384 = 16384,
        BCM2835_PWM_CLOCK_DIVIDER_8192 = 8192,
        BCM2835_PWM_CLOCK_DIVIDER_4096 = 4096,
        BCM2835_PWM_CLOCK_DIVIDER_2048 = 2048,
        BCM2835_PWM_CLOCK_DIVIDER_1024 = 1024,
        BCM2835_PWM_CLOCK_DIVIDER_512 = 512,
        BCM2835_PWM_CLOCK_DIVIDER_256 = 256,
        BCM2835_PWM_CLOCK_DIVIDER_128 = 128,
        BCM2835_PWM_CLOCK_DIVIDER_64 = 64,
        BCM2835_PWM_CLOCK_DIVIDER_32 = 32,
        BCM2835_PWM_CLOCK_DIVIDER_16 = 16,
        BCM2835_PWM_CLOCK_DIVIDER_8 = 8,
        BCM2835_PWM_CLOCK_DIVIDER_4 = 4,
        BCM2835_PWM_CLOCK_DIVIDER_2 = 2,
        BCM2835_PWM_CLOCK_DIVIDER_1 = 1,
    }

    public enum bcm2835PadDrive : byte
    { 
    
        BCM2835_PAD_DRIVE_2mA = 0x00,
        //2mA drive current

        BCM2835_PAD_DRIVE_4mA = 0x01,
        //4mA drive current

        BCM2835_PAD_DRIVE_6mA = 0x02,
        //6mA drive current

        BCM2835_PAD_DRIVE_8mA = 0x03,
        //8mA drive current

        BCM2835_PAD_DRIVE_10mA = 0x04,
        //10mA drive current

        BCM2835_PAD_DRIVE_12mA = 0x05,
        //12mA drive current

        BCM2835_PAD_DRIVE_14mA = 0x06,
        //14mA drive current

        BCM2835_PAD_DRIVE_16mA = 0x07
        //16mA drive current
    
    }

    public enum RaspberryModel
    {

        V1,
        V2,
        V2BPlus

    }

}
