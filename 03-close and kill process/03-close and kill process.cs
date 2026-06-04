using System.Diagnostics;

var processes = Process.GetProcessesByName("notepad"); // отримуємо всі процеси з назвою "notepad"

if (processes.Length == 0)
{
    Console.WriteLine("Не знайдено процесу Notepad");
}
else
{
    foreach (var proc in processes)
    {
        Console.WriteLine($"\nСпроба закрити процес: {proc.ProcessName}, PID: {proc.Id}");

        // 1. М'яке закриття (Close)
        if(proc.CloseMainWindow()) // надсилаємо запит на закриття головного вікна процесу
        {
            if (proc.WaitForExit(5000)) // чекаємо до 5 секунд, поки процес завершиться
            {
                Console.WriteLine($"Процес {proc.ProcessName} (PID: {proc.Id}) успішно закрито.");
            }
            else
            {
                Console.WriteLine($"Процес {proc.ProcessName} (PID: {proc.Id}) не відповідає на запит закриття. Спроба примусового завершення...");
                TryKillProcess(proc);
            }
        }
        else
        {
            Console.WriteLine($"Не вдалося надіслати запит на закриття процесу {proc.ProcessName} (PID: {proc.Id}). Спроба примусового завершення...");
            TryKillProcess(proc);
        }
    }
    static void TryKillProcess(Process proc)
    {
        try
        {
            proc.Kill();
            proc.WaitForExit();
            Console.WriteLine($"Процес {proc.ProcessName} (PID: {proc.Id}) примусово завершено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при завершенні процесу: {ex.Message}");
        }
    }
}