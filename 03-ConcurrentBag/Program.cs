/*   ConcurrentBag<T> =  колекція, яка дозволяє зберігати елементи в довільному порядку і забезпечує потокобезпечний доступ до них.
     Це колекція 'bag' (мішок), яка дозволяє паралельний доступ без блокувань.
     Порядок елементів не гарантується.
     Може використовуватись як склад для тимчасових об’єктів, кеш, або збір необроблених завдань.
     На відміну від ConcurrentQueue (FIFO) і ConcurrentStack (LIFO), ConcurrentBag просто зберігає елементи 
    і роздає їх будь-яким потоком, без певного порядку.

Методи
Add(item)	Додає елемент у колекцію
TryTake(out item)	Витягує елемент (повертає false, якщо порожньо)
TryPeek(out item)	Показує елемент, не видаляючи його
IsEmpty	Перевіряє, чи порожня колекція
*/

using System.Collections.Concurrent;

class Program
{
    static ConcurrentBag<int> bag = new ConcurrentBag<int>();

    static int producerCount = 3;
    static int consumerCount = 3;
    static int itemsPerProducer = 5;

    static bool producersFinished = false;

    static object lockObj = new object();
    static Random globalRand = new Random();

    static void Main()
    {
        Thread[] producers = new Thread[producerCount];
        Thread[] consumers = new Thread[consumerCount];

        // ---------------- PRODUCERS ----------------
        for (int i = 0; i < producerCount; i++)
        {
            int id = i + 1; // щоб не було пастки лямбди, нова локальна змінна 
            producers[i] = new Thread(() => Producer(i)); // пастка = лямбда захоплюється за посиланням, і потоки бачать ожнакове значення i 
        }

        // ---------------- CONSUMERS ----------------
        for (int i = 0; i < consumerCount; i++)
        {
            int id = i + 1;
            consumers[i] = new Thread(() => Consumer(id));
        }

        foreach (var t in producers) t.Start();
        foreach (var t in consumers) t.Start();

        foreach (var t in producers) t.Join();

        producersFinished = true;

        foreach (var t in consumers) t.Join();

        Console.WriteLine("All done!");
    }

    // ---------------- PRODUCER METHOD ----------------
    static void Producer(int id)
    {
        Random rnd;

        lock (lockObj)
        {
            rnd = new Random(globalRand.Next());
        }

        for (int i = 0; i < itemsPerProducer; i++)
        {
            int item = rnd.Next(1, 100);
            bag.Add(item);

            Console.WriteLine($"Producer {id} -> {item}");

            Thread.Sleep(rnd.Next(100, 250));
        }
    }

    // ---------------- CONSUMER METHOD ----------------
    static void Consumer(int id)
    {
        while (!producersFinished || !bag.IsEmpty)
        {
            if (bag.TryTake(out int item))
            {
                Console.WriteLine($"\tConsumer {id} processed {item}");
            }
            else
            {
                Thread.Sleep(50);
            }
        }

        Console.WriteLine($"\tConsumer {id} finished");
    }
}
/*
 volatile - Цю змінну можуть змінювати різні потоки, тому не кешуй її значення — завжди читай напряму з пам’яті.


Без volatile:
один потік може “запам’ятати” значення змінної
і не побачити, що інший потік її змінив

З volatile:
кожне читання = свіже значення
кожен запис = одразу видно іншим потокам
 
 */