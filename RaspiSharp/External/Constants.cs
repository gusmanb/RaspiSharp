using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{
    public class Constants
    {
        public static byte HIGH = 0x1;
        //This means pin HIGH, true, 3.3volts on a pin.

        public static byte LOW = 0x0;
        //This means pin LOW, false, 0volts on a pin.

        public static uint BCM2835_CORE_CLK_HZ = 250000000;
        //Speed of the core clock core_clk. More...

        public static uint BCM2835_PERI_BASE = 0x20000000;
        //Base Physical Address of the BCM 2835 peripheral registers.

        public static uint BCM2835_ST_BASE = (BCM2835_PERI_BASE + 0x3000);
        //Base Physical Address of the System Timer registers.

        public static uint BCM2835_GPIO_PADS = (BCM2835_PERI_BASE + 0x100000);
        //Base Physical Address of the Pads registers.

        public static uint BCM2835_CLOCK_BASE = (BCM2835_PERI_BASE + 0x101000);
        //Base Physical Address of the Clock/timer registers.

        public static uint BCM2835_GPIO_BASE = (BCM2835_PERI_BASE + 0x200000);
        //Base Physical Address of the GPIO registers.

        public static uint BCM2835_SPI0_BASE = (BCM2835_PERI_BASE + 0x204000);
        //Base Physical Address of the SPI0 registers.

        public static uint BCM2835_BSC0_BASE = (BCM2835_PERI_BASE + 0x205000);
        //Base Physical Address of the BSC0 registers.

        public static uint BCM2835_GPIO_PWM = (BCM2835_PERI_BASE + 0x20C000);
        //Base Physical Address of the PWM registers.

        public static uint BCM2835_BSC1_BASE = (BCM2835_PERI_BASE + 0x804000);
        //Base Physical Address of the BSC1 registers.

        public static uint BCM2835_PAGE_SIZE = (4 * 1024);
        //Size of memory page on RPi.

        public static uint BCM2835_BLOCK_SIZE = (4 * 1024);
        //Size of memory block on RPi.

        public static ushort BCM2835_GPFSEL0 = 0x0000;
        //GPIO register offsets from BCM2835_GPIO_BASE. Offsets into the GPIO Peripheral block in bytes per 6.1 Register View. More...

        public static ushort BCM2835_GPFSEL1 = 0x0004;
        //GPIO Function Select 1.

        public static ushort BCM2835_GPFSEL2 = 0x0008;
        //GPIO Function Select 2.

        public static ushort BCM2835_GPFSEL3 = 0x000c;
        //GPIO Function Select 3.

        public static ushort BCM2835_GPFSEL4 = 0x0010;
        //GPIO Function Select 4.

        public static ushort BCM2835_GPFSEL5 = 0x0014;
        //GPIO Function Select 5.

        public static ushort BCM2835_GPSET0 = 0x001c;
        //GPIO Pin Output Set 0.

        public static ushort BCM2835_GPSET1 = 0x0020;
        //GPIO Pin Output Set 1.

        public static ushort BCM2835_GPCLR0 = 0x0028;
        //GPIO Pin Output Clear 0.

        public static ushort BCM2835_GPCLR1 = 0x002c;
        //GPIO Pin Output Clear 1.

        public static ushort BCM2835_GPLEV0 = 0x0034;
        //GPIO Pin Level 0.

        public static ushort BCM2835_GPLEV1 = 0x0038;
        //GPIO Pin Level 1.

        public static ushort BCM2835_GPEDS0 = 0x0040;
        //GPIO Pin Event Detect Status 0.

        public static ushort BCM2835_GPEDS1 = 0x0044;
        //GPIO Pin Event Detect Status 1.

        public static ushort BCM2835_GPREN0 = 0x004c;
        //GPIO Pin Rising Edge Detect Enable 0.

        public static ushort BCM2835_GPREN1 = 0x0050;
        //GPIO Pin Rising Edge Detect Enable 1.

        public static ushort BCM2835_GPFEN0 = 0x0058;
        //GPIO Pin Falling Edge Detect Enable 0.

        public static ushort BCM2835_GPFEN1 = 0x005c;
        //GPIO Pin Falling Edge Detect Enable 1.

        public static ushort BCM2835_GPHEN0 = 0x0064;
        //GPIO Pin High Detect Enable 0.

        public static ushort BCM2835_GPHEN1 = 0x0068;
        //GPIO Pin High Detect Enable 1.

        public static ushort BCM2835_GPLEN0 = 0x0070;
        //GPIO Pin Low Detect Enable 0.

        public static ushort BCM2835_GPLEN1 = 0x0074;
        //GPIO Pin Low Detect Enable 1.

        public static ushort BCM2835_GPAREN0 = 0x007c;
        //GPIO Pin Async. Rising Edge Detect 0.

        public static ushort BCM2835_GPAREN1 = 0x0080;
        //GPIO Pin Async. Rising Edge Detect 1.

        public static ushort BCM2835_GPAFEN0 = 0x0088;
        //GPIO Pin Async. Falling Edge Detect 0.

        public static ushort BCM2835_GPAFEN1 = 0x008c;
        //GPIO Pin Async. Falling Edge Detect 1.

        public static ushort BCM2835_GPPUD = 0x0094;
        //GPIO Pin Pull-up/down Enable.

        public static ushort BCM2835_GPPUDCLK0 = 0x0098;
        //GPIO Pin Pull-up/down Enable Clock 0.

        public static ushort BCM2835_GPPUDCLK1 = 0x009c;
        //GPIO Pin Pull-up/down Enable Clock 1.

        public static ushort BCM2835_PADS_GPIO_0_27 = 0x002c;
        //Pad control register offsets from BCM2835_GPIO_PADS. More...

        public static ushort BCM2835_PADS_GPIO_28_45 = 0x0030;
        //Pad control register for pads 28 to 45.

        public static ushort BCM2835_PADS_GPIO_46_53 = 0x0034;
        //Pad control register for pads 46 to 53.

        public static uint BCM2835_PAD_PASSWRD = (0x5A << 24);
        //Pad Control masks. More...

        public static byte BCM2835_PAD_SLEW_RATE_UNLIMITED = 0x10;
        //Slew rate unlimited.

        public static byte BCM2835_PAD_HYSTERESIS_ENABLED = 0x08;
        //Hysteresis enabled.

       

        public static ushort BCM2835_SPI0_CS = 0x0000;
        //SPI Master Control and Status.

        public static ushort BCM2835_SPI0_FIFO = 0x0004;
        //SPI Master TX and RX FIFOs.

        public static ushort BCM2835_SPI0_CLK = 0x0008;
        //SPI Master Clock Divider.

        public static ushort BCM2835_SPI0_DLEN = 0x000c;
        //SPI Master Data Length.

        public static ushort BCM2835_SPI0_LTOH = 0x0010;
        //SPI LOSSI mode TOH.

        public static ushort BCM2835_SPI0_DC = 0x0014;
        //SPI DMA DREQ Controls.

        public static uint BCM2835_SPI0_CS_LEN_LONG = 0x02000000;
        //Enable Long data word in Lossi mode if DMA_LEN is set.

        public static uint BCM2835_SPI0_CS_DMA_LEN = 0x01000000;
        //Enable DMA mode in Lossi mode.

        public static uint BCM2835_SPI0_CS_CSPOL2 = 0x00800000;
        //Chip Select 2 Polarity.

        public static uint BCM2835_SPI0_CS_CSPOL1 = 0x00400000;
        //Chip Select 1 Polarity.

        public static uint BCM2835_SPI0_CS_CSPOL0 = 0x00200000;
        //Chip Select 0 Polarity.

        public static uint BCM2835_SPI0_CS_RXF = 0x00100000;
        //RXF - RX FIFO Full.

        public static uint BCM2835_SPI0_CS_RXR = 0x00080000;
        //RXR RX FIFO needs Reading ( full)

        public static uint BCM2835_SPI0_CS_TXD = 0x00040000;
        //TXD TX FIFO can accept Data.

        public static uint BCM2835_SPI0_CS_RXD = 0x00020000;
        //RXD RX FIFO contains Data.

        public static uint BCM2835_SPI0_CS_DONE = 0x00010000;
        //Done transfer Done.

        public static uint BCM2835_SPI0_CS_TE_EN = 0x00008000;
        //Unused.

        public static uint BCM2835_SPI0_CS_LMONO = 0x00004000;
        //Unused.

        public static uint BCM2835_SPI0_CS_LEN = 0x00002000;
        //LEN LoSSI enable.

        public static uint BCM2835_SPI0_CS_REN = 0x00001000;
        //REN Read Enable.

        public static uint BCM2835_SPI0_CS_ADCS = 0x00000800;
        //ADCS Automatically Deassert Chip Select.

        public static uint BCM2835_SPI0_CS_INTR = 0x00000400;
        //INTR Interrupt on RXR.

        public static uint BCM2835_SPI0_CS_INTD = 0x00000200;
        //INTD Interrupt on Done.

        public static uint BCM2835_SPI0_CS_DMAEN = 0x00000100;
        //DMAEN DMA Enable.

        public static uint BCM2835_SPI0_CS_TA = 0x00000080;
        //Transfer Active.

        public static uint BCM2835_SPI0_CS_CSPOL = 0x00000040;
        //Chip Select Polarity.

        public static uint BCM2835_SPI0_CS_CLEAR = 0x00000030;
        //Clear FIFO Clear RX and TX.

        public static uint BCM2835_SPI0_CS_CLEAR_RX = 0x00000020;
        //Clear FIFO Clear RX.

        public static uint BCM2835_SPI0_CS_CLEAR_TX = 0x00000010;
        //Clear FIFO Clear TX.

        public static uint BCM2835_SPI0_CS_CPOL = 0x00000008;
        //Clock Polarity.

        public static uint BCM2835_SPI0_CS_CPHA = 0x00000004;
        //Clock Phase.

        public static uint BCM2835_SPI0_CS_CS = 0x00000003;
        //Chip Select.

        public static ushort BCM2835_BSC_C = 0x0000;
        //BSC Master Control.

        public static ushort BCM2835_BSC_S = 0x0004;
        //BSC Master Status.

        public static ushort BCM2835_BSC_DLEN = 0x0008;
        //BSC Master Data Length.

        public static ushort BCM2835_BSC_A = 0x000c;
        //BSC Master Slave Address.

        public static ushort BCM2835_BSC_FIFO = 0x0010;
        //BSC Master Data FIFO.

        public static ushort BCM2835_BSC_DIV = 0x0014;
        //BSC Master Clock Divider.

        public static ushort BCM2835_BSC_DEL = 0x0018;
        //BSC Master Data Delay.

        public static ushort BCM2835_BSC_CLKT = 0x001c;
        //BSC Master Clock Stretch Timeout.

        public static uint BCM2835_BSC_C_I2CEN = 0x00008000;
        //I2C Enable, 0 = disabled, 1 = enabled.

        public static uint BCM2835_BSC_C_INTR = 0x00000400;
        //Interrupt on RX.

        public static uint BCM2835_BSC_C_INTT = 0x00000200;
        //Interrupt on TX.

        public static uint BCM2835_BSC_C_INTD = 0x00000100;
        //Interrupt on DONE.

        public static uint BCM2835_BSC_C_ST = 0x00000080;
        //Start transfer, 1 = Start a new transfer.

        public static uint BCM2835_BSC_C_CLEAR_1 = 0x00000020;
        //Clear FIFO Clear.

        public static uint BCM2835_BSC_C_CLEAR_2 = 0x00000010;
        //Clear FIFO Clear.

        public static uint BCM2835_BSC_C_READ = 0x00000001;
        //Read transfer.

        public static uint BCM2835_BSC_S_CLKT = 0x00000200;
        //Clock stretch timeout.

        public static uint BCM2835_BSC_S_ERR = 0x00000100;
        //ACK error.

        public static uint BCM2835_BSC_S_RXF = 0x00000080;
        //RXF FIFO full, 0 = FIFO is not full, 1 = FIFO is full.

        public static uint BCM2835_BSC_S_TXE = 0x00000040;
        //TXE FIFO full, 0 = FIFO is not full, 1 = FIFO is full.

        public static uint BCM2835_BSC_S_RXD = 0x00000020;
        //RXD FIFO contains data.

        public static uint BCM2835_BSC_S_TXD = 0x00000010;
        //TXD FIFO can accept data.

        public static uint BCM2835_BSC_S_RXR = 0x00000008;
        //RXR FIFO needs reading (full)

        public static uint BCM2835_BSC_S_TXW = 0x00000004;
        //TXW FIFO needs writing (full)

        public static uint BCM2835_BSC_S_DONE = 0x00000002;
        //Transfer DONE.

        public static uint BCM2835_BSC_S_TA = 0x00000001;
        //Transfer Active.

        public static byte BCM2835_BSC_FIFO_SIZE = 16;
        //BSC FIFO size.

        public static ushort BCM2835_ST_CS = 0x0000;
        //System Timer Control/Status.

        public static ushort BCM2835_ST_CLO = 0x0004;
        //System Timer Counter Lower 32 bits.

        public static ushort BCM2835_ST_CHI = 0x0008;
        //System Timer Counter Upper 32 bits. 
    }
}
