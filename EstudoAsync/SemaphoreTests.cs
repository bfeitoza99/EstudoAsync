using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoAsync
{
    public static class SemaphoreTests
    {
        
        private static Semaphore _pool;
        
        private static int _padding;

        public static void FirtTest()
        {
            _pool = new Semaphore(initialCount: 0, maximumCount: 1);

            for (int i = 1; i <= 5; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Worker));

               
                t.Start(i);
            }

            Thread.Sleep(500);
            _pool.Release(releaseCount: 1);


        }

        private static void Worker(object num)
        {
       
            Console.WriteLine("A thread {0}, esta esperando para entrar no semafaro", num);
            _pool.WaitOne();
           
            int padding = Interlocked.Add(ref _padding, 100);

            Console.WriteLine("Thread {0} entrou no semafaro {1}", num, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",CultureInfo.InvariantCulture));


            Thread.Sleep(1000 + padding);


            //Console.WriteLine("Thread {0} finalizou ", num);
            Console.WriteLine("Thread {0} finalizou e liberou espaço no semafaro. {1}",
                num, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture), _pool.Release());
        }
    }
}
