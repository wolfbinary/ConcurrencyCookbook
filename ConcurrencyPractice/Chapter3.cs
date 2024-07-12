
using ConcurrencyPractice;


/// <summary>
/// Creating Asynchronous Streams
/// </summary>
public class Chapter3
{

    private DoSomething _doSomething = new DoSomething();

    /// <summary>
    /// Recipe 3.1 and 3.2 pt1 Create/Consume Asynchronous Streams
    /// </summary>
    /// <returns></returns>
    public async Task CreateAsynchronousStreamPt1Async()
    {
        await foreach (var result in _doSomething.GetValuesAsync())
        {
            Console.WriteLine(result);
        }
    }

    /// <summary>
    /// Recipe 3.1 and 3.2 pt2 A more realistic example of creating/consuming asychrnous streams
    /// </summary>
    /// <returns></returns>
    public async Task TaskCreateAsynchronousStreamPt2Async()
    {
        using var client = new HttpClient();

        await foreach (var result in _doSomething.GetValuesPt2Async(client))
        {
            Console.WriteLine(result);
        }
    }

    /// <summary>
    /// Recipe 3.3 pt1 Using Linq with Asynchronous Streams
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task WhereAwaitLinqWithAsynchronousStreamsAsync()
    {
        await _doSomething.ProcessSlowRangeAsync();
    }
}