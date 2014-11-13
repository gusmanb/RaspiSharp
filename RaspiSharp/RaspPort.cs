using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp
{

    public abstract class RaspPort
    {

        public abstract void Write(RaspPortData Data);
        public abstract byte Read();
    
    }
    public class RaspPortData
    {

        public byte Data;
    
    }

    public class RaspSimplePort : RaspPort
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

        public RaspSimplePort(IEnumerable<RaspPin> Pins)
        {

            foreach (var pin in Pins)
                pinList.Add(pin);

            if (pinList.Count != 8)
                throw new InvalidOperationException("Pin list must be 8 items length");

            Direction = PortDirection.Output;
        
        }

        public override void Write(RaspPortData Value)
        {
            if(direction != PortDirection.Output)
                Direction = PortDirection.Output;

            bool hasToSet = false;
            uint toSet = 0;
            bool hasToClear = false;
            uint toClear = 0;

            if ((Value.Data & 1) == 1)
            {

                toSet |= (uint)(1 << (int)pinList[0].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[0].currentPin);
                hasToClear = true;
            
            }

            if ((Value.Data & 2) == 2)
            {

                toSet |= (uint)(1 << (int)pinList[1].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[1].currentPin);
                hasToClear = true;

            }

            if ((Value.Data & 4) == 4)
            {

                toSet |= (uint)(1 << (int)pinList[2].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[2].currentPin);
                hasToClear = true;

            }

            if ((Value.Data & 8) == 8)
            {

                toSet |= (uint)(1 << (int)pinList[3].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[3].currentPin);
                hasToClear = true;

            }

            if ((Value.Data & 16) == 16)
            {

                toSet |= (uint)(1 << (int)pinList[4].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[4].currentPin);
                hasToClear = true;

            }

            if ((Value.Data & 32) == 32)
            {

                toSet |= (uint)(1 << (int)pinList[5].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[5].currentPin);
                hasToClear = true;

            }

            if ((Value.Data & 64) == 64)
            {

                toSet |= (uint)(1 << (int)pinList[6].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[6].currentPin);
                hasToClear = true;

            }

            if ((Value.Data & 128) == 128)
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

        public override byte Read()
        {
            if (direction != PortDirection.Input)
                Direction = PortDirection.Input;

            return  (byte)((pinList[0].Status ? 1 : 0) |
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

    public class Rasp8bLCDPort : RaspPort
    { 
    
        List<RaspPin> pinList = new List<RaspPin>();
        RaspPin ePin;
        RaspPin rwPin;
        RaspPin rsPin;

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

        public Rasp8bLCDPort(IEnumerable<RaspPin> Pins, RaspPin EPin, RaspPin RWPin, RaspPin RSPin)
        {

            foreach (var pin in Pins)
                pinList.Add(pin);

            if (pinList.Count != 8)
                throw new InvalidOperationException("Pin list must be 8 items length");

            if(EPin == null || RSPin == null)
                throw new InvalidOperationException("Enable pin and RegisterSelect pin cannot be null");

            ePin = EPin;
            rwPin = RWPin;
            rsPin = RSPin;

            ePin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;
            ePin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF;
            ePin.Status = false;
            
            if (rwPin != null)
            {
                rwPin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;
                rwPin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF;
                rwPin.Status = false;
            }

            rsPin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;
            rsPin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF;
            RSPin.Status = false;
        
        }

        public override void Write(RaspPortData Value)
        {
            var data = (RaspLCDData)Value;

            if(direction != PortDirection.Output)
                Direction = PortDirection.Output;

            bool hasToSet = false;
            uint toSet = 0;
            bool hasToClear = false;
            uint toClear = 0;

            if ((data.Data & 1) == 1)
            {

                toSet |= (uint)(1 << (int)pinList[0].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[0].currentPin);
                hasToClear = true;
            
            }

            if ((data.Data & 2) == 2)
            {

                toSet |= (uint)(1 << (int)pinList[1].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[1].currentPin);
                hasToClear = true;

            }

            if ((data.Data & 4) == 4)
            {

                toSet |= (uint)(1 << (int)pinList[2].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[2].currentPin);
                hasToClear = true;

            }

            if ((data.Data & 8) == 8)
            {

                toSet |= (uint)(1 << (int)pinList[3].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[3].currentPin);
                hasToClear = true;

            }

            if ((data.Data & 16) == 16)
            {

                toSet |= (uint)(1 << (int)pinList[4].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[4].currentPin);
                hasToClear = true;

            }

            if ((data.Data & 32) == 32)
            {

                toSet |= (uint)(1 << (int)pinList[5].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[5].currentPin);
                hasToClear = true;

            }

            if ((data.Data & 64) == 64)
            {

                toSet |= (uint)(1 << (int)pinList[6].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[6].currentPin);
                hasToClear = true;

            }

            if ((data.Data & 128) == 128)
            {

                toSet |= (uint)(1 << (int)pinList[7].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[7].currentPin);
                hasToClear = true;

            }

            if(ePin.Status != false)
            {
                ePin.Status = false;
                RaspDelay.uSDelay(1);
            }

            if (rwPin != null)
                rwPin.Status = false;

            if (hasToClear)
                RaspExtern.GPIO.bcm2835_gpio_clr_multi(toClear);

            if(hasToSet)
                RaspExtern.GPIO.bcm2835_gpio_set_multi(toSet);
            
            if(rsPin != null)
                rsPin.Status = data.RS;

            ePin.Status = true;
            RaspDelay.uSDelay(50);
            ePin.Status = false;
            RaspDelay.uSDelay(1);
        }

        public override byte Read()
        {
            if (direction != PortDirection.Input)
                Direction = PortDirection.Input;

            if (ePin.Status != true)
            {
                ePin.Status = true;
                RaspDelay.uSDelay(1);
            }

            if (rwPin != null)
                rwPin.Status = false;

            ePin.Status = false;
            RaspDelay.uSDelay(50);
            var data =  (byte)((pinList[0].Status ? 1 : 0) |
                    (pinList[1].Status ? 2 : 0) |
                    (pinList[2].Status ? 4 : 0) |
                    (pinList[3].Status ? 8 : 0) |
                    (pinList[4].Status ? 16 : 0) |
                    (pinList[5].Status ? 32 : 0) |
                    (pinList[6].Status ? 64 : 0) |
                    (pinList[7].Status ? 128 : 0));

            ePin.Status = true;
            RaspDelay.uSDelay(1);
            return data;
        
        }

        public enum PortDirection
        { 
            Unknown,
            Input,
            Output                    
        }
    
    }
    
    public class Rasp4bLCDPort : RaspPort
    {

        List<RaspPin> pinList = new List<RaspPin>();
        RaspPin ePin;
        RaspPin rwPin;
        RaspPin rsPin;

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
                    else if (direction == PortDirection.Output)
                        pin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;
                }
            }
        }

        public Rasp4bLCDPort(IEnumerable<RaspPin> Pins, RaspPin EPin, RaspPin RWPin, RaspPin RSPin)
        {

            foreach (var pin in Pins)
                pinList.Add(pin);

            if (pinList.Count != 4)
                throw new InvalidOperationException("Pin list must be 4 items length");

            if (EPin == null || RSPin == null)
                throw new InvalidOperationException("Enable pin and RegisterSelect pin cannot be null");

            ePin = EPin;
            rwPin = RWPin;
            rsPin = RSPin;

            ePin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;
            ePin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF;
            ePin.Status = false;
            
            if (rwPin != null)
            {
                rwPin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;
                rwPin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF;
                rwPin.Status = false;
            }

            rsPin.Function = bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP;
            rsPin.PullUpDown = bcm2835PUDControl.BCM2835_GPIO_PUD_OFF;
            RSPin.Status = false;

        }

        public override void Write(RaspPortData Value)
        {
            var data = (RaspLCDData)Value;

            WriteNibble((byte)((data.Data >> 4) & 0x0F), true, 1);
            WriteNibble((byte)((data.Data >> 4) & 0x0F), true, 50);
        }

        private void WriteNibble(byte Nibble, bool RS, ulong Delay)
        {
            if (direction != PortDirection.Output)
                Direction = PortDirection.Output;

            bool hasToSet = false;
            uint toSet = 0;
            bool hasToClear = false;
            uint toClear = 0;

            if ((Nibble & 1) == 1)
            {

                toSet |= (uint)(1 << (int)pinList[0].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[0].currentPin);
                hasToClear = true;

            }

            if ((Nibble & 2) == 2)
            {

                toSet |= (uint)(1 << (int)pinList[1].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[1].currentPin);
                hasToClear = true;

            }

            if ((Nibble & 4) == 4)
            {

                toSet |= (uint)(1 << (int)pinList[2].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[2].currentPin);
                hasToClear = true;

            }

            if ((Nibble & 8) == 8)
            {

                toSet |= (uint)(1 << (int)pinList[3].currentPin);
                hasToSet = true;

            }
            else
            {

                toClear |= (uint)(1 << (int)pinList[3].currentPin);
                hasToClear = true;

            }

            if (ePin.Status != false)
            {
                ePin.Status = false;
                RaspDelay.uSDelay(1);
            }

            if (hasToClear)
                RaspExtern.GPIO.bcm2835_gpio_clr_multi(toClear);

            if (hasToSet)
                RaspExtern.GPIO.bcm2835_gpio_set_multi(toSet);

            if (rsPin != null)
                rsPin.Status = RS;

            ePin.Status = true;
            RaspDelay.uSDelay(Delay);
            ePin.Status = false;
            RaspDelay.uSDelay(1);
        }

        public override byte Read()
        {
            return (byte)(((ReadNibble(50) << 4) | ReadNibble(1)) & 0xFF);
        }

        private byte ReadNibble(ulong delay)
        {
            if (direction != PortDirection.Input)
                Direction = PortDirection.Input;

            if (ePin.Status != true)
            {
                ePin.Status = true;
                RaspDelay.uSDelay(1);
            }

            ePin.Status = false;
            RaspDelay.uSDelay(50);
            var data = (byte)((pinList[0].Status ? 1 : 0) |
                    (pinList[1].Status ? 2 : 0) |
                    (pinList[2].Status ? 4 : 0) |
                    (pinList[3].Status ? 8 : 0));

            ePin.Status = true;
            RaspDelay.uSDelay(1);
            return data;

        }

        public enum PortDirection
        {
            Unknown,
            Input,
            Output
        }

    }

    public class RaspLCDData : RaspPortData
    {

        public bool RS;
    
    }
}
