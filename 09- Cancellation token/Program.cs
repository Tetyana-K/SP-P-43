// CancelationToken - це механізм для скасування асинхронних операцій у C#.

// Створюємо джерело токена
CancellationTokenSource cts = new CancellationTokenSource();


// Передаємо токен у завдання
Task task = Task.Run(() =>
{
    bool flowControl = ProcessMethod(cts);
    if (!flowControl)
    {
        return;
    }
}, cts.Token);
TaskStatus tstate = task.Status;
Console.WriteLine($"Task status: {task.Status}");

// Через 2 секунди скасовуємо завдання
Thread.Sleep(2000);
Console.WriteLine($"Task status: {task.Status}");
cts.Cancel();
Thread.Sleep(2000);
Console.WriteLine($"Task status after task cancel: {task.Status}");

task.Wait(); // чекаємо завершення завдання
Console.WriteLine("Main thread finished.");

bool ProcessMethod(CancellationTokenSource cts) // cts - джерело токена передаємо в метод, якщо потрібно перевіряти таску (токен) на скасування
{
    for (int i = 1; i <= 10; i++)
    {
        // Перевірка токена
        if (cts.Token.IsCancellationRequested)
        {
            Console.WriteLine("Task was cancelled!");
            return false; // вихід із завдання
        }

        Console.WriteLine($"\t\tProcessing {i}");
        Thread.Sleep(500);
    }

    return true;
}