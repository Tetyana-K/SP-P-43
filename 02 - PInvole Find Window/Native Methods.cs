using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _02___PInvole_Find_Window
{
    // клас для зберігання P/Invoke методів для роботи з вікнами Windows
    static class NativeMethods 
    {
        public const int WM_SETTEXT = 0x000C; // Код повідомлення для встановлення тексту вікна
        public const int WM_CLOSE = 0x0010; // Код повідомлення для закриття вікна

        // user32.dll містить функції для роботи з вікнами, повідомленнями та іншими елементами інтерфейсу користувача Windows.
        // Dllimport - атрибут, який вказує, що метод є зовнішнім і знаходиться в зазначеній бібліотеці (user32.dll).
        // SetLastError = true дозволяє отримувати код помилки, якщо виклик функції не вдається.
        // CharSet = CharSet.Auto дозволяє автоматично вибирати правильну версію функції (ANSI або Unicode) залежно від платформи.

        // FindWindow - це функція Windows API, яка дозволяє знайти вікно за його класом або заголовком.
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);
        // lpClassName - ім'я класу вікна, яке шукаємо. Якщо не хочемо використовувати цей параметр, передаємо null.
        // lpWindowName - заголовок вікна, яке шукаємо. Якщо не хочемо використовувати цей параметр, передаємо null.

        // FindWindowEx - це функція Windows API, яка дозволяє знайти дочірнє вікно з певними параметрами.

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(
            IntPtr parent, // Дескриптор батьківського вікна. Якщо шукаємо серед верхньорівневих вікон, передаємо IntPtr.Zero.
            IntPtr childAfter, // Дескриптор дочірнього вікна, після якого починається пошук. Якщо передаємо IntPtr.Zero, пошук починається з першого дочірнього вікна.
            string? className, // Ім'я класу вікна, яке шукаємо. Якщо не хочемо використовувати цей параметр, передаємо null.
            string? windowName); // Заголовок вікна, яке шукаємо. Якщо не хочемо використовувати цей параметр, передаємо null.

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hWnd, string text);


        // SendMessage - це функція Windows API, яка дозволяє відправити повідомлення вікну.
        // В даному випадку ми використовуємо її для встановлення тексту вікна (WM_SETTEXT) 
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(
            IntPtr hWnd,
            int msg,
            IntPtr wParam,
            string lParam); // lParam - це параметр, який містить текст, який ми хочемо встановити у вікно. Він передається як рядок.

        // Перевантаження SendMessage для випадку, коли lParam є IntPtr (наприклад, для WM_CLOSE, де lParam не використовується).
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(
            IntPtr hWnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam);
    }
}
