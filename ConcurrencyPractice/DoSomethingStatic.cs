using System.Net;

namespace ConcurrencyPractice
{
	public static class DoSomethingStatic
	{
		/// <summary>
		/// 8.3 Downloads the string asynchronous.
		/// </summary>
		/// <param name="httpService">The HTTP service.</param>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static Task<string> DownloadStringAsync( this IMyAsyncHttpService httpService, Uri address)
		{
			var tcs = new TaskCompletionSource<string>();
			httpService.DownloadString(address, (result, exception) =>
			{
				if (exception != null)
					tcs.TrySetException(exception);
				else
					tcs.TrySetResult(result);
			});
			return tcs.Task;
		}

		/// <summary>
		/// 8.2 Gets the r esponse asynchronous.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <returns></returns>
		public static Task<WebResponse>GetREsponseAsync(this WebRequest client)
		{
			return Task<WebResponse>.Factory.FromAsync(client.BeginGetResponse, client.EndGetResponse, null);
		}

		/// <summary>
		/// 8.1 Downloads the string task asynchronous.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static Task<string> DownloadStringTaskAsync(this WebClient client, Uri address)
		{
			var tcs = new TaskCompletionSource<string>();

			//The event handler will complete the task and unregister itself.
			DownloadStringCompletedEventHandler handler = null;
			handler = (_, e) =>
				{
					client.DownloadStringCompleted -= handler;
					if (e.Cancelled)
						tcs.TrySetCanceled();
					else if (e.Error != null)
						tcs.TrySetException(e.Error);
					else
						tcs.TrySetResult(e.Result);
				};

			//register for the event and *then* start the operation
			client.DownloadStringCompleted += handler;
			client.DownloadStringAsync(address);

			return tcs.Task;
		}
	}
}
