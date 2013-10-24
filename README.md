# BenchShark #

A lightweight library for benchmarking performance of your .NET applications. BenchShark provides a powerful and easy to use API to evaluate the speed of your functions in order to track down the bottlenecks within your code.

At the opposite of most software that profile the entire of .NET applications generically, this library is made to target predefined functions to evaluate, the results are then queryable programmatically.

BenchShark was written in accordance with the best practices exposed by [Eric Lippert](http://ericlippert.com/about-eric-lippert/) in his articles called « C# Performance Benchmark Mistakes » [Part One](http://tech.pro/blog/1293/c-performance-benchmark-mistakes-part-one), [Part Two](http://tech.pro/tutorial/1295/c-performance-benchmark-mistakes-part-two), [Part Three](http://tech.pro/tutorial/1317/c-performance-benchmark-mistakes-part-three) and [Part Four](http://tech.pro/tutorial/1433/performance-benchmark-mistakes-part-four).

## Features ##

The benchmark operations are executed through the class `BenchShark`, the entry point of the library.

![](http://binarysharp.com/ccs_files/Products/BenchShark/Class_BenchShark.png)

### Simplistic evaluations ###

The simplest way to use BenchShark is to create an instance of the main class and call the method `EvaluateTask`.

```csharp
public void SimplisticTask()
{
    // Some code...
}

// [..]

// Initialize a new instance of BenchShark
var shark = new BenchShark();

// Evaluate the function with 100 iterations
var result = shark.EvaluateTask(SimplisticTask, 1000);

// Display the result
Console.WriteLine("Total elapsed: {0} milliseconds / {1} ticks",
    result.TotalExecutionTime,
    result.TotalElapsedTicks);
Console.WriteLine("Average elapsed: {0} milliseconds / {1} ticks",
    result.AverageExecutionTime,
    result.AverageElapsedTicks);
```

The method `SimplisticTask` without parameter and return value can be passed directly to the evaluation. The method is executed 1000 times and the result is stored in an instance of the class `EvaluationResult`.

If the function to evaluate contains parameter(s) a return a value, it can be encapsulated within a lambda expression.

```csharp
// Prepare the parameters
var param1 = 2;
var param2 = 3;
// Create the lambda expression
Action task = () => Math.Max(param1, param2);

// Evaluate the function with 100 iterations
var result = shark.EvaluateTask(task, 1000);
```

This is the content of class `EvaluationResult`.

![](http://binarysharp.com/ccs_files/Products/BenchShark/Class_EvaluationResult.png)

### Multiple evaluations ###

In addition to be able to call the method `EvaluateTask` multiple times, BenchShark also provides a way to store several functions and perform the evaluation at the same time. Thus, the results are centralized in an instance of the class `EvaluationResultCollection`.

```csharp
// Prepare the parameter
var param = "Hello";

// Add the tasks
shark.AddTask("Static comparison", () => string.Equals(param, param));
shark.AddTask("Instance comparison", () => param.Equals(param));

// Evaluate the tasks with 100 iterations
var results = shark.EvaluateStoredTasks(100);

// Display the results
Console.WriteLine("The best result was {0} in {1} milliseconds / {2} ticks",
    results.FastestEvaluations.First().Name,
    results.FastestEvaluations.First().AverageExecutionTime,
    results.FastestEvaluations.First().AverageElapsedTicks);
```

This is the content of class `EvaluationResultCollection`.

![](http://binarysharp.com/ccs_files/Products/BenchShark/Class_EvaluationResultCollection.png)

### Evaluations by reflection ###

BenchShark offers the possibility to evaluate predefined methods within a class or an instance of class using reflection with the attribute `BenchSharkTaskAttribute`.

This is an example where an instance of a class is evaluating itself.

```csharp
/// <summary>
/// Define a class that contains evaluations.
/// </summary>
public class Evaluations
{
    /// <summary>
    /// Runs the evaluations in the current instance of the class.
    /// </summary>
    public void Run()
    {
        // Initialize a new instance of BenchShark
        var shark = new BenchShark();
        // Evaluate all the tasks in this instance with 100 iterations
        var results = shark.EvaluateDecoratedTasks(this, 100);
    }

    [BenchSharkTask("Math Power Function")]
    private double TaskOne()
    {
        return Math.Pow(5, 2);
    }

    [BenchSharkTask("Math Power Operator")]
    private double TaskTwo()
    {
        return 5 * 5;
    }
}
```

One idea is to create a dedicated project for benchmarking purpose for in the .NET solution, like a unit tests project.

This class can also be evaluate without creating an instance. This code can be called from the entry point of the application or a unit test.

```csharp
/// <summary>
/// Define a class that contains evaluations.
/// </summary>
public class Evaluations
{
    // Tasks definition...
}

// [..]

// Evaluate all the tasks in the class with 100 iterations
var results = shark.EvaluateStoredTasks<Evaluations>(100);
```

### Using the events ###

The library allows the application to subscribe to some events exposed by the instances of the class `BenchShark`.

The event `IterationCompleted` is raised when an iteration was executed and gives the completed iteration and the state of the current evaluation. 

```csharp
// Initialize a new instance of BenchShark
var shark = new BenchShark();
// Subscribe to an event
shark.IterationCompleted += SharkOnIterationCompleted;

// [..]

private void SharkOnIterationCompleted(object sender, BenchSharkIterationEventArgs args)
{
    // Display the information of the iteration
    Console.WriteLine("Iteration executed in {0} milliseconds / {1} ticks",
        args.CurrentIteration.ExecutionTime,
        args.CurrentIteration.ElapsedTicks);
    // Display the state of the current evaluation
    Console.WriteLine("Run #{0}. Average: {1} milliseconds / {2} ticks",
        args.CurrentEvaluation.IterationsCount,
        args.CurrentEvaluation.AverageExecutionTime,
        args.CurrentEvaluation.AverageElapsedTicks);
}
```

This event is especially useful if you are building an interface that shows the current state of the evaluations for an application. The information can be displayed in a window and refreshed every time an iteration occurs.

The event `EvaluationCompleted` is raised when an evaluation was fully evaluated.

```csharp
// Initialize a new instance of BenchShark
var shark = new BenchShark();
// Subscribe to an event
shark.EvaluationCompleted += SharkOnEvaluationCompleted;

// [..]
private void SharkOnEvaluationCompleted(object sender, BenchSharkEvaluationEventArgs args)
{
    // Display the result
    Console.WriteLine("Total elapsed: {0} milliseconds / {1} ticks",
        args.TaskEvaluated.TotalExecutionTime,
        args.TaskEvaluated.TotalElapsedTicks);
    Console.WriteLine("Average elapsed: {0} milliseconds / {1} ticks",
        args.TaskEvaluated.AverageExecutionTime,
        args.TaskEvaluated.AverageElapsedTicks);
}
```

### Advanced settings ###

**Performance gatekeeper**

BenchShark does not grant to run benchmarks when the solution is compiled in *Debug* mode or if a debugger is attached to the .NET process, in order to avoid wrong results. This protection can be bypassed by setting the the property `EnableUnoptimizedEvaluations` of the instance of the class `BenchShark` to `true`.

**Keep all the iterations in memory**

When an evaluation of 1000 iterations is requested, the library does not keep a trace of all of them. It basically sums the total/average and checks for the best/worst performance. Thus, it saves a lot of memory if the application runs several millions of iterations. Nevertheless, the developer can choose to keep all the iterations to make statistics afterwards. This behavior can be overrided by settings the first parameter of the constructor of the class `BenchShark` to `true`. Using the following constructor forces the library to keep all the iterations in memory and querable through the instances of the class `EvaluationResult`.

```csharp
// Initialize a new instance of BenchShark and force the evaluations to keep all the iterations
var shark = new BenchShark(true);
```

**Garbage Collection**

BenchShark runs the *Garbage Collector* before each iteration in order to avoid that it collects the unused objects during the evaluation. This behavior can be very expensive if the application runs several millions of iterations. Hopefully, it can be overrided by settings the second parameter of the constructor of the class `BenchShark`. Using the following constructor runs the *Garbage Collector* each 100 iterations.

```csharp
// Initialize a new instance of BenchShark and override the interval of garbage collection
var shark = new BenchShark(false, 100);
```

## Author ##
This developer and the copyright holder of this library is [ZenLulz (Jämes Ménétrey)](https://github.com/ZenLulz).  
The official website of BenchShark is [www.binarysharp.com](www.binarysharp.com).

## License ##
BenchShark is licensed under the MIT License (as of v2.0.0). The license is simple and easy to understand and it places almost no restrictions on what you can do with a project using BenchShark. You are free to use any BenchShark project in any other project (even commercial projects) as long as the copyright header is left intact.