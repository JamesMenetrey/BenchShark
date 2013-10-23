# BenchShark #

A lightweight library for benchmarking performance of your .NET applications. BenchShark provides a powerful and easy to use API to evaluate the speed of your functions in order to track down the bottlenecks within your code.

At the opposite of most software that profile the entire of .NET applications generically, this library is made to target predefined functions to evaluate, the results are then queryable programmatically.

BenchShark was written in accordance with the best practices exposed by [Eric Lippert](http://ericlippert.com/about-eric-lippert/) in his articles called « C# Performance Benchmark Mistakes » [Part One](http://tech.pro/blog/1293/c-performance-benchmark-mistakes-part-one), [Part Two](http://tech.pro/tutorial/1295/c-performance-benchmark-mistakes-part-two), [Part Three](http://tech.pro/tutorial/1317/c-performance-benchmark-mistakes-part-three) and [Part Four](http://tech.pro/tutorial/1433/performance-benchmark-mistakes-part-four).

## Features ##

The benchmark operations are executed through the class `BenchShark`, the entry point of the library.

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

![](http://binarysharp.com/ccs_files/Products/BenchShark/EvaluationResult.png)


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

![](http://binarysharp.com/ccs_files/Products/BenchShark/EvaluationResultCollection.png)




## Author ##
This developer and the copyright holder of this library is [ZenLulz (Jämes Ménétrey)](https://github.com/ZenLulz).  
The official website of BenchShark is [www.binarysharp.com](www.binarysharp.com).

## License ##
BenchShark is licensed under the MIT License (as of v2.0.0). The license is simple and easy to understand and it places almost no restrictions on what you can do with a project using BenchShark. You are free to use any BenchShark project in any other project (even commercial projects) as long as the copyright header is left intact.