// Клас TaskFactory - фабрика задач, яка надає методи для створення та запуску задач.

/*
 TaskFactory  дозволяє:
- запускати таски з параметрами керування (наприклад, CancellationToken, TaskCreationOptions);
- запускати одразу кілька задач;
- працювати з кастомним TaskScheduler.
*/
class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Створюємо фабрику
        TaskFactory factory = new TaskFactory();

        // Створюємо та запускаємо задачу через фабрику
        Task<int> task = factory.StartNew(() => Square(7));

        Console.WriteLine("Main() може виконувати інший код...");
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Main() робить щось {i}");
            await Task.Delay(500); // імітація роботи
        }
        // Очікуємо завершення задачі
        int result = await task; //забираємо результат у змінну result
        Console.WriteLine($"Результат: {result}");
    }

    static int Square(int x)
    {
        Console.WriteLine($"Обчислення {x} * {x}...");
        Task.Delay(2000).Wait(); // імітація довгої операції
        return x * x;
    }
}
//Task.Run = простий спосіб, без додаткових налаштувань.

//TaskFactory.StartNew - гнучкіший, можна передати CancellationToken, TaskCreationOptions, TaskScheduler.