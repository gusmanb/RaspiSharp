using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspiSharp.Software
{
    [RaspElementCategory(Category = "Conditions")]
    public class RaspBufferFixedCondition : RaspElement
    {

        bool currentOutput = false;

        BufferConditionType condition = BufferConditionType.Equal;

        [RaspProperty]
        public BufferConditionType Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        RaspBuffer compareValue = new RaspBuffer();

        [RaspProperty]
        public RaspBuffer CompareValue
        {
            get { return compareValue; }
            set { compareValue = value; }
        }

        [RaspOutput(OutputType = IOType.Signal)]
        public event EventHandler<SignalEventArgs> Output;

        [RaspInput(InputType = IOType.Buffer)]
        public void Input(object sender, BufferEventArgs e)
        {
            Runner.AddTask((o) =>
            {

                bool newOutput = false;

                switch (condition)
                {
                    case BufferConditionType.Equal:

                        if (e.Buffer == null && compareValue == null)
                            newOutput = true;
                        else if (e.Buffer == null || compareValue == null)
                            newOutput = false;
                        else
                        {
                            if (e.Length != compareValue.Size)
                                newOutput = false;
                            else
                            {
                                newOutput = true;

                                for (int buc = 0; buc < e.Length; buc++)
                                {
                                    if (e.Buffer.buffer[buc + e.Offset] != compareValue.buffer[buc])
                                    {
                                        newOutput = false;
                                        break;
                                    }
                                }
                            }
                        }
                        break;

                    case BufferConditionType.NotEqual:

                        if (e.Buffer == null && compareValue == null)
                            newOutput = false;
                        else if (e.Buffer == null || compareValue == null)
                            newOutput = true;
                        else
                        {
                            if (e.Length != compareValue.Size)
                                newOutput = false;
                            else
                            {
                                newOutput = false;

                                for (int buc = 0; buc < e.Length; buc++)
                                {
                                    if (e.Buffer.buffer[buc + e.Offset] != compareValue.buffer[buc])
                                    {
                                        newOutput = true;
                                        break;
                                    }
                                }
                            }
                        }

                        break;
                }

                if (newOutput != currentOutput)
                {
                    currentOutput = newOutput;

                    if (Output != null)
                        Output(this, new SignalEventArgs { Signal = currentOutput });
                }
            });
        }
    }

    [RaspElementCategory(Category = "Conditions")]
    public class RaspBufferDynamicCondition : RaspElement
    {

        bool currentOutput = false;

        BufferConditionType condition = BufferConditionType.Equal;

        [RaspProperty]
        public BufferConditionType Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        bool clockPolarity = false;
        [RaspProperty]
        public bool ClockPolarity
        {
            get { return clockPolarity; }
            set { clockPolarity = value; }
        }

        [RaspOutput(OutputType = IOType.Signal)]
        public event EventHandler<SignalEventArgs> Output;


        BufferEventArgs inputA;
        BufferEventArgs inputB;

        [RaspInput(InputType = IOType.Buffer)]
        public void InputA(object sender, BufferEventArgs e)
        {
            inputA = e;
        }

        [RaspInput(InputType = IOType.Buffer)]
        public void InputB(object sender, BufferEventArgs e)
        {
            inputB = e;
        }
        
        [RaspInput(InputType = IOType.Signal)]
        public void Clock(object sender, SignalEventArgs e)
        {
            Runner.AddTask((o) =>
            {
                if (e.Signal == clockPolarity)
                {
                    bool newOutput = false;

                    switch (condition)
                    {
                        case BufferConditionType.Equal:

                            if (inputA == null && inputB == null)
                                newOutput = true;
                            else if (inputA == null || inputB == null)
                                newOutput = false;
                            else
                            {
                                if (inputA.Length != inputB.Length)
                                    newOutput = false;
                                else
                                {
                                    newOutput = true;

                                    for (int buc = 0; buc < inputA.Length; buc++)
                                    {
                                        if (inputA.Buffer.buffer[buc + inputA.Offset] != inputB.Buffer.buffer[buc + inputB.Offset])
                                        {
                                            newOutput = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;

                        case BufferConditionType.NotEqual:

                            if (inputA == null && inputB == null)
                                newOutput = false;
                            else if (inputA == null || inputB == null)
                                newOutput = true;
                            else
                            {
                                if (inputA.Length != inputB.Length)
                                    newOutput = true;
                                else
                                {
                                    newOutput = false;

                                    for (int buc = 0; buc < inputA.Length; buc++)
                                    {
                                        if (inputA.Buffer.buffer[buc + inputA.Offset] != inputB.Buffer.buffer[buc + inputB.Offset])
                                        {
                                            newOutput = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            break;
                    }

                    if (newOutput != currentOutput)
                    {
                        currentOutput = newOutput;

                        if (Output != null)
                            Output(this, new SignalEventArgs { Signal = currentOutput });
                    }
                }
            });
        }

        [RaspInput(InputType = IOType.Signal)]
        public void Reset(object sender, SignalEventArgs e)
        {

            if (e.Signal == clockPolarity)
            {

                inputA = null;
                inputB = null;

            }

        }
    }

    public enum BufferConditionType
    {
        Equal,
        NotEqual
    }
}
