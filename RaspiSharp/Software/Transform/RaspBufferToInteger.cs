using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
    [RaspElementCategory(Category = "Transformation")]
    public class RaspBufferToInteger : RaspElement
    {
        public int offset;
        [RaspProperty]
        public int Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public ByteOrder order;
        [RaspProperty]
        public ByteOrder Order
        {
            get { return order; }
            set { order = value; }
        }

        int outputValue = 0;

        [RaspOutput(OutputType = IOType.Integer)]
        public event EventHandler<IntegerEventArgs> Output;

        [RaspInput(InputType = IOType.Buffer)]
        public void Input(object sender, BufferEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                int count = Math.Min(e.Length, 4);
                int value = 0;
                int pos = 0;

                if (order == ByteOrder.MSB)
                {
                    for (int buc = e.Offset + count - 1; buc > 0; buc--)
                    {
                        value |= e.Buffer.buffer[buc] << (pos * 8);
                        pos++;
                    }
                }
                else
                {
                    for (int buc = e.Offset ; buc < e.Offset + e.Length; buc++)
                    {
                        value |= e.Buffer.buffer[buc] << (pos * 8);
                        pos++;
                    }
                }

                if (outputValue != value)
                {
                    outputValue = value;

                    if (Output != null)
                        Output(this, new IntegerEventArgs { Value = outputValue });
                }
            });
        }
    }

    public enum ByteOrder
    {
        MSB,
        LSB
    }
}
