// створюємо м`ютекс
using System.Diagnostics;

// У пркладі використано м'ютекс для синхорнізації між  процесами
Mutex mutex = new Mutex(
    false, // початково не захоплений
    "Global\\LogFileMutex"); // унікальне ім`я для міжпроцесного м`ютекса

// Спробуємо захопити м`ютекс
mutex.WaitOne();
try
{
    using (StreamWriter writer = new StreamWriter("log.txt", true))
    {
        writer.WriteLine($"{DateTime.Now} :: PID {Process.GetCurrentProcess().Id}");// або так Environment.ProcessId;
        Thread.Sleep(5000); // імітуємо довгу операцію запису в файл
    }
}
finally
{
    mutex.ReleaseMutex(); // звільняємо м`ютекс
}