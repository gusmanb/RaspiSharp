using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{
    public class RaspPort
    {

        List<RaspPin> pinList = new List<RaspPin>();

        PortDirection direction = PortDirection.Unknown;
        public PortDirection Direction
        {

            get { return direction; }
            set 
            {

                direction = value;

                foreach (var pin in pinList)
                {
                    if (direction == PortDirection.Input)
                        pin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT;
                    else if(direction == PortDirection.Output)
                        pin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;
                }
            }
        }

        public RaspPort(IEnumerable<RaspPin> Pins)
        {

            foreach (var pin in Pins)
                pinList.Add(pin);

            if (pinList.Count != 8)
                throw new InvalidOperationException("Pin list must be 8 items length");
        
        }

        public void Write(byte Value)
        {
            bool hasToSet = false;
            uint toSet = 0;
            bool hasToClear = false;
            uint toClear = 0;

            if ((Value & 1) == 1)
            {

                toSet |= (uint)(1 << (int)pinList[0].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[0].currentPin);
                hasToClear = true;
            
            }

            if ((Value & 2) == 2)
            {

                toSet |= (uint)(1 << (int)pinList[1].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[1].currentPin);
                hasToClear = true;

            }

            if ((Value & 4) == 4)
            {

                toSet |= (uint)(1 << (int)pinList[2].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[2].currentPin);
                hasToClear = true;

            }

            if ((Value & 8) == 8)
            {

                toSet |= (uint)(1 << (int)pinList[3].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[3].currentPin);
                hasToClear = true;

            }

            if ((Value & 16) == 16)
            {

                toSet |= (uint)(1 << (int)pinList[4].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[4].currentPin);
                hasToClear = true;

            }

            if ((Value & 32) == 32)
            {

                toSet |= (uint)(1 << (int)pinList[5].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[5].currentPin);
                hasToClear = true;

            }

            if ((Value & 64) == 64)
            {

                toSet |= (uint)(1 << (int)pinList[6].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[6].currentPin);
                hasToClear = true;

            }

            if ((Value & 128) == 128)
            {

                toSet |= (uint)(1 << (int)pinList[7].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[7].currentPin);
                hasToClear = true;

            }

            if (hasToClear)
                RaspExtern.GPIO.bcm2835_gpio_clr_multi(toClear);

            if(hasToSet)
                RaspExtern.GPIO.bcm2835_gpio_set_multi(toSet);

        }

        public byte Read()
        {

            return (byte)((pinList[0].Status ? 1 : 0) |
                (pinList[1].Status ? 2 : 0) |
                (pinList[2].Status ? 4 : 0) |
                (pinList[3].Status ? 8 : 0) |
                (pinList[4].Status ? 16 : 0) |
                (pinList[5].Status ? 32 : 0) |
                (pinList[6].Status ? 64 : 0) |
                (pinList[7].Status ? 128 : 0));
        
        }

        public enum PortDirection
        { 
            Unknown,
            Input,
            Output                    
        }
    }
}
