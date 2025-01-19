using System;
using System.Linq;
using System.Threading;

class Program
{
    // Количество потоков
    const int ThreadCount = 12;  // Соответствует количеству логических процессоров
    // Время работы каждой задачи
    const int ThreadLifeTime = 10; // Время в секундах
    // Время наблюдения
    const int ObservationTime = 30; // Время в секундах
    // Матрица для хранения информации о работе задач
    static int[,] Matrix = new int[ThreadCount, ObservationTime];
    // Время начала
    static DateTime StartTime = DateTime.Now;

    // Метод, который выполняет вычисления в потоке
    static void WorkThread(object o)
    {
        int id = (int)o;
        for (int i = 0; i < ThreadLifeTime * 20; i++)
        {
            DateTime CurrentTime = DateTime.Now;
            int ElapsedSeconds = (int)Math.Round(CurrentTime.Subtract(StartTime).TotalSeconds - 0.49);

            if (ElapsedSeconds >= 0 && ElapsedSeconds < ObservationTime)
            {
                Matrix[id, ElapsedSeconds] += 50;
            }
            MySleep(50); // Симуляция работы потока
        }
    }

    // Метод MySleep, который блокирует поток на определенное время
    static Double MySleep(int ms)
    {
        DateTime start = DateTime.Now;
        while ((DateTime.Now - start).TotalMilliseconds < ms)
        {
            // Блокировка потока
        }
        return 0;
    }

    static void Main(string[] args)
    {
        Thread[] t = new Thread[ThreadCount];
        Console.WriteLine("Student... creates problems...");

        // Создаем и запускаем потоки с разными приоритетами
        for (int i = 0; i < ThreadCount; ++i)
        {
            object o = i;
            t[i] = new Thread(WorkThread);

            if (i < 2) // Первые 2 потока (минимальный приоритет)
                t[i].Priority = ThreadPriority.Lowest;
            else // Остальные потоки (максимальный приоритет)
                t[i].Priority = ThreadPriority.Highest;

            t[i].Start(o);
        }

        Console.WriteLine("Student...waiting for tasks to be completed...");

        // Ожидаем завершения всех потоков
        for (int i = 0; i < ThreadCount; i++)
        {
            t[i].Join();
        }

        // Выводим результаты в виде таблицы
        Console.WriteLine("\nTask results:");
        Console.WriteLine("Second | " + string.Join(" | ", Enumerable.Range(0, ThreadCount).Select(i => $"Task {i}")));
        Console.WriteLine(new string('-', 50));

        for (int s = 0; s < ObservationTime; s++)
        {
            Console.Write($"{s,7}: |");
            for (int th = 0; th < ThreadCount; th++)
            {
                Console.Write($" {Matrix[th, s],5} |");
            }
            Console.WriteLine();
        }
    }
}
