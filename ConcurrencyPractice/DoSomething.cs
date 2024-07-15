using System.Diagnostics;
using System.Net;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using System.Timers;

namespace ConcurrencyPractice
{
	public class DoSomething
	{

		//the event ahndler will >
		/// <summary>
		/// 6.1 pt 4 Makes the errors for event stream.
		/// </summary>
		public void MakeErrorsForEventStream()
		{
			using var client = new WebClient();

			var downloadedStrings = Observable.FromEventPattern(client, nameof(WebClient.DownloadStringCompleted));

			downloadedStrings.Subscribe(data =>
			{
				var eventArgs = (DownloadStringCompletedEventArgs)data.EventArgs;
				if (eventArgs.Error != null)
				{
					//this is what's supposed to fire given RX sees this as an event rather then an error
					Console.WriteLine("onNext: (Error) " + eventArgs.Error);
				}
				else
				{
					Console.WriteLine("OnNext: " + eventArgs.Result);
				}
			},
			ex => { Console.WriteLine("OnError: " + ex.ToString()); },
			() => { Console.WriteLine("OnCompleted"); });

			client.DownloadStringAsync(new Uri("http://invalid.example.com/"));

			Console.ReadLine();
		}

		/// <summary>
		/// 6.1 pt3 Makes the timer run2.
		/// </summary>
		public void MakeTimerRun2()
		{
			var timer = new System.Timers.Timer(interval: 1000) { AutoReset = true, Enabled = true };

			var ticks = Observable.FromEventPattern(timer, nameof(System.Timers.Timer.Elapsed));

			ticks.Subscribe(data =>
			{
				Console.WriteLine("OnNext: " + ((ElapsedEventArgs)data.EventArgs).SignalTime);
			});
			//block so that events have time to fire
			Console.ReadLine();
		}

		/// <summary>
		/// 6.1 pt2 Makes the timer run.
		/// </summary>
		public void MakeTimerRun()
		{
			var timer = new System.Timers.Timer(interval: 1000) { AutoReset = true, Enabled = true };

			var ticks = Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>(
				conversionHandler => (s, a) => conversionHandler(s, a),
				addHandler => timer.Elapsed += addHandler,
				deleteHandler => timer.Elapsed -= deleteHandler);

			//var ticks = Observable.FromEventPattern(timer, nameof(System.Timers.Timer.Elapsed));

			ticks.Subscribe(data =>
			{
				//Console.WriteLine("OnNext: " + ((ElapsedEventArgs)data.EventArgs).SignalTime);
				Console.WriteLine("onNext: " + data.EventArgs.SignalTime);
			});
			//block so that events have time to fire
			Console.ReadLine();
		}
		/// <summary>
		/// 6.1 pt1 Makes the progress example to call.
		/// </summary>
		public async Task MakeProgressExampleToCall()
		{
			//The thing that actually does the progress
			var progressToReport = new Progress<int>(report =>
			{
				//do something to report on
				report++;
			});

			//observe the progress that's happening
			var observeReports = Observable.FromEventPattern<int>(
				handler => progressToReport.ProgressChanged += handler,
				handler => progressToReport.ProgressChanged -= handler);

			//start listening to the observations happening
			observeReports.Subscribe(data => Console.WriteLine("OnNext: " + data.EventArgs));

			//kick everything off now that things are setup to flow
			await Task.Run(() => ReportProgress(progressToReport));
		}

		/// <summary>
		/// Reports the progress.
		/// </summary>
		/// <param name="progress">The progress.</param>
		void ReportProgress(IProgress<int> progress = null)
		{
			for (int i = 0; i < 1000; i++)
			{
				if (progress != null) progress.Report(i / 10);

				Console.WriteLine("ReportProgress");
			}
		}

		/// <summary>
		/// 5.6 Creates my custom block.
		/// </summary>
		/// <returns></returns>
		public IPropagatorBlock<int, int> CreateMyCustomBlock()
		{
			var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
			var addBlock = new TransformBlock<int, int>(item => item + 2);
			var divideBlock = new TransformBlock<int, int>(item => item / 2);

			var flowCompletion = new DataflowLinkOptions { PropagateCompletion = true };
			multiplyBlock.LinkTo(addBlock, flowCompletion);
			addBlock.LinkTo(divideBlock, flowCompletion);

			return DataflowBlock.Encapsulate(multiplyBlock, divideBlock);
		}

