using System;
using System.Runtime.InteropServices; // Простір імен для P/Invoke
                                      //  FindWindow  - Функція для пошуку вікна за класом і заголовком
                                      // user32.dll - бібліотека Windows, яка містить функції для роботи з вікнами та повідомленнями
                                      // SetLastError = true - дозволяє отримувати код помилки, якщо функція не виконується успішно через Marshal.GetLastWin32Error();
                                      // DLLImport - атрибут, який вказує, що метод є зовнішнім і знаходиться в зазначеній бібліотеці
                                      // CharSet.Auto - автоматично вибирає правильну версію функції (ANSI або Unicode) залежно від платформи
                                      // ANSI - використовує 8-бітну кодування символів (старі версії Windows)
                                      // Unicode - використовує 16-бітну кодування символів (сучасні версії Windows)
[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
 static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);
// lpClassName - ім'я класу вікна (може бути null)
// lpWindowName - заголовок вікна (може бути null)

// Функція для надсилання повідомлення у вікно
[DllImport("user32.dll", CharSet = CharSet.Auto)]
 static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
// hWnd - дескриптор вікна
// Msg - повідомлення, яке потрібно надіслати
// wParam, lParam - додаткові параметри повідомлення
// повідомлення можуть бути різними, наприклад WM_CLOSE для закриття вікна, WM_SETTEXT для зміни тексту вікна 

const uint WM_CLOSE = 0x0010; // WM_CLOSE - повідомлення для закриття вікна
//const int WM_SETTEXT = 0x000C;


// Встановлення тексту вікна
[DllImport("user32.dll", CharSet = CharSet.Auto)]
static extern bool SetWindowText(IntPtr hWnd, string lpString);

// Головна функція (top level)

//Знайти вікно за класом вікна або заголовком
//IntPtr hWnd = FindWindow(null, "Калькулятор"); // шукати вікно за заголовком "Калькулятор" (можна використовувати частину заголовка, якщо він унікальний)
//IntPtr hWnd = FindWindow("Windows.UI.Core.CoreWindow", null); // Windows.UI.Core.CoreWindow - клас вікна для багатьох сучасних додатків Windows (UWP)
//IntPtr hWnd = FindWindow("Notepad", null); // "Notepad"  - клас вікна
IntPtr hWnd = FindWindow(null, "Form1"); // "Notepad"  - клас вікна
//IntPtr hWnd = FindWindow(null, "Безымянный – Блокнот"); // "Notepad"  - клас вікна




if (hWnd != IntPtr.Zero) // Якщо вікно знайдено (hWnd не є нульовим вказівником)
{
    SetWindowText(hWnd, "Новий заголовок вікна");
    Console.WriteLine("Вікно знайдено. Міняємо його заголовок...");
    System.Threading.Thread.Sleep(5000); // Зачекати 5 секунд, щоб побачити зміну заголовка


    Console.WriteLine("Закриваємо вікно...");
    System.Threading.Thread.Sleep(2000); // Зачекати 2 секунди, щоб побачити зміну заголовка

    // Надіслати повідомлення для закриття вікна
    SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
}
else
{
    Console.WriteLine("Вікно не знайдено");
}
// Верхнє меню Visual Studio має інструмент Spy++ (Spy Plus Plus)
// Tools-> Spy ++ - це інструмент для перегляду структури вікон Windows, їх класів та заголовків.
// Він допомагає визначити правильні параметри для функції FindWindow, щоб знайти потрібне вікно.