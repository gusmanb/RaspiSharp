using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RaspiSharp
{
	public class TaskRunner : IDisposable
	{

		private object locker = new object();
		private Thread thread;
		private Queue<WaitCallback> tasks = new Queue<WaitCallback>();
		private AutoResetEvent signal;
		private bool end;
		private TaskMode mode = TaskMode.AsynchronousLowPriority;

		public static void RunDelayed(Action Method, int Delay)
		{
			Timer timer = null;
			timer = new Timer((c) =>
			{
				Method.Invoke();
				timer.Dispose();

			}, null, Delay, Timeout.Infinite);

		}

		public TaskRunner(TaskMode Mode)
		{

			mode = Mode;

			if (mode == TaskMode.AsynchronousHighPriority)
			{
				signal = new AutoResetEvent(false);
				thread = new Thread(TaskLoop);
				thread.Start();
			}
		}

		public void AddTask(WaitCallback NewTask)
		{

			switch (mode)
			{ 
			
				case TaskMode.Synchronous:

					NewTask(null);
					break;

				case TaskMode.AsynchronousHighPriority:

					lock (locker)
					{

						tasks.Enqueue(NewTask);

						if (tasks.Count == 1)
							signal.Set();

					}

					break;

				case TaskMode.AsynchronousLowPriority:

					ThreadPool.QueueUserWorkItem(NewTask);
					break;
			
			}
		
		}

		public void TaskLoop()
		{

			WaitCallback currentTask = null;

			while (!end)
			{
				signal.WaitOne();

				while (true)
				{

					lock (locker)
					{

						if (tasks.Count == 0)
							break;

						currentTask = tasks.Dequeue();
					}

					currentTask(this);
				
				}

			}
		}

		public void Dispose()
		{
			end = true;

			if (signal != null)
			{

				signal.Dispose();
				signal = null;
				thread.Abort();
				thread = null;

			}
		}

	}

	public enum TaskMode
	{ 
	
		AsynchronousLowPriority,
		AsynchronousHighPriority,
		Synchronous,
		
	}
}
