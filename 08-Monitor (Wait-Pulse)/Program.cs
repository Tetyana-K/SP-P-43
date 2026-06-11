// Monitor використовують, коли потрібно не тільки заблокувати ресурс, а й координувати роботу потоків.
// Wait() - звільняє блокування і переводить потік у стан очікування до отримання сигналу.
// Pulse() - посилає сигнал одному з потоків, які очікують на цьому об'єкті, що вони можуть продовжити роботу.
// PulseAll() - посилає сигнал усім потокам, які очікують на цьому об'єкті.
using System.Diagnostics;

object locker = new object();
bool ready = false;

Thread worker = new Thread(Worker); // створили  потік 1
worker.Start();

Thread worker2 = new Thread(Worker); // створили потік 2
worker2.Start();

Thread.Sleep(2000);

lock (locker)
{
    ready = true;
    Monitor.Pulse(locker); // головна  функція будить якийсь один потік ("будимо" потік, який чекає на цьому об'єкті locker)
}

worker.Join();
worker2.Join();
Console.WriteLine($"Виконано!");

void Worker()
{
    lock(locker)
    {
        while (!ready)
        {
            Console.WriteLine($"Worker {Thread.CurrentThread.ManagedThreadId} : очікую сигнал"); ;
            Monitor.Wait(locker); //  чекає, поки інший потік зробить Pulse по цьому locker
        }
        Console.WriteLine($"\tWorker {Thread.CurrentThread.ManagedThreadId} : отримав сигнал, працюю...");
        Monitor.Pulse(locker);
    }
}
