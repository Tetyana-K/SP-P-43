
//Без Thread Pool
using System.Diagnostics;

int count = 100;
Stopwatch sw = Stopwatch.StartNew(); // об'єкт для вимірювання часу -  StartNew() починає рахувати час
Thread[] threads = new Thread[count];

for (int i = 0; i < count; i++) //Тут створиться 100 окремих потоків.
{
    int id = i; // Локальна копія змінної, щоб кожен потік отримав своє значення i

    threads[id] = new Thread(() =>
    {
        Console.WriteLine($"Потік {id} : ManagedId #{Thread.CurrentThread.ManagedThreadId}");
    });
    threads[id].Start(); // самі запускаємо
}
for (int i = 0; i < threads.Length; i++)
{
    threads[i].Join(); // самі робимо join 
}
sw.Stop(); //  зупиняємо таймер вимірювання часу

Console.WriteLine($"Running time : {sw.Elapsed.Seconds} {sw.Elapsed.Milliseconds}");
Console.WriteLine($"Running time (ms): {sw.Elapsed.TotalMilliseconds} ");
Console.ReadLine();

Stopwatch sw2 = Stopwatch.StartNew();
CountdownEvent done = new CountdownEvent(count); // лічильник потоків-задач
for (int i = 0; i < count; i++)
{
    int id = i;

    ThreadPool.QueueUserWorkItem(_ =>
    {
        try
        {
            Console.WriteLine($"Задача {id} : ManagedId #{Thread.CurrentThread.ManagedThreadId}");
        }
        finally // гаарнтія, що лічильник зменшиться 
        {
            done.Signal(); // повідомляємо про завершення задачі
        }
    });
}
done.Wait();   // Чекаємо, поки всі count задач завершаться (чекаємо поки пул потоків повиконує усі задачі)


sw2.Stop();
Console.WriteLine($"Running time : {sw2.Elapsed.Seconds} {sw2.Elapsed.Milliseconds}");
Console.WriteLine($"Running time : {sw2.Elapsed.TotalMilliseconds}");
Console.ReadLine();

