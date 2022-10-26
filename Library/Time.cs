using System.Diagnostics;

namespace Librarys
{
    public static class Time
    {
        private static readonly DateTime StartTime = DateTime.Now;
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();
        public static DateTime Now => StartTime + Stopwatch.Elapsed;
    }
}