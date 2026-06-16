//Thread Pool(пул потоків) — це механізм, який дозволяє не створювати новий потік для кожного завдання,
//а використовувати набір уже створених потоків.

//Ідея:
//створення потоку — відносно дорога операція;
//якщо завдань багато і вони короткі, постійно створювати та знищувати потоки неефективно;
//Thread Pool створює певну кількість потоків і повторно використовує їх для різних задач.

//Без Thread Pool
using System.Diagnostics;

int count = 100; // кількість потоків, які хочемо запустити

Thread[] threads = new Thread[count]; // масив потоків

for (int i = 0; i < count; i++) //Тут створиться 100 окремих потоків.
{
    int id = i;

    threads[id] = new Thread(() => // власне створюємо потоки, у потоці викликається лямбда
    {
        Console.WriteLine($"Потік {id} : ManagedId #{Thread.CurrentThread.ManagedThreadId}");
    });
    threads[id].Start(); // запустили потік
}
for (int i = 0; i < threads.Length; i++)
{
    threads[i].Join(); // дочекалися завершення потоків
}


Console.ReadLine();
// виконуємо щось подібне через пул потоків ThreadPool

for (int i = 0; i < count; i++)
{
    int id = i;

    ThreadPool.QueueUserWorkItem(_ => // задачі ставляться у чергу і запускаються
    {
        Console.WriteLine($"Задача {id} : ManagedId #{Thread.CurrentThread.ManagedThreadId}");

    });
}
Console.ReadLine();


//ThreadPool.QueueUserWorkItem(...) - НЕ створює потік явно, як new Thread(...).Start(), натомість:

    //додає задачу в чергу пулу;
    //Thread Pool знаходить вільний потік;
    //потік виконує делегат.

//Уявно це виглядає так:

//Main
// │
// ├─ QueueUserWorkItem(1)
// ├─ QueueUserWorkItem(2)
// ├─ QueueUserWorkItem(3)
// │
// ▼
//Черга ThreadPool
// │
// ├─ Потік #5 виконує задачу 1
// ├─ Потік #8 виконує задачу 2
// └─ Потік #5 виконує задачу 3
