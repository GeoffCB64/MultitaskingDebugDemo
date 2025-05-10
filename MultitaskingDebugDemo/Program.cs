using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleParallelDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting parallel debug demo...\n");

            // Create a new instance of the Worker class, passing in a logger method
            var worker = new Worker(Console.WriteLine);
            
            await worker.RunAllDemosAsync();

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// This class demonstrates various parallel programming techniques in C#.
    /// Place breakpoints in CPUIntensiveTask(), IOBoundTask(), Log(), and the LINQ query to 
    /// observe the behavior of parallel execution in the Threads, Tasks, Parallel Stack and
    /// Parallel Watch debug windows
    /// </summary>
    public class Worker
    {

        private readonly Action<string> _log;

        public Worker(Action<string> logger)
        {
            _log = logger;
        }

        public async Task RunAllDemosAsync()
        {
            _log($"Starting demo...running on Thread {Environment.CurrentManagedThreadId}...\n");
            Stopwatch sw = Stopwatch.StartNew();

            // CPU-bound task
            Log($"Starting CPU-bound task...");
            var cpuTask = Task.Run(() => CPUIntensiveTask("Task.Run CPU"));

            // I/O-bound task
            Log("Entering I/O-bound task...");
            var ioTask = IOBoundTask("Async I/O Task");

            // Parallel.For (blocking)
            Log("Entering Parallel.For (blocking)...");
            Parallel.For(0, 5, i =>
            {
                Log($"Parallel.For index {i} processing on Thread {Environment.CurrentManagedThreadId}");
                int localValue = i * 10;
                Log($"localValue = {localValue}"); // Place breakpoint here
            });


            // Parallel.Invoke
            Log("Entering Parallel.Invoke...");
            Parallel.Invoke(
                () => Log($"Invoke 1 processing on Thread {Environment.CurrentManagedThreadId}"),
                () => Log($"Invoke 2 processing on Thread {Environment.CurrentManagedThreadId}"),
                () => Log($"Invoke 3 processing on Thread {Environment.CurrentManagedThreadId}")
            );

            // LINQ with Thread.Sleep
            Log("Entering LINQ.AsParallel...");
            var processed = Enumerable.Range(1, 5)
                .AsParallel()
                .Select(n =>
                {
                    Log($"AsParallel processing {n} on Thread {Environment.CurrentManagedThreadId}");
                    Thread.Sleep(1000); // Simulate work
                    return n * 2;
                })
                .ToList(); // Forces execution immediately

            foreach (var result in processed)
            {
                Log($"Printing synchronously the AsParallel result: {result}");
            }

            Log("Entering await Task.WhenAll...");
            await Task.WhenAll(cpuTask, ioTask);

            sw.Stop();
            Log($"All tasks completed in {sw.ElapsedMilliseconds} ms.");
        }

        private void CPUIntensiveTask(string label)
        {
            Log($"{label} started on Thread {Environment.CurrentManagedThreadId}");
            double result = 0;
            for (int i = 0; i < 10000000; i++)
                result += Math.Sqrt(i);
            Log($"{label} completed on Thread {Environment.CurrentManagedThreadId}");
        }

        private async Task IOBoundTask(string label)
        {
            Log($"{label} started on Thread {Environment.CurrentManagedThreadId}");
            await Task.Delay(1000);
            Log($"{label} resumed on Thread {Environment.CurrentManagedThreadId}");
        }

        private void Log(string message)
        {
            _log($"{DateTime.Now:HH:mm:ss} {message}");
        }
    }
}
