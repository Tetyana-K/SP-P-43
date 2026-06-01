// успадкований код - це код, який був створений раніше і використовується в новому проекті або програмі.
//P / Invoke(Platform Invocation Services) — це механізм у .NET, який дозволяє викликати некерований код (наприклад, функції з нативних бібліотек Windows DLL) із керованого коду C#.


//Керований код(managed code) — це код, який виконується під контролем CLR (.NET Common Language Runtime), наприклад звичайний C#.
//Некерований код (unmanaged code) — це код поза CLR: бібліотеки Windows(user32.dll, kernel32.dll), С/С++ DLL, COM-об’єкти.
//P/Invoke — це міст між ними.
//P/Invoke використовується для виклику функцій із нативних бібліотек, які не мають керованих обгорток.

using System.Runtime.InteropServices; // Простір імен для P/Invoke

//Приклад(виклик MessageBox із Windows API):
class Program
{
    // Імпорт функції MessageBox із user32.dll
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    public static extern bool Beep(uint freq, uint duration);
    static void Main()
    {
        // Виклик MessageBox
        MessageBox(IntPtr.Zero, "Hello, P/Invoke!\nHello from Windows API\n", "P/Invoke Example", 0x00000001);// 0); // 0x00000001 - кнопки Ok-Cancel
        if (Beep(750, 300))
        {
            MessageBox(IntPtr.Zero, "Beep was successful", "P/Invoke Example", 0);
        }
        else
        {
            MessageBox(IntPtr.Zero, "Beep failed", "P/Invoke Example", 0);
        }
    }
}
