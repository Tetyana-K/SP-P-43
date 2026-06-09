

object obj = new object(); // об'єкт для блокування

// Перший потік (виводяться букви)
ParameterizedThreadStart threadStart = new ParameterizedThreadStart(PrintLetters);
Thread thread = new Thread(threadStart);
thread.Start('Z'); // запускаємо потік, передаємо 'Z' як параметр

//Thread thread = new Thread(PrintLetters);
//thread.Start('Z'); // Передаємо 'Z' як параметр

int left = 1, right = 50;
// Другий потік  з лямбда-виразом (виводяться числа)
Thread thread2 = new Thread(() =>
{
    for (int i = left; i < right; i++)
    {
        lock (obj) // Блокування для синхронізації доступу
        {

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\t\t\t\t{i} in thread 2");
            Console.ResetColor();
        }
        Thread.Sleep(100); // Затримка для наочності
    }
});
thread2.Start();

// Головний потік
for (int i = 0; i < 35; i++)
{
    lock (obj)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Hello in main");
        Console.ResetColor();
    }
    Thread.Sleep(100); // Затримка для наочності
}

// функція, що виконується в окремому потоці
void PrintLetters(object end)
{
    for (char c = 'A'; c <= (char)end; c++)
    {
        lock (obj) // Блокування для синхронізації доступу
        {
            // Критична секція = частина коду, яка виконується лише одним потоком одночасно
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\t\t{c} in thread");
            Console.ResetColor();
        }
        Thread.Sleep(100); // Затримка для наочності
    }
}
