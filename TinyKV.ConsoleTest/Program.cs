using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TinyKV.Client;

class Program
{
    private static int setCount = 0;
    private static long totalSetTicks = 0;

    static async Task Main()
    {
        var client = new TinyKVClient("http://localhost:5009/tinykv");
        await client.StartAsync();

        int maxKeys = 200_000;
        int iterations = 200_000;

        Console.WriteLine("Starting Set performance test...");

        await RunSetLoop(client, maxKeys, iterations);

        Console.SetCursorPosition(0, Console.CursorTop);
        Console.WriteLine($"Progress: [SET: {setCount}/{iterations}] ✅");

        // Convert ticks to milliseconds
        double totalSetTimeMs = (double)totalSetTicks / TimeSpan.TicksPerMillisecond;
        double avgSetTimeMs = setCount > 0 ? totalSetTimeMs / setCount : 0;

        Console.WriteLine("\nPerformance Metrics:");
        Console.WriteLine($"- Total Set Time: {totalSetTimeMs:F2} ms");
        Console.WriteLine($"- Avg Set Time per Operation: {avgSetTimeMs:F6} ms");

        Console.WriteLine("\nSet operation test completed.");
        Console.ReadLine();
    }

    static async Task RunSetLoop(TinyKVClient client, int maxKeys, int iterations)
    {
        int batchSize = 100; // Tune this value for optimal throughput
        var batchTasks = new List<Task>(batchSize);
        var stopwatch = new Stopwatch();

        for (int i = 0; i < iterations; i++)
        {
            int key = i % maxKeys;
            string keyStr = $"key:{key}";
            string valueStr = $"value:{key}";

            stopwatch.Restart();
            batchTasks.Add(client.SetAsync(keyStr, valueStr));

            // Execute batch when full
            if (batchTasks.Count >= batchSize)
            {
                await Task.WhenAll(batchTasks); // Execute all in parallel
                batchTasks.Clear();
            }

            Interlocked.Increment(ref setCount);
            Interlocked.Add(ref totalSetTicks, stopwatch.ElapsedTicks);
        }

        // Final batch execution
        if (batchTasks.Count > 0)
            await Task.WhenAll(batchTasks);
    }

}
