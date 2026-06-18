using System.Threading;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("\n=== Масив Thread ===");

int threadNumber = 5;
Thread[] threads = new Thread[threadNumber];

for (int i = 0; i < threads.Length; i++)
{
    int id = i + 1;

    threads[i] = new Thread(() =>
    {
        Console.WriteLine($"Потік {id} ::{Thread.CurrentThread.ManagedThreadId} почав роботу");
        Thread.Sleep(2000);
        Console.WriteLine($"\tПотік {id} завершив роботу");
    });

    threads[i].Start();
}

// чекаємо всі потоки
foreach (Thread t in threads)
{
    t.Join();
}

Console.WriteLine("Усі потоки завершені");
Console.ReadLine();

Console.WriteLine("=== ThreadPool ==="); /* зручно запускати виконання завдань,
 але самі:
   - створюємо ManualResetEvent
   - сигналізуємо завершення
   - чекаємо через WaitHandle
 багато ручної роботи
 */

WaitHandle[] handles = new WaitHandle[threadNumber];
for (int i = 0; i < threadNumber; i++)
{
    int id = i + 1;

    ManualResetEvent done = new ManualResetEvent(false); // для події завершення роботи
    handles[i] = done;

    ThreadPool.QueueUserWorkItem(_ =>
    {
        Console.WriteLine($"Work {id}::{Thread.CurrentThread.ManagedThreadId} почав працювати");

        Thread.Sleep(2000);

        Console.WriteLine($"\nWork {id} завершив роботу");

        done.Set(); // потік подає сигнал про завершення роботи
    });
}

// чекаємо всі задачі вручну
WaitHandle.WaitAll(handles);


Console.WriteLine("\n=== Масив Task ===");
Task[] tasks = new Task[threadNumber];

for (int i = 0; i < tasks.Length; i++)
{
    int id = i + 1;

    tasks[i] = Task.Run(() =>
    {
        Console.WriteLine($"Task {id} ::{Thread.CurrentThread.ManagedThreadId} почав роботу");

        Thread.Sleep(2000);

        Console.WriteLine($"\tTask {id} завершив роботу");
    });
}

Task.WaitAll(tasks); // чекаємо завершення усіх завдань, потік блокується

Console.WriteLine("Усі Task завершені");

//ThreadPool не те саме, що  Task
//Task — це НЕ інший потік, а надбудова
/* Чому Task з`явився
Бо в ThreadPool треба було вручну:
    чекати
    сигналізувати
    синхронізувати
*/

/*
Task — це високорівнева абстракція над асинхронними операціями, яка:
- планує та відстежує виконання коду (найчастіше у ThreadPool);
- дозволяє гнучко чекати завершення (через Wait() або неблокуючий await);
- може повертати результат (Task<T>);
- підтримує ланцюжки продовжень (continuations через .ContinueWith або await);
- вміє обробляти та прокидати виключення (Exceptions).

Аналогія
Thread = окремий працівник
ThreadPool = бригада працівників
Task = замовлення в черзі (опис роботи)

** «Прокинути виняток» (throw або rethrow an exception) — це процес, коли програма виявляє помилку в одному місці
(наприклад, глибоко всередині якогось методу), але не обробляє її там, 
а «передає» (прокидає) вище по ланцюжку викликів — туди, де цей метод був викликаний.
__________________________________________________
!!!Task — керує виконанням операції, часто через  ThreadPool
 Тобто  Task може виконуватися і без ThreadPool. 
!!! Наприклад, чисто асинхронні I/O-операції (запит до БД чи читання файлу через Task) під час очікування 
взагалі не займають жодного потоку з ThreadPool. 
Вони використовують апаратні переривання ОС. 
Також завдання можна запустити на окремому потоці (повз пул), використавши прапорець TaskCreationOptions.LongRunning.


Task — це об’єкт у .NET (простір імен System.Threading.Tasks), який є абстракцією над 
асинхронною операцією, дозволяє керувати її життєвим циклом, отримувати результат 
та обробляти помилки без жорсткої прив'язки до конкретного фізичного потоку.*/