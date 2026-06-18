using System;
using System.Threading.Tasks;

class Program
{
    //static void Main() // синхронний Main
    //{
    //    Console.OutputEncoding = System.Text.Encoding.UTF8;
    //    Console.WriteLine("Запуск кавового автомату...");
    //    Console.WriteLine("Приготування кави розпочато (чекайте 5 секунд)...");

    //    // Жорстке синхронне блокування потоку
    //    Thread.Sleep(5000);

    //    Console.WriteLine("Кава готова! Заберіть склянку.");

    //    // Спроба поспілкуватися з користувачем ПІСЛЯ того, як усе замерзло
    //    Console.WriteLine("Введіть ваше ім'я для програми лояльності:");
    //    string name = Console.ReadLine()!;
    //    Console.WriteLine($"Приємно познайомитися, {name}!");
    //}
    //// 1. Метод Main стає асинхронним, якщо у кьому буде await
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Запуск кавового автомату...");

        // 2. Запускаємо варіння кави у фоні (БЕЗ await, щоб таска просто стартувалп)
        Task coffeeTask = MakeCoffeeAsync();

        // 3. Головний потік абсолютно ВІЛЬНИЙ. Тепер можна паралельно спілкуватися з користувачем
        Console.WriteLine("Поки кава готується, ви можете ввести своє ім'я:");

        // Консоль НЕ заблокована, користувач може  друкувати прямо  зараз
        string name = Console.ReadLine()!;
        Console.WriteLine($"Дякуємо, {name}! Ваше ім'я збережено.");

        // 4. Тепер нам все ж треба дочекатися кави, якщо користувач ввів ім'я швидше, ніж за 5 секунд
        Console.WriteLine("Очікуємо фінальної готовності напою...");

        Console.ForegroundColor = ConsoleColor.Green;
        await coffeeTask;
        Console.WriteLine("Кава готова! Гарного дня!");
        Console.ResetColor();
    }

    // Імітація асинхронного процесу варіння кави
    static async Task MakeCoffeeAsync()
    {
        // Не блокує потік,  ставить затримку на 5 секунд
        await Task.Delay(5000);
    }
}