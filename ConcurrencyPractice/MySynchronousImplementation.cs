using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyPractice
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="ConcurrencyPractice.IMyAsyncInterface" />
	public class MySynchronousImplementation : IMyAsyncInterface
	{

		/// <summary>
		/// 2.2 pt5 Does something asynchronous.
		/// </summary>
		/// <returns></returns>
		public Task DoSomethingAsync()
		{
			try
			{
				DoSomethingSynchronously();
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				return Task.FromException(ex);
			}
		}

		private void DoSomethingSynchronously()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 2.2 pt4 Gets the value asynchronous.
		/// </summary>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		public Task<int> GetValueAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<int>(cancellationToken);
			}
			return Task.FromResult(13);
		}

		/// <summary>
		/// 2.2 pt3 Nots the implemented asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public Task<T> NotImplementedAsync<T>()
		{
			return Task.FromException<T>(new NotImplementedException());
		}

		/// <summary>
		/// 2.2.pt2 Does something asynchronous.
		/// </summary>
		/// <returns></returns>
		public Task GiveMeACompletedTask()
		{
			return Task.CompletedTask;
		}
		/// <summary>
		/// 2.2 pt1 Gets the value asynchronous.
		/// </summary>
		/// <returns></returns>
		public Task<int> GetValueAsync()
		{
			return Task.FromResult(13);
		}
	}
}
