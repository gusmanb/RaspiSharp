using System;
using RaspiSharp;
using System.Threading;

namespace GPIOTestConsola
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            RaspInterface pi = new RaspInterface(RaspberryModel.V1);

            var pin = pi.GPIO[RPiGPIOPin.RPI_GPIO_P1_03];

            pin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF;
            pin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;

            for (int buc = 0; buc < 60; buc++)
            {
                pin.Status = true;
                Thread.Sleep(1000);
                pin.Status = false;
                Thread.Sleep(1000);
            }

            Console.WriteLine(pi.Peripherals[Constants.BCM2835_GPIO_BASE]);

            pi.Dispose();
		}
	}
}
