using _02___PInvole_Find_Window;

var app = new WindowAutomation();

// Приклад 1: знайти  вікно Калькулятор
IntPtr hwnd = NativeMethods.FindWindow(null, "Калькулятор");

if (hwnd != IntPtr.Zero)
{
    Console.WriteLine("Вікно Калькулятор знайдено!");

    app.SetWindowTitle(hwnd, "Новий заголовок");
    System.Threading.Thread.Sleep(3000);

    app.CloseWindow(hwnd);
}
else
{
    Console.WriteLine("Вікно Калькулятор не знайдено.");
}

// Приклад 2: знайти Notepad вікно
 hwnd = NativeMethods.FindWindow("Notepad", null);

if (hwnd != IntPtr.Zero)
{
    Console.WriteLine("Вікно Notepad знайдено!");
    Console.WriteLine("Змінюємо заголовок...");

    app.SetWindowTitle(hwnd, "Мій Новий Заголовок");
    System.Threading.Thread.Sleep(3000);

    Console.WriteLine("Пишемо текст у вікно...");
    app.WriteTextToNotepad("Привіт з C# !!!");
    System.Threading.Thread.Sleep(7000);
    
    Console.WriteLine("Закриваємо вікно...");
    app.CloseWindow(hwnd);
}
else
{
    Console.WriteLine("Вікно Notepad не знайдено.");
}

