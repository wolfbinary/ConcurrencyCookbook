using ConcurrencyPractice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyTests
{
	public class SuccessHttpServiceStub:IHttpService
	{
		public IScheduler Scheduler { get; set; }
		public TimeSpan Delay { get; set; }

		public IObservable<string> GetString(string url)
		{
			return Observable.Return("stub")
				.Delay(Delay, Scheduler);
		}
	}
}
