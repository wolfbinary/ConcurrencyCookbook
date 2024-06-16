using System.Reactive.Linq;

namespace ConcurrencyPractice
{
	public class FailureHttpServiceStub : IHttpService
	{
		public IObservable<string> GetString(string url)
		{
			return Observable.Throw<string>(new HttpRequestException());
		}
	}
}
