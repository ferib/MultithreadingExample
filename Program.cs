using System;
using System.Threading;

namespace PoC_MultiThread
{
    class Program
    {
        private static int IntervalTimeMs = 50;
        private static int ThreadCount = 20;
        private static DateTime StartTime = DateTime.Now;
        private static Thread[] ScanningThreads = new Thread[ThreadCount];
        private static DateTime[] LastThreadActivity = new DateTime[ThreadCount];
        private static long[] ThreadResult = new long[ThreadCount];
        private static int DecreaseValue = 0;
        static void Main(string[] args)
        {
            //ASCII art
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
                    //Print last threads result and date of update for each thread
                    Console.WriteLine("   ║ " + LastThreadActivity[i].ToString("HH:mm:ss.FFF").PadRight(11) + " - " +  ThreadResult[i].ToString().PadLeft(9) + " ║");
                }
                Console.WriteLine("   ╚══════════════════════════╝");
                //sleep to avoid screen from refreshing to fast
                Thread.Sleep(50);
                Console.Clear();
            }
            Console.WriteLine("Killing threads...");
            KillThreads();
            Console.ReadLine();
        }

        private static void StartThreads()
        {
            //start all threads at once, asign current DateTime
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
            //TODO: use cancellation token to close threads properly
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
                if(LastThreadActivity[Thread.CurrentThread.ManagedThreadId - DecreaseValue].AddMilliseconds((ThreadCount)*IntervalTimeMs) < DateTime.Now)
                {
                    LastThreadActivity[Thread.CurrentThread.ManagedThreadId - DecreaseValue] = LastThreadActivity[Thread.CurrentThread.ManagedThreadId - DecreaseValue].AddMilliseconds((ThreadCount) * IntervalTimeMs);
                    //Generate random number to show visible changes
                    Random rnd = new Random();
                    ThreadResult[Thread.CurrentThread.ManagedThreadId - DecreaseValue] = rnd.Next(1000, 10000);
                }
                else
                {
                    //sleep to prevent CPU for getting raped
                    Thread.Sleep(2);
                }
            }
        }
    }
}
