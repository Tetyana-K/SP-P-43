// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
/*
 Процесами можна керувати шляхом їх створення, завершення, зміни пріоритету,  очікування завершення. 
Один процес може породжувати інші процеси за допомогою системних викликів.

Батьківський процес може отримувати інформацію про дочірній процес, чекати його завершення та завершувати його.
Дочірні процеси створюються для паралельного виконання, ізоляції задач, підвищення надійності та запуску зовнішніх програм.

 */
Console.OutputEncoding = System.Text.Encoding.UTF8;
try
{
    RunNotepad(); // Запускаємо процес блокнота

    //RunCalc();

    //RunBrowser();
    //RunBrowserWithUrl("https://msdn.com");
    
    //OpenResource("https://w3schools.com"); // Відкриваємо веб-сайт у браузері за замовчуванням
    //OpenResource("my.txt"); // Відкриваємо файл у застосунку  за замовчуванням, тобто у Блокноті (якщо це текстовий файл)
    //OpenResource("C:\\Users\\Ryzen\\source\\repos\\SP P-43\\02-Create process\\02-start and kill process.cs"); // Відкриваємо веб-сайт у браузері за замовчуванням)
    //KillProcess("notepad"); // Завершуємо всі процеси з назвою "notepad" (можна використовувати іншу назву процесу, наприклад "calc" для калькулятора)

}
catch (Exception ex)
{
    Console.WriteLine($"Помилка при запуску процесу: {ex.Message}");
}

void RunNotepad()
{
    try
    {
        var notepad = Process.Start("notepad.exe"); // Запускаємо процес блокнота
        Console.WriteLine($"Блокнот запущено з PID: {notepad.Id}");
        Console.WriteLine($"Назва процесу: {notepad.ProcessName}");
        Console.WriteLine($"Кількість потоків: {notepad.Threads.Count}");
        notepad.PriorityClass = ProcessPriorityClass.High; // Встановлюємо базовий пріоритет процесу (можливі значення від 0 до 31, де 0 - найнижчий пріоритет, а 31 - найвищий)
        notepad.WaitForInputIdle(); // Чекаємо, поки блокнот буде готовий до взаємодії (завантажиться і відобразиться вікно)
        notepad.WaitForExit(); // Чекаємо
        Console.WriteLine($"Блокнот закрито. Exit code {notepad.ExitCode}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка при запуску процесу: {ex.Message}");
    }
}

static void RunCalc()
{
    var calc = Process.Start("calc.exe"); // Запускаємо процес калькулятора
    Console.WriteLine($"\nКалькулятор запущено з PID: {calc.Id}");
    Console.WriteLine($"Назва процесу: {calc.ProcessName}");
    Console.WriteLine($"Час запуску: {calc.StartTime}");
    Console.WriteLine($"Кількість потоків: {calc.Threads.Count}");

    calc.WaitForInputIdle(); // Чекаємо, поки калькулятор буде готовий до взаємодії (завантажиться і відобразиться вікно)
    calc.WaitForExit(); // Чекаємо
    Console.WriteLine($"Калькулятор закрито. Exit code {calc.ExitCode}");
}

static void RunBrowser()
{
    var process = Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"); // Запускаємо процес Google Chrome
    //Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "www.msdn.com"); 

}
static void RunBrowserWithUrl(string url)
{
    // ProcessStartInfo - це клас, який дозволяє налаштувати параметри запуску процесу, такі як ім'я файлу, аргументи, робочий каталог та інші.
    Process.Start(new ProcessStartInfo
    {
        FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
        Arguments = url,
        UseShellExecute = true // Вказуємо, що потрібно використовувати оболонку для запуску процесу
                               // = Запускати програму через Windows (оболонку), як ніби ми двічі клікнули по файлу
    });
}

static void OpenResource(string path)
{
    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true // Вказуємо, що потрібно використовувати оболонку для запуску процесу
        });
        Console.WriteLine($"Ресурс {path} відкрито у програмі  за замовчуванням.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка при відкритті ресурсу {path}: {ex.Message}");
    }
}
// UseShellExecute = true -    Windows вирішує, як запускати
// UseShellExecute = false -   .NET напряму запускає exe

static void KillProcess(string processName)
{
    var processes = Process.GetProcessesByName(processName); // Отримуємо всі процеси з вказаною назвою (наприклад, "notepad" для блокнота)
    FindProcessesByName(processName); // Виводимо інформацію про знайдені процеси

    foreach (var process in processes)
    {
        try
        {
            process.Kill(); // Завершуємо процес
            Console.WriteLine($"\tПроцес {process.ProcessName} з PID {process.Id} завершено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при завершенні процесу {process.ProcessName} з PID {process.Id}: {ex.Message}");
        }
    }
}

static void FindProcessesByName(string processName)
{
    var processes = Process.GetProcessesByName(processName); // Отримуємо всі процеси з вказаною назвою (наприклад, "notepad" для блокнота)
    Console.WriteLine($"Знайдено {processes.Length} процес(ів) з назвою {processName}.");
    foreach (var process in processes)
    {
        try
        {
            Console.WriteLine($"Процес {process.ProcessName} з PID {process.Id} знайдено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при пошуку процесу {process.ProcessName} з PID {process.Id}: {ex.Message}");
        }
    }
}