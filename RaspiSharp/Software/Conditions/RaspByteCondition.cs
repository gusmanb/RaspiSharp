using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspiSharp.Software
{
    [RaspElementCategory(Category = "Conditions")]
    public class RaspByteFixedCondition : RaspElement
    {

        bool currentOutput = false;

        NumericConditionType condition = NumericConditionType.Equal;

        [RaspProperty]
        public NumericConditionType Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        byte compareValue = 0;

        [RaspProperty]
        public byte CompareValue
        {
            get { return compareValue; }
            set { compareValue = value; }
        }

        [RaspOutput(OutputType = IOType.Signal)]
        public event EventHandler<SignalEventArgs> Output;

        [RaspInput(InputType = IOType.Byte)]
        public void Input(object sender, ByteEventArgs e)
        {
            Runner.AddTask((o) =>
            {

                bool newOutput = false;

                switch (condition)
                {
                    case NumericConditionType.Equal:
                        newOutput = e.Value == compareValue;
                        break;
                    case NumericConditionType.NotEqual:
                        newOutput = e.Value != compareValue;
                        break;
                    case NumericConditionType.Greater:
                        newOutput = e.Value > compareValue;
                        break;
                    case NumericConditionType.Lesser:
                        newOutput = e.Value < compareValue;
                        break;
                    case NumericConditionType.GreaterOrEqual:
                        newOutput = e.Value >= compareValue;
                        break;
                    case NumericConditionType.LesserOrEqual:
                        newOutput = e.Value <= compareValue;
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
    public class RaspByteDynamicCondition : RaspElement
    {

        bool currentOutput = false;

        NumericConditionType condition = NumericConditionType.Equal;

        [RaspProperty]
        public NumericConditionType Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        byte valA = 0;
        byte valB = 0;

        [RaspOutput(OutputType = IOType.Signal)]
        public event EventHandler<SignalEventArgs> Output;

        [RaspInput(InputType = IOType.Byte)]
        public void InputA(object sender, ByteEventArgs e)
        {
            valA = e.Value;
        }

        [RaspInput(InputType = IOType.Byte)]
        public void InputB(object sender, ByteEventArgs e)
        {
            valB = e.Value;
        }

        bool clockPolarity = false;
        [RaspProperty]
        public bool ClockPolarity
        {
            get { return clockPolarity; }
            set { clockPolarity = value; }
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
                        case NumericConditionType.Equal:
                            newOutput = valA == valB;
                            break;
                        case NumericConditionType.NotEqual:
                            newOutput = valA != valB;
                            break;
                        case NumericConditionType.Greater:
                            newOutput = valA > valB;
                            break;
                        case NumericConditionType.Lesser:
                            newOutput = valA < valB;
                            break;
                        case NumericConditionType.GreaterOrEqual:
                            newOutput = valA >= valB;
                            break;
                        case NumericConditionType.LesserOrEqual:
                            newOutput = valA <= valB;
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

                valA = 0;
                valB = 0;

            }

        }
    }

    public enum NumericConditionType
    {
        Equal,
        NotEqual,
        Greater,
        Lesser,
        GreaterOrEqual,
        LesserOrEqual
    }
}
