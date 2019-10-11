using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.AsyncRequests.Models
{
    public class CodeHolder
    {
        public TimeSpan Seconds { get; }

        public CodeHolder(in TimeSpan seconds)
        {
            Seconds = seconds;
        }

        public Status SyncData(int id)
        {
            Thread.Sleep(Seconds);
            return new Status(id);
        }

        public async Task<Status> AsyncData(int id)
        {
            await Task.Delay(Seconds);
            return new Status(id);
        }
    }

    public static class IntExtensions
    {
        public static TimeSpan ToTimeSpanSeconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        public static TimeSpan ToTimeSpanMs(this int ms)
        {
            return TimeSpan.FromSeconds(ms);
        }
    }
}