using System;
using System.Collections.Concurrent;
using System.Threading;

class Program
{
    static ConcurrentQueue<int> queue = new ConcurrentQueue<int>(); // Потокобезпечна черга, тут  використовується
                                                                    // для зберігання цілих чисел, які будуть додаватися виробником і оброблятися споживачами.
    // Вона дозволяє одночасний доступ з кількох потоків без необхідності додаткової синхронізації.
    // Це забезпечує безпечне додавання та видалення елементів з черги в багатопотоковому середовищі.

    static Random rand = new Random();
    static object randLock = new object();
    static int GetRandom(int min, int max)
    {
        lock (randLock)
        {
            return rand.Next(min, max);
        }
    }
    static void Main()
    {
        // Створюємо виробника
        Thread producer = new Thread(() =>
        {
            int time;
            for (int i = 1; i <= 10; i++)
            {
                queue.Enqueue(i); // Додає елемент у кінець потокобезпечної черги
                Console.WriteLine($"Producer added: {i}");
                time = GetRandom(100, 300);
                Thread.Sleep(time);
            }
        });

        // Створюємо два споживача
        Thread consumer1 = new Thread(() => ConsumeQueue(1));
        Thread consumer2 = new Thread(() => ConsumeQueue(2));

        producer.Start();
        consumer1.Start();
        consumer2.Start();

        producer.Join();
        consumer1.Join();
        consumer2.Join();

        Console.WriteLine("All done!");
    }
    static object lockObj = new ();
    static void ConsumeQueue(int id)
    {
        int r;
        while (!queue.IsEmpty)
        {
            if (queue.TryDequeue(out int item)) // намагаємося видалити елемент з початку черги
            {
                Console.WriteLine($"\tConsumer {id} processed: {item}");
                int time = GetRandom(200, 400); 
                Thread.Sleep(time);
            }
        }
    }
}
/*
 Виробник додає числа 1–10 у чергу.
Два споживача паралельно витягують числа через TryDequeue.

Ніяких блокувань (lock) не потрібно — ConcurrentQueue  синхронізована.
*/

/*
 Коли що використовувати
Колекція	Використання
ConcurrentQueue	черги задач
ConcurrentStack	undo/stack логіка
ConcurrentDictionary	кеші, словники
BlockingCollection	producer-consumer
ConcurrentBag	результати без порядку
 
 */