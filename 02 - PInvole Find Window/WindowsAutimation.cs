using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___PInvole_Find_Window
{
    // Клас для спрощення роботи з вікнами Windows за допомогою P/Invoke
    class WindowAutomation
    {
        public void SetWindowTitle(IntPtr hwnd, string title)
        {
            if (hwnd == IntPtr.Zero) return;

            NativeMethods.SetWindowText(hwnd, title); // викликаємо імпортовану функцію для зміни заголовка вікна
            Console.WriteLine("Заголовок змінено");
        }

        public void CloseWindow(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero) return;

            NativeMethods.SendMessage(hwnd, NativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            Console.WriteLine("Вікно закрито");
        }

        // Метод для запису тексту в Notepad
        public void WriteTextToNotepad(string text)
        {
            IntPtr notepad = NativeMethods.FindWindow("Notepad", null);

            if (notepad == IntPtr.Zero)
            {
                Console.WriteLine("Notepad не знайдено");
                return;
            }

            // Знаходимо дочірній елемент класу Edit, який відповідає за текстове поле в Notepad
            IntPtr edit = NativeMethods.FindWindowEx(notepad, IntPtr.Zero, "Edit", null);

            if (edit == IntPtr.Zero)
            {
                Console.WriteLine("Edit control не знайдено");
                return;
            }
            // Відправляємо повідомлення WM_SETTEXT для встановлення тексту в Edit control
            NativeMethods.SendMessage(edit, NativeMethods.WM_SETTEXT, IntPtr.Zero, text);

            Console.WriteLine("Текст записано в Notepad");
        }
    }
}
