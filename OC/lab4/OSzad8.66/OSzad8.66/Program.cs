using System;
using System.Threading;

class Program
{
    static int Count = 0; // Общая переменная
    static object lockObject = new object(); // Объект для синхронизации

    static void WorkThread()
    {
        for (int i = 0; i < 5000000; ++i)
        {
            // Синхронизация доступа к переменной Count
            lock (lockObject)
            {
                Count = Count + 1;
            }
        }
    }

    static void Main(string[] args)
    {
        Thread[] threads = new Thread[20];

        // Запуск 20 потоков
        for (int i = 0; i < 20; ++i)
        {
            threads[i] = new Thread(WorkThread);
            threads[i].Start();
        }

        // Ожидание завершения всех потоков
        for (int i = 0; i < 20; ++i)
        {
            threads[i].Join();
        }

        Console.WriteLine($"Результат: {Count}");
        Console.WriteLine($"Ожидаемое значение: {20 * 5000000}");
    }
}