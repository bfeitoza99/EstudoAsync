using System.Diagnostics;

public static class MultThreadTests
{

    public static long ThreadPoolTest(long start, long end)
    {
        long result = 0;
        const long chunkSize = 100;
        var completed = 0;
        var allDone = new ManualResetEvent(initialState: false);

        var chunks = (end - start) / chunkSize;

        for (long i = 0; i < chunks; i++)
        {
            var chunkStart = (start) + i * chunkSize;
            var chunkEnd = i == (chunks - 1) ? end : chunkStart + chunkSize;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (var number = chunkStart; number < chunkEnd; number++)
                {
                    if (IsPrime(number))
                    {
                        Interlocked.Increment(ref result);
                    }
                }

                if (Interlocked.Increment(ref completed) == chunks)
                {
                    allDone.Set();
                }
            });

        }
        allDone.WaitOne();
        return result;
    }

    static bool IsPrime(long number)
    {
        if (number == 2) return true;
        if (number % 2 == 0) return false;
        for (long divisor = 3; divisor < (number / 2); divisor += 2)
        {
            if (number % divisor == 0)
            {
                return false;
            }
        }
        return true;
    }

    private static long MultThread(int start, int end)
    {
        long result = 0;
        var range = end - start;
        var numberOfThreads = (long)Environment.ProcessorCount;

        var threads = new Thread[numberOfThreads];

        var chunkSize = range / numberOfThreads;

        for (long i = 0; i < numberOfThreads; i++)
        {
            var chunkStart = start + i * chunkSize;
            var chunkEnd = i == (numberOfThreads - 1) ? end : chunkStart + chunkSize;
            threads[i] = new Thread(() =>
            {
                for (var number = chunkStart; number < chunkEnd; ++number)
                {
                    if (IsPrime(number))
                    {
                        Interlocked.Increment(ref result);
                    }
                }
            });

            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return result;
    }

    public static void PrimesInRange()
    {
        long result;
        int start = 200;
        int end = 800000;
        var sw = new Stopwatch();
        sw.Start();
        /* result= ThreadUnica(start, end);*/ //61 segundos      
        /*  result= MultThread(start, end);*/ // 25 segundos
        result= ThreadPoolTest(start, end); // 19 segundos
       
        sw.Stop();
        Console.WriteLine($"{result} os numeros foram encontrados em {sw.ElapsedMilliseconds / 1000} seconds ({Environment.ProcessorCount} threads).");
        
    }

    private static object UniqueThread(int start, int end)
    {
        long result = 0;
        for (var number = start; number < end; number++)
        {
            if (IsPrime(number))
            {
                result++;
            }
        }
        return result;
    }
}