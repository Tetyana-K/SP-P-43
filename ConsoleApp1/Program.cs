using System;
using System.IO;
using System.Threading;

class Program
{
    static string filePath = "data1.txt";
    static string resultPath = "result1.txt"; // +
    static string resultPath2 = "result2.txt"; // *

    static ManualResetEvent doneEvent = new ManualResetEvent(false);

    static void GeneratePairs()
    {
        Random rnd = new Random();

        using (StreamWriter sw = new StreamWriter(filePath))
        {
            for (int i = 1; i <= 10; i++)
            {
                int a = rnd.Next(1, 10);
                int b = rnd.Next(1, 10);

                sw.WriteLine($"{a} {b}");
                Console.WriteLine($"Згенеровано: {a} {b}");

                Thread.Sleep(200);
            }
        }

        doneEvent.Set(); // сигнал
    }

    static void CalculateSum()
    {
        doneEvent.WaitOne();

        using (StreamReader sr = new StreamReader(filePath))
        using (StreamWriter sw = new StreamWriter(resultPath, true))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                int a = int.Parse(parts[0]);
                int b = int.Parse(parts[1]);

                sw.WriteLine($"Сума: {a + b}");
            }
        }

        Console.WriteLine("Сума є");
    }

    static void CalculateProduct()
    {
        doneEvent.WaitOne();

        using (StreamReader sr = new StreamReader(filePath))
        using (StreamWriter sw = new StreamWriter(resultPath2, true))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(' ');
                int a = int.Parse(parts[0]);
                int b = int.Parse(parts[1]);

                sw.WriteLine($"Продукт: {a * b}");
            }
        }

        Console.WriteLine("Продукт є");
    }

    static void Main()
    {
        Thread t1 = new Thread(GeneratePairs);
        Thread t2 = new Thread(CalculateSum);
        Thread t3 = new Thread(CalculateProduct);

        t1.Start();
        t2.Start();
        t3.Start();
        t1.Join();
        t2.Join();
        t3.Join();

        Console.WriteLine("Програма завершена");
    }
}
