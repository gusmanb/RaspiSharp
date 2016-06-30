using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
    [RaspElementCategory(Category = "Transformation")]
    public class RaspSignalToInteger : RaspElement
    {

        int highValue = 0;
        [RaspProperty]
        public int HighValue
        {
            get { return highValue; }
            set { highValue = value; }
        }


        int lowValue = 0;
        [RaspProperty]
        public int LowValue
        {
            get { return lowValue; }
            set { lowValue = value; }
        }

        bool enableHigh = false;
        [RaspProperty]
        public bool EnableHigh
        {
            get { return enableHigh; }
            set { enableHigh = value; }
        }
        bool enableLow = false;
        [RaspProperty]
        public bool EnableLow
        {
            get { return enableLow; }
            set { enableLow = value; }
        }

        [RaspOutput(OutputType = IOType.Integer)]
        public event EventHandler<IntegerEventArgs> Output;

        [RaspInput(InputType = IOType.Signal)]
        public void Input(object sender, SignalEventArgs e)
        {
#if DEBUG
            Console.WriteLine("RaspSignalToByte received input " + e.Signal);
#endif
            if (Output != null)
            {
                if (e.Signal && enableHigh)
                {
#if DEBUG
                    Console.WriteLine("RaspSignalToInteger sending high value " + highValue);
#endif
                    Output(this, new IntegerEventArgs { Value = highValue });
                }
                else if (!e.Signal && enableLow)
                {
#if DEBUG
                    Console.WriteLine("RaspSignalToInteger sending low value " + highValue);
#endif
                    Output(this, new IntegerEventArgs { Value = lowValue });
                }
            }

        }
    }
}
