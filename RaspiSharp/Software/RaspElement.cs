using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{
	public abstract class RaspElement
	{
		protected TaskRunner Runner;

		protected RaspElement()
		{

			Runner = new TaskRunner(TaskMode.AsynchronousLowPriority);
		
		}
		TaskMode mode = TaskMode.AsynchronousLowPriority;

		[RaspProperty]
		public virtual TaskMode ExecutionMode
		{

			get { return mode; }
			set { mode = value; Runner.Dispose(); Runner = new TaskRunner(value); }
		
		}
	}
}
