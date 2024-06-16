using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks.Dataflow;

namespace ConcurrencyPractice
{
	public class Chapter9
	{
		private readonly BlockingCollection<int> _blockingQueue = new BlockingCollection<int>();
		private readonly BlockingCollection<int> _blockingStack = new BlockingCollection<int>(new ConcurrentStack<int>());
		//could be a bag for unordered,unsorted container
		//private readonly BlockingCollection<int> _blockingBag = new BlockingCollection<int>(new ConcurrentBag<int>());
		private Channel<int> queue = Channel.CreateUnbounded<int>();
		private BufferBlock<int> asyncQueue = new BufferBlock<int>();
		private Channel<int> boundedQueue = Channel.CreateBounded<int>(1);
		private BufferBlock<int> bufferBlockQueue = new BufferBlock<int>( new DataflowBlockOptions { BoundedCapacity = 1 });
		private BlockingCollection<int> blockingCollection = new BlockingCollection<int>(boundedCapacity: 1);


		/// <summary>
		/// 9.9 pt4 Blockings the collection throttled producer.
		/// skipping pt3 since it's a Nito method and I'm interested in native .net methods only
		/// </summary>
		public void BlockingCollectionThrottledProducer()
		{
			
			//adds immediately
			blockingCollection.Add(7);

			//waits for 7 to be removed first
			blockingCollection.Add(13);

			blockingCollection.CompleteAdding();
		}


		/// <summary>
		/// 9.9 pt2 Buffers the block throttled producer asynchronous.
		/// skipping pt3 since it's a Nito method and I'm interested in native .net methods only
		/// </summary>
		public async Task BufferBlockThrottledProducerAsync()
		{
			//completes immediately
			await bufferBlockQueue.SendAsync(7);

			//sends after 7 is removed
			await bufferBlockQueue.SendAsync(13);

			bufferBlockQueue.Complete();
		}

		/// <summary>
		/// 9.9 pt1 Channels the bounded throttled producer asynchronous.
		/// </summary>
		public async Task ChannelBoundedThrottledProducerAsync()
		{
			ChannelWriter<int> writer = boundedQueue.Writer;

			//write finished immediately
			await writer.WriteAsync(7);

			//waits for the for 7 to be removed before it enqueues the 13 asynchronously
			await writer.WriteAsync(13);

			writer.Complete();
		}

		/// <summary>
		/// 9.8 pt2 Buffers the block producer asynchronous.
		/// </summary>
		public async Task BufferBlockProducerAsync()
		{
			await asyncQueue.SendAsync(7);
			await asyncQueue.SendAsync(13);

			asyncQueue.Complete();
		}

		/// <summary>
		/// 9.8 pt2 Buffers the block consumer asynchronous.
		/// </summary>
		public async Task BufferBlockConsumerAsync()
		{
			while(await asyncQueue.OutputAvailableAsync())
			{
				Trace.WriteLine(await asyncQueue.ReceiveAsync());
			}
		}

		/// <summary>
		/// 9.8 pt2 multi consumers Buffers the block multi consumer asynchronous.
		/// </summary>
		public async Task BufferBlockMultiConsumerAsync()
		{
			while (true)
			{
				int item;

				try
				{
					item = await asyncQueue.ReceiveAsync();
				}
				catch (InvalidOperationException)
				{
					break;
				}

				Trace.WriteLine(item);
			}
		}

		/// <summary>
		/// pt 9.8 pt1 Channels the producer.
		/// </summary>
		public async Task ChannelProducerAsync()
		{
			ChannelWriter<int> writer = queue.Writer;
			await writer.WriteAsync(7);
			await writer.WriteAsync(13);
			writer.Complete();
		}

		/// <summary>
		/// pt 9.8 pt1 Channels the consumer.
		/// </summary>
		public async Task ChannelConsumerAsync()
		{
			ChannelReader<int> reader = queue.Reader;

			await foreach(var value in reader.ReadAllAsync())
			{
				Trace.WriteLine(value);
			}
		}



		/// <summary>
		/// pt 9.7Blockings the stack producer.
		/// </summary>
		public void BlockingStackProducer()
		{
			//using private blocking collection
			_blockingStack.Add(7);
			_blockingStack.Add(13);
			Thread.Sleep(TimeSpan.FromSeconds(5));
			_blockingStack.Add(21);
			_blockingStack.Add(42);
			_blockingStack.CompleteAdding();
		}

		/// <summary>
		/// pt 9.7 Blockings the stack consumer.
		/// </summary>
		public void BlockingStackConsumer()
		{
			foreach (var item in _blockingStack.GetConsumingEnumerable())
			{
				Trace.WriteLine(item);
			}
		}

		/// <summary>
		/// 9.6 Blockings the queues.
		/// run in Task.Run so that you can see this pair in action
		/// run them both as separate tasks
		/// </summary>
		public void BlockingQueuesProducer()
		{
			//using private blocking collection
			_blockingQueue.Add(7);
			_blockingQueue.Add(13);
			_blockingQueue.Add(21);
			_blockingQueue.Add(42);
			_blockingQueue.CompleteAdding();
		}

		/// <summary>
		/// Blockings the queues consumer.
		/// </summary>
		public void BlockingQueuesConsumer()
		{
			foreach(var item in _blockingQueue.GetConsumingEnumerable())
			{
				Thread.Sleep(TimeSpan.FromSeconds(3));
				Trace.WriteLine(item);
			}
		}
		
