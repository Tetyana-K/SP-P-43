using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // створюємо фабрику
        TaskFactory factory = new TaskFactory();

        // запускаємо три задачі паралельно
        Task<int> t1 = factory.StartNew(() => Square(5));
        Task<int> t2 = factory.StartNew(() => Cube(3));
        Task<int> t3 = factory.StartNew(() => Multiply(4, 7));

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Main() може робити іншу роботу, поки таски виконуються...");
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Main() робить щось {i}");
            await Task.Delay(500); // імітація роботи
        }
        Console.ResetColor();

        // чекаємо, поки ВСІ задачі завершаться Task.WhenAll(t1, t2, t3), і збираємо результати у масив results
        int[] results = await Task.WhenAll(t1, t2, t3);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n--- Результати ---");
        Console.WriteLine($"Square(5)\t= {results[0]}");
        Console.WriteLine($"Cube(3)\t= {results[1]}");
        Console.WriteLine($"Multiply(4,7)\t= {results[2]}");


    }

    static int Square(int x)
    {
        Console.WriteLine($"[Task 1] Обчислюємо {x} * {x}...");
        Task.Delay(1500).Wait();
        return x * x;
    }

    static int Cube(int x)
    {
        Console.WriteLine($"[Task 2] Обчислюємо {x} * {x} * {x}...");
        Task.Delay(2000).Wait();
        return x * x * x;
    }

    static int Multiply(int a, int b)
    {
        Console.WriteLine($"[Task 3] Обчислюємо {a} * {b}...");
        Task.Delay(1000).Wait();
        return a * b;
    }
}
