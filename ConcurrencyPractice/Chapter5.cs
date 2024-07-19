using ConcurrencyPractice;

public class Chapter5
{
    DoSomething _doSomething;

    public Chapter5()
    {
        _doSomething = new DoSomething();
    }

    /// <summary>
    /// Recipe 5.1 pt1 Basic block linking
    /// </summary>
    /// <returns></returns>
    public async Task LinkingBlocksAsync()
    {
        await _doSomething.TplBasic();
    }

    /// <summary>
    /// Recipe 5.2 pt1 Propagating errors, catching them
    /// </summary>
    /// <returns></returns>
    public async Task PropagatingErrorsAsync()
    {
        await _doSomething.PropogateErrors();
    }

    /// <summary>
    /// Recipe 5.2 pt2 Propagating errors as aggregate exception
    /// </summary>
    /// <returns></returns>
    public async Task PropagatingErrorsAsAggregateAsync()
    {
        await _doSomething.PropogateErrorsPt2();
    }

    /// <summary>
    /// Recipe 5.3 Unlinking blocks
    /// </summary>
    /// <returns></returns>
    public void UnlinkingBlocks()
    {
        _doSomething.UnLinkExample();
    }

    /// <summary>
    /// Recipe 5.4 throttling block examples
    /// </summary>
    public void ThrottlingBlockExample()
    {
        _doSomething.ThrottlingBlocks();
    }

    /// <summary>
    /// Recipe 5.5 parallel processing example
    /// </summary>
    public void ParallelProcessingBlocksExample()
    {
        _doSomething.ParallelProcessingDataflowBlocks();
    }
}