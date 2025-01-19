using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program
{
    const int ThreadCount = 20; // Количество потоков
    const int ThreadLifeTime = 10; // Время жизни потока в секундах
    const int ObservationTime = 30; // Время наблюдения в секундах
    static int[,] Matrix = new int[ThreadCount, ObservationTime];
    static DateTime StartTime = DateTime.Now;

    // Метод, выполняемый потоками
    static void WorkThread(object o)
    {
        int id = (int)o;
        for (int i = 0; i < ThreadLifeTime * 20; i++) // 20 итераций в секунду
        {
            DateTime CurrentTime = DateTime.Now;
            int ElapsedSeconds = (int)Math.Round(CurrentTime.Subtract(StartTime).TotalSeconds - 0.49);
            if (ElapsedSeconds < ObservationTime) // Проверка на время наблюдения
            {
                Matrix[id, ElapsedSeconds] += 50; // Обновление матрицы
                MySleep(50); // Задержка
            }
        }
    }

    static void Main(string[] args)
    {
        // Ограничение количества логических процессоров
        Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)15;

        Console.WriteLine("A student ... is placing threads to the pool...");
        Thread[] t = new Thread[ThreadCount];

        for (int i = 0; i < ThreadCount; ++i)
        {
            object o = i;
            t[i] = new Thread(WorkThread);
            // Установка приоритета потока
            if (i % 3 == 0)
            {
                t[i].Priority = ThreadPriority.Lowest; // Минимальный приоритет
            }
            else if (i % 3 == 2)
            {
                t[i].Priority = ThreadPriority.Highest; // Максимальный приоритет
            }
            t[i].Start(o); // Запуск потока
        }

        Console.WriteLine("A student ... is waiting for the threads to finish...");
        foreach (var thread in t)
        {
            thread.Join(); // Ожидание завершения всех потоков
        }

        // Вывод результатов
        Console.WriteLine("Время (секунд) | " + string.Join(" | ", Enumerable.Range(0, ThreadCount).Select(x => $"Поток {x}")));
        Console.WriteLine(new string('-', 50));
        for (int s = 0; s < ObservationTime; s++)
        {
            Console.Write($"{s,3}: ");
            for (int th = 0; th < ThreadCount; th++)
            {
                Console.Write($" {Matrix[th,s],5}");
            }
            Console.WriteLine();
        }
    }

    static double MySleep(int ms)
    {
        double sum = 0, temp;
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddMilliseconds(ms);

        while (DateTime.Now < endTime)
        {
            for (int t = 0; t < 100; ++t)
            {
                temp = 0.711 + (double)t / 10000.0;
                double a, b, c, d, e, nt;

                for (int k = 0; k < 5500; ++k)
                {
                    nt = temp - k / 27000.0;
                    a = Math.Sin(nt);
                    b = Math.Cos(nt);
                    c = Math.Cos(nt / 2.0);
                    d = Math.Sin(nt / 2);
                    e = Math.Abs(1.0 - a * a - b * b) + Math.Abs(1.0 - c * c - d * d);
                    sum += e;
                }
            }
        }
        return sum;
    }
}