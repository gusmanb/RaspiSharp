using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{
    public enum GPIOFunctionSelect : byte
    {
		Function_INPT = 0, Function_OUTP = 1, Function_ALT0 = 4, Function_ALT1 = 5,
		Function_ALT2 = 6,Function_ALT3 = 7,Function_ALT4 = 3,Function_ALT5 = 2,
		Function_MASK = 7
    }

    public enum PullUpDownControl : byte { Pull_OFF = 0,Pull_DOWN = 1,Pull_UP = 2 }

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

    public enum SPIBitOrder : byte { Order_LSBFIRST = 0, Order_MSBFIRST = 1 }

    public enum SPIMode : byte { MODE0 = 0, MODE1 = 1, MODE2 = 2, MODE3 = 3 }

    public enum ChipSelect : byte { CS0 = 0, CS1 = 1, CS2 = 2, None = 3 }

    public enum SPIClockDivider : ushort
    {
       Divider_65536 = 0,Divider_32768 = 32768,Divider_16384 = 16384,Divider_8192 = 8192,
       Divider_4096 = 4096,Divider_2048 = 2048,Divider_1024 = 1024,Divider_512 = 512,
       Divider_256 = 256,Divider_128 = 128,Divider_64 = 64,Divider_32 = 32,
       Divider_16 = 16,Divider_8 = 8,Divider_4 = 4,Divider_2 = 2,
       Divider_1 = 1
    }

    public enum I2CClockDivider : ushort { BCM2835_I2C_CLOCK_DIVIDER_2500 = 2500,Divider_626 = 626,Divider_150 = 150,Divider_148 = 148 }

    public enum I2CReasonCodes : byte { Reason_OK = 0x00, Reason_ERROR_NACK = 0x01, Reason_ERROR_CLKT = 0x02, Reason_ERROR_DATA = 0x04 }

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

	public enum DetectEdge : byte
	{ 
	
		Off = 0,
		Rising = 1,
		Falling = 2,
		Both = 3
	
	}

}
