using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
    [RaspElementCategory(Category = "Transformation")]
    public class RaspByteToInteger : RaspElement
    {
        int outputStatus = 0;
        
        [RaspOutput(OutputType = IOType.Integer)]
        public event EventHandler<IntegerEventArgs> Output;

        [RaspInput(InputType = IOType.Byte)]
        public void Input(object sender, ByteEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                if (e.Value != outputStatus)
                {
                    outputStatus = e.Value;
                    Output(this, new IntegerEventArgs { Value = outputStatus });
                }

            });

        }
    }
}
