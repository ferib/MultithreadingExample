using System;
using System.Threading;

namespace PoC_MultiThread
{
    class Program
    {
        private static int IntervalTimeMs = 50;
        private static int ThreadCount = 21;
        private static DateTime StartTime = DateTime.Now;
        private static Thread[] ScanningThreads = new Thread[ThreadCount];
        private static DateTime[] LastThreadActivity = new DateTime[ThreadCount];
        private static long[] ThreadResult = new long[ThreadCount];
        private static int DecreaseValue = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("   ____ ___ ___  _ ___  ");
            Console.WriteLine("  |  __| __| _ \\| | _ \\ ");
            Console.WriteLine("  |  _|| _||   /| | _ |");
            Console.WriteLine("  |_|  |___|_|_\\|_|___/");
            StartThreads();
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("   ╔══════════════════════════╗");
                //Console.WriteLine("   ║      Whale Hunter 1.0    ║");
                Console.WriteLine("   ║     ~ Thread Ripper ~    ║");
                Console.WriteLine("   ╠══════════════════════════╣");
                for (int i = 1; i < ThreadResult.Length; i++)
                {
                    Console.WriteLine("   ║ " + LastThreadActivity[i].ToString("HH:mm:ss.FFF").PadRight(11) + " - " +  ThreadResult[i].ToString().PadLeft(9) + " ║");
                }
                Console.WriteLine("   ╚══════════════════════════╝");
                Thread.Sleep(50);
                Console.Clear();
            }
            Console.WriteLine("Killing threads...");
            KillThreads();
            Console.ReadLine();
        }

        private static void StartThreads()
        {
            DateTime StartTime = DateTime.Now;
            for (int i = 0; i < ScanningThreads.Length; i++)
            {
                ScanningThreads[i] = new Thread(new ThreadStart(TestSleep));
                ScanningThreads[i].Start();
                if (i == 0)
                    DecreaseValue = ScanningThreads[i].ManagedThreadId;
                LastThreadActivity[i] = StartTime.AddMilliseconds(i*IntervalTimeMs);
            }
        }
        private static void KillThreads()
        {
            for (int i = 0; i < ScanningThreads.Length; i++)
            {
                if(ScanningThreads[i].IsAlive)
                    ScanningThreads[i].Abort();
            }
        }

        private static void TestSleep()
        {
            while (true)
            {
                if(LastThreadActivity[Thread.CurrentThread.ManagedThreadId - DecreaseValue].AddMilliseconds((ThreadCount-1)*IntervalTimeMs) < DateTime.Now)
                {
                    LastThreadActivity[Thread.CurrentThread.ManagedThreadId - DecreaseValue] = LastThreadActivity[Thread.CurrentThread.ManagedThreadId - DecreaseValue].AddMilliseconds((ThreadCount - 1) * IntervalTimeMs);
                    Random rnd = new Random();
                    ThreadResult[Thread.CurrentThread.ManagedThreadId - DecreaseValue] = rnd.Next(1000, 10000);
                }
                else
                {
                    Thread.Sleep(2);
                }
            }
        }
    }
}
