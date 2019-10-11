using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BenchmarkAsync
{
    class Program
    {

        static HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };
        static void Main(string[] args)
        {
            try
            {
                MainAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        static async Task MainAsync()
        {
            var processorInfo = JsonSerializer.Deserialize<ThreadInfo>(
                await client.GetStringAsync("status/threads"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            int times = processorInfo.MaxWorkerThreads;
            var syncUrl = "sync/0";
            var asyncUrl = "async/0";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" Synchronous time for {times} connections: {await RunTest(syncUrl, times)}");

            Console.ForegroundColor = ConsoleColor.Red;
            times = processorInfo.MaxWorkerThreads + 1;
            Console.WriteLine($" Synchronous time for {times} connections: {await RunTest(syncUrl, times)}");

            times = processorInfo.MaxWorkerThreads;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Asynchronous time for {times} connections: {await RunTest(asyncUrl, times)}");

            times = processorInfo.MaxWorkerThreads * 100;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Asynchronous time for {times} connections: {await RunTest(asyncUrl, times)}");
        }

        static async Task<TimeSpan> RunTest(string url, int concurrentConnections)
        {
            var sw = new Stopwatch();

            await client.GetStringAsync(url); // warmup
            sw.Start();
            await Task.WhenAll(Enumerable.Range(0, concurrentConnections).Select(i => client.GetStringAsync(url)));
            sw.Stop();

            return sw.Elapsed;
        }
    }

    public class ThreadInfo
    {
        public int MaxAsyncIOThreads { get; set; }
        public int MaxWorkerThreads { get; set; }
    }
}
