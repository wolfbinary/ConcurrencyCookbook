
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
    /// Recipe 3.3 pt1 and pt2 Using Linq with Asynchronous Streams
    /// Small difference between these. One uses a WhereAwait and 
    /// the other just declares the use of return type to get the 
    /// stream./*  */
    /// </summary>
    /// <returns></returns>
    public async Task WhereAwaitLinqWithAsynchronousStreamsAsync()
    {
        await _doSomething.ProcessSlowRangeAsync();
    }

    /// <summary>
    /// Recipe 3.3 pt3 Example of a scalar, value based asynchronous stream methods
    /// Scalar, value returning are postfixed with Async, can be combined with await;
    /// takes in an delegate
    /// </summary>
    /// <returns></returns>
    public async Task ScalarLinqAsynchronousStreamsAsync()
    {
        await _doSomething.ProcessSlowRangeCountAsync();
    }

    /// <summary>
    /// Recipe 3.3 pt4 Example of sequence returning stream method
    /// Sequence consuming methods are postfixed with Await, can be combined with async to
    /// return a value and consume an anonymous  asynchronous method
    /// </summary>
    /// <returns></returns>
    public async Task SequenceLinqAsynchronousStreamsAsync()
    {
        await _doSomething.ProcessSlowRangeCount2Async();
    }

    /// <summary>
    /// Recipe 3.4 pt1 Basic example of canceling a stream
    /// Cancels the sequence after 500 milliseconds.
    /// </summary>
    /// <returns></returns>
    public async Task CancelAsynchronousStreamBasicAsync()
    {
        await _doSomething.ProcessCancelledSlowRangeAsync();
    }

    /// <summary>
    /// Recipe 3.4 pt2 Canceling the stream's iterator, does not 
    /// cancel the sequence itself.
    /// For pt3 it uses the same solution as pt2, but with ConfigureAwait(false); 
    /// so it has been omitted for brevity. Just place it after the WithCancellation(token) call if needed
    /// </summary>
    /// <returns></returns>
    public async Task CancelAsynchronousStreamOnIteratorAsync()
    {
        var toConsume = _doSomething.SlowRangeCancellation();

        await _doSomething.ConsumeSequence(toConsume);
    }
}