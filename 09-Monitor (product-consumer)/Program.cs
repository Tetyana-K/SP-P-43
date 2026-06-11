Queue<int> queue = new(); // черга з числами, які буде класти туди Виробник, спільний ресурс для потоків
object locker = new();// об'єкт для блокування
Random rnd = new Random();
int stopNumber = -1; // стопове число, ознака завершення для Споживача

Thread producer = new Thread(Producer);
Thread consumer = new Thread(()=>Consumer(1));
Thread consumer2 = new Thread(() =>Consumer(2));

producer.Start();
consumer.Start();
consumer2.Start();

producer.Join();
consumer.Join();
consumer2.Join();

Console.WriteLine("Завершення!");
void Producer()
{
    for (int i = 1; i <= 10; ++i) // 1 2 3 4 5 ... 10
    {
        lock (locker)
        {
            queue.Enqueue(i); // виробник поклав у чергу нове число
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Виробник поклав: {i}");
            Console.ResetColor();
           // Monitor.Pulse(locker); // БУДИМО споживача, якщо він чекає
            Monitor.PulseAll(locker); // БУДИМО всіх споживачів, якщо вони чекають
        }
        Thread.Sleep(rnd.Next(300, 800)); // імітація роботи
    }

    lock (locker) // 
    {
        queue.Enqueue(stopNumber); // виробник кладе у чергу завершуючі числа
        queue.Enqueue(stopNumber);
        Monitor.Pulse(locker);
        Monitor.PulseAll(locker);
    }
}
void Consumer( int id)
{
    int item;
    while (true) // нескінченний цикл, будемо виходити при зчитуванні з черги -1 (завершуючого числа)
    {
        lock (locker)
        {
            while (queue.Count == 0)
            {
                Console.WriteLine($"Черга пуста, споживач {id} чекає...");
                Monitor.Wait(locker); // ЧЕКАЄМО, поки з’явиться елемент
            }
            item = queue.Dequeue();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\t\tСпоживач {id} взяв: {item}");
            Console.ResetColor();
            if (item == stopNumber) break; // вийшли з циклу при зустрічі -1
        }
        Thread.Sleep(rnd.Next(500, 1000)); // імітація обробки

    }

}

