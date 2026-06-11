using System;
using System.Threading;

class Program
{
    static int counter = 0;
    static object locker = new object(); // створили об'єкт для блокування

    static void Main()
    {
        Thread t1 = new Thread(Increment);
        Thread t2 = new Thread(Increment);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine($"The end - Counter = {counter}");
    }

    static void Increment()
    {
        for (int i = 0; i < 100000; i++)
        {
            Monitor.Enter(locker); // захоплюємо блокування
            try
            {
                counter++;
            }
            finally
            {
                Monitor.Exit(locker); // завжди звільняємо блокування
            }
        }
    }
}
// Monitor - це більш гнучкий механізм синхронізації в порівнянні з lock
// lock - це синтаксичний цукор над Monitor, який автоматично виконує захоплення і звільнення блокування
// lock розкладається на Monitor.Enter і Monitor.Exit
// основна перевага Monitor полягає в тому, що він дозволяє більш тонко контролювати процес блокування і розблокування
// наприклад, можна використовувати Monitor.Wait і Monitor.Pulse для реалізації складніших сценаріїв синхронізації
// однак, використання Monitor вимагає більшої обережності, оскільки потрібно вручну забезпечити звільнення блокування в блоці finally
// у цьому прикладі ми використовуємо Monitor.Enter для захоплення блокування на об'єкті locker перед збільшенням counter
