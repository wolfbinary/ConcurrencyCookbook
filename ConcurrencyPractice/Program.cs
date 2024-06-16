// See https://aka.ms/new-console-template for more information

using ConcurrencyPractice;

public class Program
{
	public static async Task Main()
	{
		var chapter9 = new Chapter9();

		await Task.Run(async () => { await chapter9.BufferBlockProducerAsync(); });

		await Task.Run(async () => { await chapter9.BufferBlockConsumerAsync(); });

		//await Task.Run(async () => { await chapter9.ChannelProducerAsync(); });

		//await Task.Run(async ()=> { await chapter9.ChannelConsumerAsync(); });

		//await Task.Run(() => { chapter9.BlockingStackProducer(); });

		//await Task.Run(() => { chapter9.BlockingStackConsumer(); });

		//await Task.Run(() => { chapter9.BlockingQueuesProducer(); });

		//await Task.Run(() => { chapter9.BlockingQueuesConsumer(); });
		//chapter9.ConcurrentDictionary95P1();

		//chapter9.ImmutableSortedDictionary94Pt2();

		//chapter9.ImmutableListExamplePt1();
		//chapter9.QueueExample();
		//var result = await DoSomethingStatic.DownloadStringTaskAsync(new System.Net.WebClient(), new Uri("http://www.example.com"));

		//var doSomething = new DoSomething();

		//doSomething.MakeErrorsForEventStream();
		//await doSomething.MakeProgressExampleToCall();

		//var timer = new System.Timers.Timer(interval: 1000) { Enabled = true };

		//await doSomething.PropogateErrorsPt2();

		//await doSomething.PropogateErrors();

		//await doSomething.TplBasic();

		//var numbersToMultiply = new List<int>() { 1, 2, 5, 3, 4 };

		//var result = doSomething.ParallelSumChapter4(numbersToMultiply);
		//Console.WriteLine(result);

		//var results = doSomething.MultiplyBy2Ordered(numbersToMultiply);

		//await foreach(var result in results.ToAsyncEnumerable())
		//{
		//	Console.WriteLine(result);
		//}

		//var nodes = new Node();
		//nodes.Left = new Node();
		//nodes.Right = new Node();
		//nodes.Left.Left = new Node(); 
		//nodes.Left.Right = new Node();
		//nodes.Right.Left = new Node();
		//nodes.Right.Right = new Node();

		//doSomething.ProcessTree(nodes);
		//var doAction = () => { Console.WriteLine("doAction"); };

		//doSomething.DoAction20Times(doAction);

		//var doubles = new double[] { 1.1, 1.2, 2.1, 2.2 };

		//doSomething.ProcessArray(doubles);

		//var ints = new List<int>() {  1, 2, 3, 4 };

		//var result = doSomething.ParallelSum3(ints);
		//var result = doSomething.ParallelSum2(ints);
		//var result = doSomething.ParallelSum(ints);
		//Console.WriteLine(result);

		//var matrices = new List<Matrix>() { new Matrix(false), new Matrix(false), new Matrix(), new Matrix(false) };

		//var result = doSomething.InvertMatrices2(matrices);
		//Console.WriteLine(result);

		//var cts = new CancellationTokenSource(5);
		//var cancellationToken = cts.Token;

		//doSomething.RotateMatrices2(matrices,6, cancellationToken);
		//doSomething.InvertMatrices(matrices);
		//doSomething.RotateMatrices(matrices, 6);

		//await doSomething.ConsumeSequence(doSomething.SlowRangeCancellation());

		//using var client = new HttpClient();

		//await foreach (var result in doSomething.GetValuesPt2Async(client))
		//{
		//	Console.WriteLine(result);
		//}

		//await foreach(var result in doSomething.GetValuesAsync())
		//{
		//	Console.WriteLine(result);
		//}

		//var result = await doSomething.ConsumingValueTaskAsync4();

		//Console.WriteLine(result);

		//await doSomething.ProcessTasksAsync2();
		//await doSomething.ProcessTasksAsync();
		//using (var client = new HttpClient())
		//{
		//	var results = await doSomething.FirstRespondingUrlAsync(client, "https://httpbin.org/get", "https://httpbin.org/get");

		//	Console.WriteLine(results);
		//}

		//await doSomething.ObserveOneExceptionAsync();

		//await doSomething.ObserveAllExceptionsAcync();

		//using (var client = new HttpClient())
		//{
		//	var urls = new List<string>() { "https://httpbin.org/get", "https://httpbin.org/get", "https://httpbin.org/get", "https://httpbin.org/get" };

		//	var results = await doSomething.DownloadAllAsync(client, urls);

		//	Console.WriteLine(results);
		//}
		//await doSomething.CallMyMethodAsync();

		//var mySynchronousImplementation = new MySynchronousImplementation();

		//await mySynchronousImplementation.DoSomethingAsync();

		//var result = await mySynchronousImplementation.GetValueAsync(new CancellationToken(false));

		//var result = mySynchronousImplementation.NotImplementedAsync<int>();
		//await result;
		//await mySynchronousImplementation.DoSomethingAsync();
		//still has to be awaited to get value at the end
		//var result = await mySynchronousImplementation.GetValueAsync();
		//Console.WriteLine(result);

		//var doSomething = new DoSomething();

		//using(var client = new HttpClient())
		//{
		//	var result = await doSomething.DownloadStringWithTimeoutAsync(client, "https://httpbin.org/get");
		//	//var result = await doSomething.DownloadStringWithRetriesAsync(client, "https://httpbin.org/get");
		//	Console.WriteLine(result);
		//}

		//var result = await doSomething.DelayResultAsync<int>(10,new TimeSpan(0,0,10));
		//Console.WriteLine(result);

		//await doSomething.DoSomethingAsync();
	}

}
