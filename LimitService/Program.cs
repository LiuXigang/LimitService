using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LimitService
{
    public class Program
    {
        private static readonly LimitService L = new LimitService(1000, 1);
        public static void Main(string[] args)
        {
            Test();
            Console.ReadKey();
        }

        public static void Test()
        {
            int threadCount = 10;
            while (threadCount >= 0)
            {
                Task.Run(() => Limit());
                threadCount--;
            }
            Console.Read();
        }
        public static void Limit()
        {
            int i = 0;
            int okCount = 0;
            int noCount = 0;
            Stopwatch w = new Stopwatch();
            w.Start();
            while (i < 1000000)
            {
                var ret = L.IsContinue();
                if (ret)
                    okCount++;
                else
                    noCount++;
                i++;
            }
            w.Stop();
            Console.WriteLine($"共用{w.ElapsedMilliseconds}毫秒,允许：{okCount}次,  拦截：{noCount}次");
        }
    }
}
