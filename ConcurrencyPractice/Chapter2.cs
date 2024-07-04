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

}