		/// <summary>
		/// Concurrents the dictionary95 p1.
		/// </summary>
		public void ConcurrentDictionary95P1()
		{
			var dictionary = new ConcurrentDictionary<int, string>();
			var newValue = dictionary.AddOrUpdate(0, 
				key => "Zero",
				(key, oldValue) => "Zero");

			dictionary[0] = "ZeroAgain";

			var keyExists = dictionary.TryGetValue(0, out var currentValue);
			Trace.WriteLine(currentValue);

			var keyExisted = dictionary.TryRemove(0, out var removedValue);
			Trace.WriteLine(removedValue);

		}

		/// <summary>
		/// Immutables the sorted dictionary94 PT2.
		/// </summary>
		public void ImmutableSortedDictionary94Pt2()
		{
			var sortedDictionary = ImmutableSortedDictionary<int, string>.Empty;
			sortedDictionary = sortedDictionary.Add(10, "Ten");
			sortedDictionary = sortedDictionary.Add(21, "Twenty-One");
			sortedDictionary = sortedDictionary.SetItem(10, "Diez");

			//Displays "10Diez" followed by "21Twenty-One".
			foreach(var item in sortedDictionary)
			{
				Trace.WriteLine(item.Key + item.Value);
			}

			var ten = sortedDictionary[10];
			//ten == "Diez"
			Trace.WriteLine(ten);

			sortedDictionary = sortedDictionary.Remove(21);
			foreach (var item in sortedDictionary)
			{
				Trace.WriteLine(item.Key + item.Value);
			}
		}
		/// <summary>
		/// Immutables the dictionary PT1.
		/// </summary>
		public void ImmutableDictionary94Pt1()
		{
			var dictionary = ImmutableDictionary<int, string>.Empty;

			dictionary = dictionary.Add(10, "Ten");
			dictionary = dictionary.Add(21, "Twenty-One");
			dictionary = dictionary.SetItem(10, "Diez");

			//Displays "10Diez" and "21Twenty-One" in an unpredictable order
			foreach(var item in dictionary)
			{
				Trace.WriteLine(item.Key + item.Value);
			}

			var ten = dictionary[10];
			Trace.WriteLine(ten);
			//ten == "Diez"

			dictionary = dictionary.Remove(21);
			foreach (var item in dictionary)
			{
				Trace.WriteLine(item.Key + item.Value);
			}
		}

		/// <summary>
		/// Immutables the sorted set PT2.
		/// </summary>
		public void ImmutableSortedSet93Pt2()
		{
			var sortedSet = ImmutableSortedSet<int>.Empty;

			sortedSet = sortedSet.Add(13);
			sortedSet = sortedSet.Add(7);

			//Displays "7" and "13" in an unpredictable order
			foreach (var item in sortedSet)
			{
				Trace.WriteLine(item);
			}

			var smallestItem = sortedSet[0];
			Trace.WriteLine(smallestItem);

			sortedSet = sortedSet.Remove(7);
			//what's left
			foreach (var item in sortedSet)
			{
				Trace.WriteLine(item);
			}
		}

		/// <summary>
		/// Immutables the hash set PT1.
		/// </summary>
		public void ImmutableHashSet93Pt1()
		{
			var hashSet = ImmutableHashSet<int>.Empty;

			hashSet = hashSet.Add(13);
			hashSet = hashSet.Add(7);

			//Displays "7" and "13" in an unpredictable order
			foreach(var item in hashSet)
			{
				Trace.WriteLine(item);
			}

			hashSet = hashSet.Remove(7);
		}

		/// <summary>
		/// Immutables the list example PT2.
		/// </summary>
		public void ImmutableListExample92Pt2()
		{
			var list = ImmutableList<int>.Empty;
			list = list.Insert(0, 13);
			list = list.Insert(0, 7);

			foreach(var item in list)
			{
				Trace.WriteLine(item);
			}

			//much slower than foreach
			for(var i = 0; i != list.Count; ++i){
				Trace.WriteLine(list[i]);
			}
		}

		/// <summary>
		/// Immutables the list example PT1.
		/// </summary>
		public void ImmutableListExample92Pt1()
		{
			var list = ImmutableList<int>.Empty;
			list = list.Insert(0, 13);
			list = list.Insert(0, 7);

			//Displays "7" followed by "13".
			foreach(var item in list)
			{
				Trace.WriteLine(item);
			}

			list = list.RemoveAt(1);
		}

		/// <summary>
		/// 9.1 pt3 Queues the exmaple.
		/// </summary>
		public void QueueExample()
		{
			ImmutableQueue<int> queue = ImmutableQueue<int>.Empty;
			queue = queue.Enqueue(13);
			queue = queue.Enqueue(7);

			//Display's "13" followed by "7".
			foreach (var item in queue)
				Trace.WriteLine(item);

			queue = queue.Dequeue(out var nextItem);
			//Display "13"
			Trace.WriteLine(nextItem);
		}

		/// <summary>
		/// 9.1 pt2 Stacks the examplept2.
		/// </summary>
		public void StackExamplePt2()
		{
			ImmutableStack<int> stack = ImmutableStack<int>.Empty;
			stack = stack.Push(13);
			ImmutableStack<int> biggerStack = stack.Push(7);

			//Displays "7" followed by "13"
			foreach (var item in biggerStack)
				Trace.WriteLine(item);

			//Only displays "13".
			foreach (var item in stack)
				Trace.WriteLine(item);
		}

		/// <summary>
		/// 9.1 pt1 Stacks the example.
		/// </summary>
		public void StackExample()
		{
			ImmutableStack<int> stack = ImmutableStack<int>.Empty;
			stack = stack.Push(13);
			stack = stack.Push(7);

			//Displays "7" followed by "13
			foreach (int item in stack)
				Trace.WriteLine(item);

			stack = stack.Pop(out var lastItem);
			//lastItem ==7
			Trace.WriteLine(lastItem);
		}
	}
}
