using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        class Test
        {
            public int Value = 5;
        }

        static void Main(string[] args)
        {
            var t = new Test();

            new Thread(() =>
            {
                lock (t)
                {
                    Thread.Sleep(5000);
                    Console.WriteLine($"From other thread {t.Value}");
                }
            }).Start();

            Thread.Sleep(1000);
            Console.WriteLine(t.Value);
            Console.ReadKey();
        }
    }
}
