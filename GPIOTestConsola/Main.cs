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

			pin.PullUpDown = PullUpDownControl.Pull_DOWN;
            pin.Function =  GPIOFunctionSelect.Function_OUTP;

            for (int buc = 0; buc < 60; buc++)
            {
                pin.Signal = true;
                Thread.Sleep(1000);
                pin.Signal = false;
                Thread.Sleep(1000);
            }

            Console.WriteLine(pi.Peripherals[Constants.BCM2835_GPIO_BASE]);

            pi.Dispose();
		}
	}
}
