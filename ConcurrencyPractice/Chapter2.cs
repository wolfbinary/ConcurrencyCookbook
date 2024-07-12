using ConcurrencyPractice;

public class Chapter2
{


    /// <summary>
    /// Recipe 2.1 pt1 Pausing for a period of Time.
    /// Scenerios: Useful for unit testing or retry delays or simple timeouts.
    /// </summary>
    /// <returns></returns>
    public async Task PausingForPeriodOfTimeAsync()
    {
        var doSomething = new DoSomething();
        var result = await doSomething.DelayResultAsync<int>(10, new TimeSpan(0, 0, 10));
        Console.WriteLine(result);

        await doSomething.DoSomethingAsync();
    }

    /// <summary>
    /// Recipe 2.1 pt2 Exponential retries
    /// </summary>
    /// <returns></returns>
    public async Task PausingForRetriesAsync()
    {
        var doSomething = new DoSomething();

        using (var client = new HttpClient())
        {
            var result = await doSomething.DownloadStringWithRetriesAsync(client, "https://httpbin.org/get");
            Console.WriteLine(result);
        }
    }


    /// <summary>
    /// Recipe 2.1 pt3 pausing for timeout
    /// Signals to cancel an operation after a period of time
    /// </summary>
    /// <returns></returns>
    public async Task PausingForTimeoutAsync()
    {
        var doSomething = new DoSomething();

        using (var client = new HttpClient())
        {
            var result = await doSomething.DownloadStringWithTimeoutAsync(client, "https://httpbin.org/get");
            Console.WriteLine(result);
        }
    }

    /// <summary>
    /// Recipe 2.2 pt1 Returning completed tasks
    /// For when an asynchronous signure needs to be implemented synchronously for whatever reason.
    /// ie: You have an async interface, but want to implement it synchronously
    /// </summary>
    public async Task ReturningCompletedTasksAsync()
    {
        var mySynchronousImplementation = new MySynchronousImplementation();

        //still has to be awaited to get value at the end
        var result = await mySynchronousImplementation.GetValueAsync();
        Console.WriteLine(result);
    }

    /// <summary>
    /// Recipe 2.2 pt2 Return a task that is already completed .
    /// ie: when you have methods that don't have return types
    /// </summary>
    /// <returns></returns>
    public Task GiveMeACompletedTaskAsync()
    {
        var mySynchronousImplementation = new MySynchronousImplementation();

        //still has to be awaited to get value at the end
        return mySynchronousImplementation.GiveMeACompletedTask();
    }

    /// <summary>
    /// Recipe 2.2 pt3 Returning completed exception from task
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> NotImplementedAsync<T>()
    {
        var mySynchronousImplementation = new MySynchronousImplementation();

        var result = mySynchronousImplementation.NotImplementedAsync<T>();
        return await result;
    }

    /// <summary>
    /// Recipe 2.2 pt4 Creating a task from an already cancelled task.
    /// Creates a task from a CancellationToken that's already been cancelled.
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetValueAsync()
    {
        var mySynchronousImplementation = new MySynchronousImplementation();

        var result = await mySynchronousImplementation.GetValueAsync(new CancellationToken(false));
        return result;
    }

    /// <summary>
    /// Recipe 2.2 pt5 Handling syncronous implmentations in async method
    /// </summary>
    /// <returns></returns>
    public async Task CaptureSyncronousErrorAsync()
    {
        var mySynchronousImplementation = new MySynchronousImplementation();

        await mySynchronousImplementation.DoSomethingAsync();
    }

    /// <summary>
    /// Recipe 2.3 pt1 Reporting Progress
    /// ie: You need to do something as progress on an operation occurs
    /// </summary>
    /// <returns></returns>
    public async Task ReportProgressAsync()
    {
        var doSomething = new DoSomething();

        await doSomething.CallMyMethodAsync();
    }

    /// <summary>
    /// Recipe 2.4 pt1 Create a bunch of tasks then start them at the end.
    /// Waiting for a set of tasks to complete, with no exceptions
    /// </summary>
    /// <returns></returns>
    public async Task WaitForAllTasksAfterStartingAtSameTimeAsync()
    {
        var doSomething = new DoSomething();

        using (var client = new HttpClient())
        {
            var urls = new List<string>() { "https://httpbin.org/get", "https://httpbin.org/get", "https://httpbin.org/get", "https://httpbin.org/get" };

            var results = await doSomething.DownloadAllAsync(client, urls);

            Console.WriteLine(results);
        }
    }

    /// <summary>
    /// Recipe 2.4 pt2 Create a bunch of tasks then start them at the end.
    /// ie: like pt1 only with one exception
    /// </summary>
    /// <returns></returns>
    public async Task WaitForAllTasksAfterStartingAtSameTimeWithExceptionAsync()
    {
        var doSomething = new DoSomething();

        await doSomething.ObserveOneExceptionAsync();
    }

    /// <summary>
    /// Recipe 2.4 pt3 Create a bunch of tasks then start them at the end.
    /// ie: like pt2 only with multiple exceptions that are aggregated
    /// </summary>
    /// <returns></returns>
    public async Task WaitForAllTasksAfterStartingAtSameTimeWithAggregateExceptionsAsync()
    {
        var doSomething = new DoSomething();
        await doSomething.ObserveAllExceptionsAcync();
    }


