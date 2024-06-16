namespace ConcurrencyPractice
{
	public interface IHttpService
	{
		IObservable<string> GetString(string url);
	}
}
