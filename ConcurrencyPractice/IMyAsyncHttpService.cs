namespace ConcurrencyPractice
{
	public interface IMyAsyncHttpService
	{
		void DownloadString(Uri address, Action<string, Exception> callback);
	}
}