    /// <summary>
    /// Recipe 2.5 pt1 Waiting for Any Task to Complete
    /// ie: several tasks, but only need to respond to one of them that's completed
    /// </summary>
    /// <returns></returns>
    public async Task WaitForAnyTaskToCompleteAsync()
    {
        var doSomething = new DoSomething();

        using (var client = new HttpClient())
        {
            var results = await doSomething.FirstRespondingUrlAsync(client, "https://httpbin.org/get", "https://httpbin.org/get");

            Console.WriteLine(results);
        }
    }

    /// <summary>
    /// Recipe 2.6 pt1 Process tasks as they complete
    /// As each task completes you want to independently do something with each of them.
    /// Uses linq to objects and external function
    /// </summary>
    /// <returns></returns>
    public async Task ProcessTasksAsTheyCompleteAsync()
    {
        var doSomething = new DoSomething();

        await doSomething.ProcessTasksAsync();
    }

    /// <summary>
    /// Recipe 2.6 pt2 Process tasks as they complete
    /// Uses function based linq query and anonomous function
    /// </summary>
    /// <returns></returns>
    public async Task ProcessTasksAsTheyCompleteAsync2()
    {
        var doSomething = new DoSomething();

        await doSomething.ProcessTasksAsync2();
    }

    /// <summary>
    /// Recipe 2.7 pt 1 Resume on a different context
    /// ie: if started from a UI thread don't return to that thread, return on a different 
    /// </summary>
    /// <returns></returns>
    public async Task ProcessOnADifferentContextAsync()
    {
        var doSomething = new DoSomething();

        await doSomething.ResumeWithoutContextASync();
    }

    /// <summary>
    /// Recipe 2.8 pt 2 Result on calling context's thread
    /// </summary>
    /// <returns></returns>
    public async Task ProcessOnSameContextAsync()
    {
        var doSomething = new DoSomething();

        await doSomething.ResumeOnContextAsync();
    }


    /// <summary>
    /// Recipe 2.9 pt 1 and 2 handling exceptions using async and Task
    /// </summary>
    /// <returns></returns>
    public async Task HandleExceptionsAsyncTaskMethodsAsync()
    {
        var doSomething = new DoSomething();

        await doSomething.TestThrowExceptionAsync();
    }

    /// <summary>
    /// Recipe 2.9 Handle exceptions from async void methods
    /// </summary>
    /// <returns></returns>
    public async Task HandleExceptionsAsyncVoidAsync()
    {
        /*

        Best not to propogate exceptions out of async avoid methods and handle them inside instead
        sealed class MyAsyncCommand : ICommand
        {
            async void ICommand.Execute(object parameter)
            {
            await Execute parameter();
            }

            public async Task Execute (object parameter)
            {
                //Asychronous command implementation goes here.
            }

            //other members follow here
        }
        */
    }

    /// <summary>
    /// Recipe 2.10 pt1 
    /// Where a synchronous result can be returned and asynchronous behavior is more rare.
    /// Generally you should be using Task<T> and not ValueTask<T> unless profiling suggests you'd see a performance increase.
    /// </summary>
    /// <returns></returns>
    public async Task<int> CreatingAValueTaskAsync()
    {
        var doSomething = new DoSomething();

        var result = await doSomething.MethodValueTaskAsync();
        return result;
    }

    /// <summary>
    /// Recipe 2.10 pt2
    /// Run an async method synchronously conditionally.
    /// </summary>
    /// <returns></returns>
    public async Task<int> CreateSlowValueTaskAsync()
    {

        var doSomething = new DoSomething();
        doSomething.CanBehaveSynchronously = true;

        var result = await doSomething.MethodValueTaskPt2Async();
        return result;
    }

    /// <summary>
    /// Recipe 2.11 pt1
    /// Most common way is to do just await the method.
    /// </summary>
    /// <returns></returns>
    public async Task<int> ConsumingValueTaskAsync()
    {
        var doSomething = new DoSomething();

        var result = await doSomething.ConsumingValueTaskAsync();

        return result;
    }

    /// <summary>
    /// Recipe 2.11 pt2
    /// await's the task after call to method.
    /// </summary>
    /// <returns></returns>
    public async Task<int> ConsumingValueTaskWaitingAsync()
    {
        var doSomething = new DoSomething();

        var result = await doSomething.ConsumingValueTaskAsync2();

        return result;
    }


    /// <summary>
    /// Recipe 2.11 pt3
    /// Uses AsTask to convert the ValueTask to a Task
    /// </summary>
    /// <returns></returns>
    public async Task<int> ConsumingValueTaskConvertingToTaskAsync()
    {
        var doSomething = new DoSomething();

        var result = await doSomething.ConsumingValueTaskAsync3();

        return result;
    }

    /// <summary>
    /// Recipe 2.11 pt4
    /// ValueTasks can only have AsTask() called on them once.
    /// </summary>
    /// <returns></returns>
    public async Task<int> ConsumingMultipleValueTasksConvertedToTaskAsync()
    {
        var doSomething = new DoSomething();

        var result = await doSomething.ConsumingValueTaskAsync4();

        return result;
    }
}