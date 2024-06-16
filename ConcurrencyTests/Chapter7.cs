using ConcurrencyPractice;
using Microsoft.Reactive.Testing;
using System.Reactive.Linq;
using System.Threading.Tasks.Dataflow;

namespace ConcurrencyTests
{
	public class Chapter7
	{

		/// <summary>
		/// 7.6 pt2Mies the timeout class successful get long delay throws timeout exception.
		/// </summary>
		[Fact]
		public void MyTimeoutClass_SuccessfulGetLongDelay_ThrowsTimeoutException()
		{
			var scheduler = new TestScheduler();
			var stub = new SuccessHttpServiceStub()
			{
				Scheduler = scheduler,
				Delay = TimeSpan.FromSeconds(1.5)
			};
			var my = new MyTimeoutClass(stub);
			Exception result = null;

			my.GetStringWithTimeout("http://www.example.com/", scheduler)
				.Subscribe(_ =>
				{
					Assert.Fail("Received value");
				}, 
				ex => { result = ex; });

			scheduler.Start();

			Assert.True(typeof(TimeoutException).IsInstanceOfType(result));
		}

		/// <summary>
		/// 7.6 pt1 Mies the timeout class successful get short delay returns result.
		/// </summary>
		[Fact]
		public async Task MyTimeoutClass_SuccessfulGetShortDelay_ReturnsResult()
		{
			//arrange
			var scheduler = new TestScheduler();
			var stub = new SuccessHttpServiceStub
			{
				Scheduler = scheduler,
				Delay = TimeSpan.FromSeconds(0.5),
			};
			var my = new MyTimeoutClass(stub);
			string result = null;

			//act
			my.GetStringWithTimeout("http://www.example.com/", scheduler)
				.Subscribe(r => { result = r; });

			scheduler.Start();
			//assert
			Assert.Equal("stub", result);
		}

		/// <summary>
		/// 7.5 pt2 Mies the timeout class failed get propagates failure.
		/// </summary>
		[Fact]
		public async Task MyTimeoutClass_FailedGet_PropagatesFailure()
		{
			//Arrange
			var stub = new FailureHttpServiceStub();
			var my = new MyTimeoutClass(stub);

			//Act
			//Assert	
			//no need to do this sort of thing anymore since we have this functionality built in now
			//await ThrowsAsync<HttpRequestException>(async () =>
			//{
			//	await my.GetSTringWithTimeout("http://www.example.com/")
			//	.SingleAsync();
			//});
			await Assert.ThrowsAsync<HttpRequestException>(async () =>
				await my.GetStringWithTimeout("http://www.example.com/").SingleAsync());
		}



		/// <summary>
		/// 7.5 pt1 Mies the timeoutclass successful get return result.
		/// </summary>
		[Fact]
		public async Task MyTimeoutclass_SuccessfulGet_ReturnResult()
		{
			var stub = new SuccessHttpServiceStub();
			var my = new MyTimeoutClass(stub);

			var result = await my.GetStringWithTimeout("http://www.example.com/")
				.SingleAsync();

			Assert.Equal("stub", result);
		}

		/// <summary>
		/// 7.4pt2 Mies the custom block fault discard data and faults.
		/// </summary>
		[Fact]
		public async Task MyCustomBlock_Fault_DiscardDataAndFaults()
		{
			//Arrange
			var doSomething = new DoSomething();
			var myCustomBlock = doSomething.CreateMyCustomBlock();

			//Act
			myCustomBlock.Post(3);
			myCustomBlock.Post(13);
			(myCustomBlock as IDataflowBlock).Fault(new InvalidOperationException());
			//Assert	

			try
			{
				await myCustomBlock.Completion;
			}
			catch (AggregateException ex)
			{
				AssertExceptionIs<InvalidOperationException>(ex.Flatten().InnerException, false);
			}
		}

		private void AssertExceptionIs<TException>(Exception ex, bool allowDerivedTypes = true)
		{
			if (allowDerivedTypes && !(ex is TException))
				Assert.Fail($"Exception is of type {ex.GetType().Name}, but " + $"{typeof(TException).Name} or a derived type was expected.");

			if (!allowDerivedTypes && ex.GetType() != typeof(TException))
				Assert.Fail($"Exception is of type {ex.GetType().Name}, but " + $"{typeof(TException).Name} was expected.");
		}



		/// <summary>
		/// 7.4 pt1
		/// </summary>
		[Fact]
		public async Task MyCustomBlock_AddsOneToDataItems()
		{
			var doSomething = new DoSomething();
			var myCustomBlock = doSomething.CreateMyCustomBlock();

			myCustomBlock.Post(3);
			myCustomBlock.Post(13);
			myCustomBlock.Complete();

			Assert.Equal(4, myCustomBlock.Receive());
			Assert.Equal(14, myCustomBlock.Receive());
			await myCustomBlock.Completion;
		}
	}
}