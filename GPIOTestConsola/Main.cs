using System;
using RaspiSharp;
using System.Threading;
using BCM2835;
using static BCM2835.BCM2835Managed;

namespace GPIOTestConsola
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            Console.WriteLine("Abriendo...");

            RaspInterface pi = new RaspInterface(RaspberryModel.V2);

            Console.WriteLine("Dispositivo abierto");

            var pin = pi.GPIO[RPiGPIOPin.RPI_V2_GPIO_P1_18];

            Console.WriteLine("Pin obtenido");

           // pin.PullUpDown = PullUpDownControl.Pull_DOWN;
            pin.Function =  bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;

            Console.WriteLine("Pin configurado");

            for (int buc = 0; buc < 60; buc++)
            {
                pin.Signal = true;
                Console.WriteLine("On");
                Thread.Sleep(1000);
                pin.Signal = false;
                Console.WriteLine("Off");
                Thread.Sleep(1000);
            }

            Console.WriteLine(pi.Peripherals[BCM2835Managed.BCM2835_GPIO_BASE]);

            pi.Dispose();
		}
	}
}
