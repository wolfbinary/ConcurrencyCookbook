using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyPractice
{
	public interface IMyAsyncInterface
	{
		Task<int> GetValueAsync(CancellationToken cancellationToken);

		Task<T> NotImplementedAsync<T>();

		Task DoSomethingAsync();

		Task<int> GetValueAsync();
	}
}