		/// <summary>
		/// 5.3 Uns the link example.
		/// </summary>
		public void UnLinkExample()
		{
			var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
			var subtractBlock = new TransformBlock<int, int>(item => item - 2);

			using var link = multiplyBlock.LinkTo(subtractBlock);

			multiplyBlock.Post(1);
			multiplyBlock.Post(2);
		}

		/// <summary>
		/// 5.2 pt2 Propogates the errors PT2.
		/// </summary>
		public async Task PropogateErrorsPt2()
		{
			try
			{
				var multiplyBlock = new TransformBlock<int, int>(item =>
				{
					if (item == 1)
						throw new InvalidOperationException("Hi");
					return item * 2;
				});

				var subtractBlock = new TransformBlock<int, int>(item => item - 2);

				multiplyBlock.LinkTo(subtractBlock,
					new DataflowLinkOptions { PropagateCompletion = true });

				multiplyBlock.Post(1);
				await subtractBlock.Completion;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		/// <summary>
		/// 5.2 pt1 Propogates the errors.
		/// </summary>
		public async Task PropogateErrors()
		{
			try
			{
				var block = new TransformBlock<int, int>(item =>
				{
					Console.WriteLine(item);

					if (item == 1)
					{
						throw new InvalidOperationException("Test");
					}
					var result = item * 2;

					return result;
				});

				block.Post(1);
				await block.Completion;
				block.Post(2);
				await block.Completion;
				block.Post(3);
				await block.Completion;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		/// <summary>
		/// TPLs the basic.
		/// </summary>
		public async Task TplBasic()
		{
			var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
			var subtractBlock = new TransformBlock<int, int>(item => item - 2);

			var options = new DataflowLinkOptions() { PropagateCompletion = true };
			multiplyBlock.LinkTo(subtractBlock, options);

			multiplyBlock.Complete();
			await subtractBlock.Completion;
		}

		/// <summary>
		/// 4.5 pt3 Parallels the sum.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public int ParallelSumChapter4(IEnumerable<int> values)
		{
			return values.AsParallel().Sum();
		}
		
		/// <summary>
		/// 4.5 pt2 Multiplies the by2 order of elements preserved
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public IEnumerable<int> MultiplyBy2Ordered(IEnumerable<int> values)
		{
			return values.AsParallel().AsOrdered().Select(value => value * 2);
		}

		/// <summary>
		/// 4.5 pt1 Multiplies the by2.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public IEnumerable<int> MultiplyBy2(IEnumerable<int> values)
		{
			return values.AsParallel().Select(value => value * 2);
		}


		/// <summary>
		/// 4.5 pt2 Example of ContinuationWith for tasks that get created 
		/// after the parent one and are not linked.
		/// </summary>
		public void TaskOnContinuation()
		{
			Task task = Task.Factory.StartNew(() =>
				Thread.Sleep(TimeSpan.FromSeconds(2)),
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default
			);

			//t is for task
			Task continuation = task.ContinueWith(
				t => Trace.WriteLine("Task is done"),
			CancellationToken.None,
			TaskContinuationOptions.None,
			TaskScheduler.Default);
		}


		/// <summary>
		/// 4.4 Processes the tree.
		/// </summary>
		/// <param name="root">The root.</param>
		public void ProcessTree(Node root)
		{
			var task = Task.Factory.StartNew(() => Traverse(root),
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
			task.Wait();
		}

		void Traverse(Node current)
		{
			DoExpensiveActionOnNode(current);
			if (current.Left != null)
			{
				Console.WriteLine("Left");
				Task.Factory.StartNew(() => Traverse(current.Left),
					CancellationToken.None,
					TaskCreationOptions.AttachedToParent,
					TaskScheduler.Default);
			}
			if (current.Right != null)
			{
				Console.WriteLine("Right");
				Task.Factory.StartNew(() => Traverse(current.Right),
					CancellationToken.None,
					TaskCreationOptions.AttachedToParent,
					TaskScheduler.Default);
			}
		}

		void DoExpensiveActionOnNode(Node current)
		{
			Thread.Sleep(TimeSpan.FromSeconds(2));
			Console.WriteLine("DoExpensiveActionOnNode");
		}
		/// <summary>
		/// 4.3 pt3 Does the action20 times with cancellation.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public void DoAction20TimesWithCancellation(Action action, CancellationToken cancellationToken)
		{
			var actions = Enumerable.Repeat(action, 20).ToArray();
			Parallel.Invoke(new ParallelOptions { CancellationToken = cancellationToken }, actions);
		}

		/// <summary>
		/// 4.3 pt2 Does the action20 times.
		/// </summary>
		/// <param name="action">The action.</param>
		public void DoAction20Times(Action action)
		{
			var actions = Enumerable.Repeat(action, 20).ToArray();

			Parallel.Invoke(actions);
		}

		/// <summary>
		/// 4.3 p1 Processes the array.
		/// </summary>
		/// <param name="array">The array.</param>
		public void ProcessArray(double[] array)
		{
			Parallel.Invoke(
				() => ProcessPartialArray(array, 0, array.Length / 2),
				() => ProcessPartialArray(array, array.Length / 2, array.Length)
			);
		}

		void ProcessPartialArray(double[] array, int begin, int end)
		{
			Console.WriteLine("Doing stuff");
		}
		/// <summary>
		/// 4.2 pt3 Parallels the sum3.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public int ParallelSum3(IEnumerable<int> values)
		{
			return values.AsParallel().Aggregate(seed: 0,
				func: (sum, item) => sum + item);
		}

		/// <summary>
		/// 4.2 pt2 Parallels the sum2.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public int ParallelSum2(IEnumerable<int> values)
		{
			return values.AsParallel().Sum();
		}

		/// <summary>
		/// 4.2 pt1 Parallels the sum.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns></returns>
		public int ParallelSum(IEnumerable<int> values)
		{
			var mutex = new object();
			int result = 0;

			Parallel.ForEach(source: values,
				localInit: () => 0,
				body: (item, state, localValue) => localValue + item,
				localFinally: localValue =>
				{
					lock (mutex)
						result += localValue;
				});
			return result;
		}

		/// <summary>
		/// 4.1 pt4 Inverts the matrices2.
		/// </summary>
		/// <param name="matrices">The matrices.</param>
		/// <returns></returns>
		public int InvertMatrices2(IEnumerable<Matrix> matrices)
		{
			object mutex = new object();
			int nonInvertibleCount = 0;

			Parallel.ForEach(matrices, matrix =>
			{
				if (matrix.IsInvertible)
				{
					matrix.Invert();
				}
				else
				{
					lock (mutex)
					{
						++nonInvertibleCount;
					}
				}
			});
			return nonInvertibleCount;
		}

		/// <summary>
		/// 4.1 pt3 Rotates the matrices2.
		/// </summary>
		/// <param name="matrices">The matrices.</param>
		/// <param name="degrees">The degrees.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		public void RotateMatrices2(IEnumerable<Matrix> matrices, float degrees, CancellationToken cancellationToken)
		{
			Parallel.ForEach(matrices, new ParallelOptions { CancellationToken = cancellationToken },
				matrix => matrix.Rotate(degrees));
		}

		/// <summary>
		/// 4.1 p2 Inverts the matrices.
		/// </summary>
		/// <param name="matrices">The matrices.</param>
		public void InvertMatrices(IEnumerable<Matrix> matrices)
		{
			Parallel.ForEach(matrices, (matrix, state) =>
			{
				if (!matrix.IsInvertible)
				{
					Console.WriteLine("Stop");
					state.Stop();
				}
				else
				{
					Console.WriteLine("Inverted");
					matrix.Invert();
				}
			});
		}

		/// <summary>
		/// 4.1 pt1 Rotates the matrices.
		/// </summary>
		/// <param name="matrices">The matrices.</param>
		/// <param name="degrees">The degrees.</param>
		public void RotateMatrices(IEnumerable<Matrix> matrices, float degrees)
		{
			Parallel.ForEach(matrices, matrix => matrix.Rotate(degrees));
		}

		/// <summary>
		/// 3.4 pt2 Consumes the sequence.
		/// </summary>
		/// <param name="items">The items.</param>
		public async Task ConsumeSequence(IAsyncEnumerable<int> items)
		{
			using var cts = new CancellationTokenSource(2000);
			var cancellationToken = cts.Token;

			await foreach (var result in items.WithCancellation(cancellationToken))
			{
				Console.WriteLine(result);
			}
		}

		/// <summary>
		/// 3.4 pt1 Processes the cancelled slow range asynchronous.
		/// </summary>
		public async Task ProcessCancelledSlowRangeAsync()
		{
			using var cts = new CancellationTokenSource(500);

			var cancellationToken = cts.Token;

			await foreach (var result in SlowRangeCancellation(cancellationToken))
			{
				Console.WriteLine(result);
			}

		}

		public async IAsyncEnumerable<int> SlowRangeCancellation([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			for (int i = 0; i < 10; i++)
			{
				try
				{
					await Task.Delay(i * 100, cancellationToken);
				}
				catch { break; }

				yield return i;
			}
		}

		/// <summary>
		/// 3.3 pt4 Processes the slow range count2.
		/// </summary>
		/// <returns></returns>
		public async Task<int> ProcessSlowRangeCount2Async()
		{
			var count = await SlowRange().CountAwaitAsync(async value =>
			{
				await Task.Delay(10);
				return value % 2 == 0;
			});
			return count;
		}

		/// <summary>
		/// 3.3 pt 3 Processes the slow range count.
		/// </summary>
		/// <returns></returns>
		public async Task<int> ProcessSlowRangeCountAsync()
		{
			var count = await SlowRange().CountAsync(value => value % 2 == 0);
			return count;
		}

		/// <summary>
		/// 3.3 pt 1, 2 Processes the slow range.
		/// You can either use WhereAwait and get an asynchronous stream or
		/// use IAsyncEnumerable<T> as a declare type it returns into to get 
		/// the stream with Where and then interate the same in both ways. 
		/// </summary>
		public async Task ProcessSlowRangeAsync()
		{
			var values = SlowRange().WhereAwait(
				async value =>
				{
					await Task.Delay(100);
					return value % 2 == 0;
				});

			await foreach (int result in values)
			{
				Console.WriteLine(result);
			}
		}

		async IAsyncEnumerable<int> SlowRange()
		{
			for (int i = 0; i < 10; i++)
			{
				await Task.Delay(i * 100);
				yield return i;
			}
		}

		/// <summary>
		/// 3.2 pt1-3 Processes the value asynchronous.
		/// </summary>
		/// <param name="client">The client.</param>
		public async Task ProcessValueAsync(HttpClient client)
		{
			await foreach (var result in GetValuesPt2Async(client))
			{
				Console.WriteLine(result);
			}
		}

		/// <summary>
		/// 3.1 pt2 Gets the values PT2 asynchronous.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <returns></returns>
		public async IAsyncEnumerable<string> GetValuesPt2Async(HttpClient client)
		{
			for (int i = 0; i < 10; i++)
			{
				var result = await client.GetStringAsync("https://httpbin.org/get");
				await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
				yield return result;
			}
		}

		/// <summary>
		/// 3.1 pt1 Gets the values asynchronous.
		/// </summary>
		/// <returns></returns>
		public async IAsyncEnumerable<int> GetValuesAsync()
		{
			await Task.Delay(TimeSpan.FromSeconds(1));
			yield return 10;
			await Task.Delay(TimeSpan.FromSeconds(2));
			yield return 13;
		}

		/// <summary>
		/// 2.11 pt4 Consumings the value task async4.
		/// </summary>
		/// <returns></returns>
		public async Task<int> ConsumingValueTaskAsync4()
		{
			//transform into tasks multiple times
			var task = MethodValueTaskToConsumeAsync().AsTask();
			var task2 = MethodValueTaskToConsumeAsync().AsTask();
			//other concurrent work
			var tasks = await Task.WhenAll(task, task2);

			var total = tasks.Sum();

			return total;
		}

		/// <summary>
		/// 2.11 pt3 Consumings the value task async3.
		/// </summary>
		/// <returns></returns>
		public async Task<int> ConsumingValueTaskAsync3()
		{
			var task = MethodValueTaskToConsumeAsync().AsTask();
			//other concurrent work
			var value = await task;
			var value2 = await task;

			return value + value2;
		}

		/// <summary>
		/// 2.11 pt2 Consumings the value task asynchronous.
		/// </summary>
		/// <returns></returns>
		public async Task<int> ConsumingValueTaskAsync2()
		{
			var value = MethodValueTaskToConsumeAsync();

			return await value;
		}

		/// <summary>
		/// 2.11 pt1 Consumings the value task asynchronous.
		/// </summary>
		/// <returns></returns>
		public async Task<int> ConsumingValueTaskAsync()
		{
			var value = await MethodValueTaskToConsumeAsync();

			return value;
		}

		public ValueTask<int> MethodValueTaskToConsumeAsync()
		{
			return new ValueTask<int>(3);
		}

		/// <summary>
		/// 2.10 pt2 Methods the value task PT2 asynchronous.
		/// </summary>
		/// <returns></returns>
		public ValueTask<int> MethodValueTaskPt2Async()
		{
			if (CanBehaveSynchronously)
			{
				return new ValueTask<int>(3);
			}
			return new ValueTask<int>(SlowMethodAsync());
		}

		Task<int> SlowMethodAsync()
		{
			//Task.Delay(TimeSpan.FromSeconds(2));
			return new Task<int>(() => { return 13; });
		}

		public bool CanBehaveSynchronously { get; set; }
		/// <summary>
		/// 2.10 pt1 Methods the value task asynchronous.
		/// </summary>
		/// <returns></returns>
		public async ValueTask<int> MethodValueTaskAsync()
		{
			await Task.Delay(100);
			return 13;
		}

		/// <summary>
		/// 2.8 pt 1 and 2 Handling Exceptions from async
		/// </summary>
		/// <returns></returns>
		public async Task TestThrowExceptionAsync()
		{
			var task = ThrowExceptionAsync();

			try
			{
				await task;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		async Task ThrowExceptionAsync()
		{
			await Task.Delay(TimeSpan.FromSeconds(5));

			throw new InvalidOperationException();
		}

		public async Task ProcessTasksAsTheyCompleteAsync()
		{
			var taskA = TaskDelay(1);
			var taskB = TaskDelay(3);
			var taskC = TaskDelay(5);

			var tasks = new List<Task<int>>() { taskA, taskB, taskC };

			var taskArray = tasks.Select(async task =>
			{
				var result = await task;

				Console.WriteLine(result);

			}).ToArray();

			await Task.WhenAll(tasks);
		}

		async Task ProcessTask(Task<int> t)
		{
			var result = await t;
			Console.WriteLine(result);
		}

		async Task<int> TaskDelay(int seconds)
		{
			await Task.Delay(TimeSpan.FromSeconds(seconds));
			return await Task.FromResult(seconds);
		}

		/// <summary>
		/// 2.7 pt 1
		/// </summary>
		/// <returns></returns>
		public async Task ResumeWithoutContextASync()
		{
			await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

		}

		/// <summary>
		/// 2.7 pt2
		/// </summary>
		/// <returns></returns>
		public async Task ResumeOnContextAsync()
		{
			await Task.Delay(TimeSpan.FromSeconds(1));

		}
		/// <summary>
		/// 2.6 pt2 Processes the tasks async2.
		/// </summary>
		public async Task ProcessTasksAsync2()
		{
			var taskA = DelayAndReturnAsync(2);
			var taskB = DelayAndReturnAsync(3);
			var taskC = DelayAndReturnAsync(1);
			var tasks = new[] { taskA, taskB, taskC };

			var taskQuery = tasks.Select(async t =>
			{
				var result = await t;
				Trace.WriteLine(result);
			});
			var processingTasks = taskQuery.ToArray();

			await Task.WhenAll(processingTasks);
		}

		/// <summary>
		/// 2.6 pt1 Processes the tasks asynchronous.
		/// </summary>
		public async Task ProcessTasksAsync()
		{
			var taskA = DelayAndReturnAsync(2);
			var taskB = DelayAndReturnAsync(3);
			var taskC = DelayAndReturnAsync(1);
			var tasks = new[] { taskA, taskB, taskC };

			var taskQuery = from t in tasks select AwaitAndProcessAsync(t);
			var processingTasks = taskQuery.ToArray();

			await Task.WhenAll(processingTasks);
		}

		async Task<int> DelayAndReturnAsync(int value)
		{
			await Task.Delay(TimeSpan.FromSeconds(value));
			return value;
		}

		async Task AwaitAndProcessAsync(Task<int> task)
		{
			var result = await task;
			Trace.WriteLine(result);
		}

		/// <summary>
		/// 2.5 Firsts the responding URL asynchronous.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="urlA">The URL a.</param>
		/// <param name="urlB">The URL b.</param>
		/// <returns></returns>
		public async Task<int> FirstRespondingUrlAsync(HttpClient client, string urlA, string urlB)
		{
			var downloadTaskA = client.GetByteArrayAsync(urlA);
			var downloadTaskB = client.GetByteArrayAsync(urlB);

			var completedTask = await Task.WhenAny(downloadTaskA, downloadTaskB);

			var data = await completedTask;
			return data.Length;
		}


		/// <summary>
		/// 2.4 pt2 Observes the one exception asynchronous.
		/// </summary>
		/// <returns></returns>
		public async Task ObserveOneExceptionAsync()
		{
			var task1 = ThrowNotImplementedExceptinAsync();
			var task2 = ThrowInvalidOperationExceptionAsync();

			try
			{
				await Task.WhenAll(task1, task2);
			}
			catch (Exception ex)
			{

			}
		}

		public async Task ObserveAllExceptionsAcync()
		{
			var task1 = ThrowNotImplementedExceptinAsync();
			var task2 = ThrowInvalidOperationExceptionAsync();

			var allTasks = Task.WhenAll(task1, task2);

			try
			{
				await allTasks;
			}
			catch (Exception ex)
			{
				var allExceptions = allTasks.Exception;
			}
		}

		async Task ThrowNotImplementedExceptinAsync()
		{
			throw new NotImplementedException();
		}

		async Task ThrowInvalidOperationExceptionAsync()
		{
			throw new InvalidOperationException();
		}

		/// <summary>
		/// 2.4 pt1 Downloads all asynchronous.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="urls">The urls.</param>
		/// <returns></returns>
		public async Task<string> DownloadAllAsync(HttpClient client, IEnumerable<string> urls)
		{
			var downloads = urls.Select(url => client.GetStringAsync(url));

			var downloadTasks = downloads.ToArray();

			var responses = await Task.WhenAll(downloadTasks);

			return string.Concat(responses);
		}

		/// <summary>
		/// 2.3 Calls my method asynchronous.
		/// </summary>
		public async Task CallMyMethodAsync()
		{
			var progress = new Progress<double>();
			progress.ProgressChanged += (sender, args) =>
			{
				//As percentComplete gets reported back args value changes
				//you'd actually want the done value to changed based on percent complete though
			};

			await MyMethodAsync(progress);
		}

		async Task MyMethodAsync(IProgress<double>? progress = null)
		{
			bool done = false;
			double percentComplete = 0;
			while (!done)
			{
				await Task.Delay(100);
				progress?.Report(percentComplete);
			}
		}

		/// <summary>
		/// 2.1 pt2 Downloads the string with timeout.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="uri">The URI.</param>
		/// <returns></returns>
		public async Task<string?> DownloadStringWithTimeoutAsync(HttpClient client, string uri)
		{
			using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));

			Task<string> downloadTask = client.GetStringAsync(uri);
			Task timeoutTask = Task.Delay(Timeout.Infinite, cts.Token);

			Task completedTask = await Task.WhenAny(downloadTask, timeoutTask);
			if (completedTask == timeoutTask)
			{
				return null;
			}

			return await downloadTask;
		}
		/// <summary>
		/// 2.1 pt1 Downloads the string with retries.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="uri">The URI.</param>
		/// <returns></returns>
		public async Task<string> DownloadStringWithRetriesAsync(HttpClient client, string uri)
		{
			//retry after 1 second, then after 2 seconds, then 4.
			var nextDelay = TimeSpan.FromSeconds(1);

			for (int i = 0; i != 3; ++i)
			{
				try
				{
					return await client.GetStringAsync(uri);
				}
				catch { }

				await Task.Delay(nextDelay);
				nextDelay = nextDelay + nextDelay;
			}

			//try one last time, allowing the error to propagate
			return await client.GetStringAsync(uri);
		}

		/// <summary>
		/// Delays the result.
		/// 2.1
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="result">The result.</param>
		/// <param name="delay">The delay.</param>
		/// <returns></returns>
		public async Task<T> DelayResultAsync<T>(T result, TimeSpan delay)
		{
			await Task.Delay(delay);

			return result;
		}

		public async Task DoSomethingAsync()
		{
			int val = 13;

			//Async wait 1 sec
			await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

			val *= 2;

			//wait 1 sec
			await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);

			Trace.WriteLine(val);
		}
	}
}
