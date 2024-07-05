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
}