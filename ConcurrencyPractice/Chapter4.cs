using ConcurrencyPractice;

public class Chapter4
{
    private DoSomething _doSomething = new DoSomething();



    /// <summary>
    /// 4.1 pt1 Rotates the matrices.
    /// </summary>
    /// <param name="matrices">The matrices.</param>
    /// <param name="degrees">The degrees.</param>
    public void RotateMatrices(IEnumerable<Matrix> matrices, float degrees)
    {
        _doSomething.RotateMatrices(matrices, degrees);
    }


    /// <summary>
    /// 4.1 p2 Inverts the matrices.
    /// </summary>
    /// <param name="matrices">The matrices.</param>
    public void InvertMatrices(IEnumerable<Matrix> matrices)
    {
        _doSomething.InvertMatrices2(matrices);
    }

    /// <summary>
    /// 4.1 pt3 Rotates the matrices2.
    /// </summary>
    /// <param name="matrices">The matrices.</param>
    /// <param name="degrees">The degrees.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public void RotateMatrices2(IEnumerable<Matrix> matrices, float degrees, CancellationToken cancellationToken)
    {
        _doSomething.RotateMatrices2(matrices, degrees, cancellationToken);
    }

    /// <summary>
    /// 4.1 pt4 Inverts the matrices2.
    /// </summary>
    /// <param name="matrices">The matrices.</param>
    /// <returns></returns>
    public int InvertMatrices2(IEnumerable<Matrix> matrices)
    {
        return _doSomething.InvertMatrices2(matrices);
    }

    /// <summary>
    /// 4.2 pt1 Parallels the sum.
    /// </summary>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public int ParallelSum(IEnumerable<int> values)
    {
        return _doSomething.ParallelSum(values);
    }


    /// <summary>
    /// 4.2 pt2 Parallels the sum2.
    /// </summary>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public int ParallelSum2(IEnumerable<int> values)
    {
        return _doSomething.ParallelSum2(values);
    }

    /// <summary>
    /// 4.2 pt3 Parallels the sum3.
    /// </summary>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public int ParallelSum3(IEnumerable<int> values)
    {
        return _doSomething.ParallelSum3(values);
    }

    /// <summary>
    /// 4.3 p1 Processes the array.
    /// </summary>
    /// <param name="array">The array.</param>
    public void ProcessArray(double[] array)
    {
        _doSomething.ProcessArray(array);
    }

    /// <summary>
    /// 4.3 pt2 Does the action20 times.
    /// </summary>
    /// <param name="action">The action.</param>
    public void DoAction20Times(Action action)
    {
        _doSomething.DoAction20Times(action);
    }

    /// <summary>
    /// 4.3 pt3 Does the action20 times with cancellation.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public void DoAction20TimesWithCancellation(Action action, CancellationToken cancellationToken)
    {
        _doSomething.DoAction20TimesWithCancellation(action, cancellationToken);
    }

    /// <summary>
    /// 4.4 pt2 Dynamic Parallelism, when the number and structure of the parallelism 
    /// is only known at runtime.
    /// Attaches a child task to a parent task 
    /// </summary>
    /// <param name="root">The root.</param>
    public void DynamicParallelismParentChildTasks(Node root)
    {
        _doSomething.ProcessTree(root);

    }

    /// <summary>
    /// Recipe 4.4 pt2 Example of ContinuWith, kicks off an unrelated task after the first one finishes.
    /// </summary>
    public void TaskOnContinuation()
    {
        _doSomething.TaskOnContinuation();
    }

    /// <summary>
    /// 4.5 pt1 Parallel select query
    /// </summary>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public IEnumerable<int> AsParallelSelect(IEnumerable<int> values)
    {
        return _doSomething.MultiplyBy2(values);
    }

    /// <summary>
    /// 4.5 pt2 parallel orderby select query
    /// </summary>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public IEnumerable<int> AsParallelOrderBySelect(IEnumerable<int> values)
    {
        return _doSomething.MultiplyBy2Ordered(values);
    }

    /// <summary>
    /// 4.5 pt3 Parallels the sum.
    /// </summary>
    /// <param name="values">The values.</param>
    /// <returns></returns>
    public int AsParallelSum(IEnumerable<int> values)
    {
        return _doSomething.ParallelSumChapter4(values);
    }
}