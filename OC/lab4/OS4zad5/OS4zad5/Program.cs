using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Начало MySleep(10000)");
        MySleep(10000);
        Console.WriteLine("Конец MySleep(10000)");
    }

    static double MySleep(int ms)
    {
        double sum = 0, temp;
        // Получаем текущее время
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddMilliseconds(ms);

        while (DateTime.Now < endTime)
        {
            for (int t = 0; t < 100; ++t) // Параметр, чтобы достичь нужной нагрузки
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