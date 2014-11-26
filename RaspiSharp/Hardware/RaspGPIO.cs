using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{
    public class RaspGPIO
    {
        Dictionary<RPiGPIOPin, RaspPin> internalPins = new Dictionary<RPiGPIOPin, RaspPin>();
		
        public RaspPin this[RPiGPIOPin Key]
        {
            get { return internalPins[Key]; }
        }

        public uint GetPadsControl(byte Group)
        {

            return RaspExtern.GPIO.bcm2835_gpio_pad(Group);
        
        }

        public void SetPadsControl(byte Group, uint Strength)
        {

            RaspExtern.GPIO.bcm2835_gpio_set_pad(Group, Strength);
        
        }

        public RaspGPIO(RaspberryModel Model)
        {

            switch (Model)
            { 
            
                case RaspberryModel.V1:

                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_03, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_03));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_05, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_05));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_07, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_07));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_08, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_08));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_10, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_10));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_11, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_11));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_12, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_12));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_13, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_13));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_15, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_15));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_16, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_16));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_18, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_18));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_19, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_19));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_21, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_21));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_22, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_22));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_23, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_23));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_24, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_24));
                    internalPins.Add(RPiGPIOPin.RPI_GPIO_P1_26, new RaspPin(RPiGPIOPin.RPI_GPIO_P1_26));
                    break;

                case RaspberryModel.V2:

                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_03, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_03));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_05, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_05));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_07, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_07));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_08, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_08));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_10, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_10));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_11, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_11));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_12, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_12));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_13, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_13));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_15, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_15));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_16, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_16));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_18, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_18));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_19, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_19));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_21, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_21));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_22, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_22));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_23, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_23));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_24, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_24));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_26, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_26));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P5_03, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P5_03));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P5_04, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P5_04));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P5_05, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P5_05));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P5_06, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P5_06));

                    break;

                case RaspberryModel.V2BPlus:

                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_03, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_03));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_05, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_05));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_07, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_07));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_08, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_08));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_10, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_10));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_11, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_11));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_12, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_12));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_13, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_13));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_15, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_15));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_16, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_16));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_18, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_18));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_19, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_19));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_21, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_21));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_22, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_22));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_23, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_23));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_24, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_24));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P1_26, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P1_26));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P5_03, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P5_03));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P5_04, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P5_04));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P5_05, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P5_05));
                    internalPins.Add(RPiGPIOPin.RPI_V2_GPIO_P5_06, new RaspPin(RPiGPIOPin.RPI_V2_GPIO_P5_06));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_03, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_03));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_05, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_05));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_07, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_07));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_08, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_08));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_10, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_10));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_11, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_11));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_12, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_12));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_13, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_13));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_15, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_15));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_16, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_16));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_18, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_18));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_19, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_19));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_21, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_21));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_22, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_22));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_23, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_23));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_24, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_24));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_26, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_26));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_29, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_29));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_31, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_31));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_32, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_32));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_33, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_33));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_35, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_35));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_36, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_36));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_37, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_37));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_38, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_38));
                    internalPins.Add(RPiGPIOPin.RPI_BPLUS_GPIO_J8_40, new RaspPin(RPiGPIOPin.RPI_BPLUS_GPIO_J8_40));

                    break;
            }
        
        }

    }
}
