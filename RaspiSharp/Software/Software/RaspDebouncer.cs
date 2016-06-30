using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace RaspiSharp.Software
{
	[RaspElementCategory(Category = "Software")]
	public class RaspDebouncer : RaspElement
	{
		[RaspOutput(OutputType = IOType.Signal)]
		public event EventHandler<SignalEventArgs> Output;
		int dTime;

		[RaspProperty]
		public int DebounceTime
		{
			get { return dTime; }
			set { dTime = value; if (dTimer != null)dTimer.Interval = dTime; }
		}

		Timer dTimer;
		bool currentSignal;

		object locker = new object();

		public RaspDebouncer() 
		{

			dTime = 10;

			dTimer = new Timer(dTime);
			dTimer.Elapsed += dTimer_Elapsed;
		}

		void dTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			lock (locker)
			{
				if (Output != null)
					Output(sender, new SignalEventArgs { Signal = currentSignal });

				dTimer.Enabled = false;
			}
		}

		[RaspInput(InputType = IOType.Signal)]
		public void Input(object sender, SignalEventArgs e)
		{ 
		
			lock(locker)
			{
			
				currentSignal =  e.Signal;
				dTimer.Enabled = true;
			}
		
		}
	}
}
