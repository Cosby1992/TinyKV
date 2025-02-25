using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TinyKV.Client;

class Program
{
    private static int setCount = 0;
    private static int deleteCount = 0;
    private static long totalSetTicks = 0;     // Total ticks for Set operations
    private static long totalDeleteTicks = 0;  // Total ticks for Delete operations
    private static object lockObj = new();    // Lock for updating time safely
    private static readonly long TicksPerMillisecond = TimeSpan.TicksPerMillisecond;

    static async Task Main()
    {
        var client = new TinyKVClient("http://localhost:5009/tinykv");
        await client.StartAsync();

        int maxKeys = 200_000;
        int iterations = 200_000;

        Console.WriteLine("Starting concurrent Set & Delete operations...");

        // Start two parallel tasks
        await Task.WhenAll(RunSetLoop(client, maxKeys, iterations), RunDeleteLoop(client, maxKeys, iterations));

        // Ensure final console update after tasks are done
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.WriteLine($"Progress: [SET: {setCount}/{iterations}]  [DELETE: {deleteCount}/{iterations}] ✅");

        // Convert ticks to milliseconds
        double totalSetTimeMs = (double)totalSetTicks / TicksPerMillisecond;
        double totalDeleteTimeMs = (double)totalDeleteTicks / TicksPerMillisecond;
        double avgSetTimeMs = setCount > 0 ? totalSetTimeMs / setCount : 0;
        double avgDeleteTimeMs = deleteCount > 0 ? totalDeleteTimeMs / deleteCount : 0;

        Console.WriteLine($"\nPerformance Metrics:");
        Console.WriteLine($"- Total Set Time: {totalSetTimeMs:F2} ms");
        Console.WriteLine($"- Total Delete Time: {totalDeleteTimeMs:F2} ms");
        Console.WriteLine($"- Avg Set Time per Operation: {avgSetTimeMs:F6} ms");
        Console.WriteLine($"- Avg Delete Time per Operation: {avgDeleteTimeMs:F6} ms");

        Console.WriteLine("\nAll iterations completed.");
        Console.ReadLine();
    }

    static async Task RunSetLoop(TinyKVClient client, int maxKeys, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            int key = i % maxKeys;
            string keyStr = $"key:{key}";
            string valueStr = $"value:{key}";

            Stopwatch stopwatch = Stopwatch.StartNew();
            await client.SetAsync(keyStr, valueStr);
            stopwatch.Stop();

            Interlocked.Increment(ref setCount); // ✅ Thread-safe update

            // Update total time safely using Ticks
            lock (lockObj)
            {
                totalSetTicks += stopwatch.ElapsedTicks;
            }
        }
    }

    static async Task RunDeleteLoop(TinyKVClient client, int maxKeys, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            int key = i % maxKeys;
            string keyStr = $"key:{key}";

            Stopwatch stopwatch = Stopwatch.StartNew();
            await client.DeleteAsync(keyStr);
            stopwatch.Stop();

            Interlocked.Increment(ref deleteCount); // ✅ Thread-safe update

            // Update total time safely using Ticks
            lock (lockObj)
            {
                totalDeleteTicks += stopwatch.ElapsedTicks;
            }
        }
    }
}
