// Створюємо AutoResetEvent у несигнальному стані (false)
// Це означає: потоки будуть чекати сигналу Set()

AutoResetEvent are = new AutoResetEvent(false); 
//ManualResetEvent are = new (false); //-- для порівняння роботи

// Створюємо та запускаємо потоки
for (int i = 1; i <= 3; i++)
    new Thread(Worker).Start(i); //  Кожен потік запускає метод Worker і отримує свій id (1,2,3)

Thread.Sleep(2000);

Console.WriteLine("\n--- SET 1 ---");
are.Set();  // Подаємо сигнал: AutoResetEvent розблокує ТІЛЬКИ 1 потік

Thread.Sleep(2000);// для демонтсрації, щоб побачити щоодин із потоків працював

Console.WriteLine("\n--- SET 2 ---");
are.Set(); //  Другий сигнал -  розблокує ще 1 потік

Thread.Sleep(2000);

Console.WriteLine("\n--- SET 3 ---");
are.Set(); // ще 1 потік



void Worker(object? id)
{
    Console.WriteLine($"Client {id} ЧЕКАЄ");
    are.WaitOne(); // чекаємо сигналу для входу

    Console.WriteLine($"Client {id} УВІЙШОВ");
}
