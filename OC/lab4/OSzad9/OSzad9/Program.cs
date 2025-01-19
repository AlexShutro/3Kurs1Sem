using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    const int TaskCount = 10; // Количество задач
    const int TaskLifeTime = 10; // Время жизни задачи в секундах
    const int ObservationTime = 30; // Время наблюдения в секундах
    static int[,] Matrix = new int[TaskCount, ObservationTime];
    static DateTime StartTime = DateTime.Now;

    // Метод, выполняемый задачами
    static void Work(int id)
    {
        for (int i = 0; i < TaskLifeTime * 20; i++) // 20 итераций в секунду
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
        Task[] tasks = new Task[TaskCount];
        Console.WriteLine("A student ... is creating tasks...");

        // Запуск задач в цикле
        for (int i = 0; i < TaskCount; i++)
        {
            int taskId = i; // Локальная переменная для замыкания
            tasks[i] = Task.Run(() => Work(taskId));
        }

        Console.WriteLine("A student ... is waiting for tasks to finish...");
        Task.WaitAll(tasks); // Ожидание завершения всех задач

        // Вывод результатов
        Console.WriteLine("Время (секунд) | " + string.Join(" | ", Enumerable.Range(0, TaskCount).Select(x => $"Задача {x}")));
        Console.WriteLine(new string('-', 50));
        for (int s = 0; s < ObservationTime; s++)
        {
            Console.Write($"{s,3}: ");
            for (int th = 0; th < TaskCount; th++)
            {
                Console.Write($" {Matrix[th, s],5}");
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