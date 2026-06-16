using System.Collections.Concurrent;
using System.Threading;

//ConcurrentStack працює як стек (LIFO) 
//thread - safe push / pop
//синхронізація відбувається автоматично.

// У програмі

//Worker  додає дії (Push у стек)
//Undo  скасовує дії (TryPop зі стеку)
//ConcurrentStack працює як історія я дій (LIFO)
//SafeWrite  - для  потокобезпечного виводу на косоль з кольором

class Program
{
    static ConcurrentStack<string> actions = new ConcurrentStack<string>();
    // Last in first out
    static string[] possibleActions = // масив можливих дій користувача (імітація текстового редактора)
    {
        "Write text",
        "Delete line",
        "Paste content",
        "Format paragraph",
        "Insert image"
    };

    static Random rnd = new Random();
    
    static object rndLock = new object(); //  об’єкт для синхронізації доступу до Random
    static object consoleLock = new object();  // об’єкт для синхронізації виводу в консоль

    // прапорець завершення роботи потоків, які виконують дії (producers)
    // volatile -> щоб всі потоки бачили актуальне значення
    static volatile bool producersFinished = false;

    static void Main()
    {
        Thread[] workers = new Thread[3];  // масив потоків, які створюють дії (producer-и)
        Thread[] undoWorkers = new Thread[2];// масив потоків, які скасовують дії (undoworkers-и)

        // cтворення потоків, які створюють дії
        for (int i = 0; i < workers.Length; i++)
        {
            int id = i + 1;
            workers[i] = new Thread(() => DoActions(id)); // створили потоки з функцію DoAction
        }

        // cтворення потоків, які скасовують дії
        for (int i = 0; i < undoWorkers.Length; i++)
        {
            int id = i + 1;
            undoWorkers[i] = new Thread(() => UndoActions(id));
        }

        foreach (var t in workers) t.Start();
        foreach (var t in undoWorkers) t.Start();

        foreach (var t in workers) t.Join(); // чекаємо завершення всіх producer-потоків

        // трохи часу, щоб undo щось зробили
        Thread.Sleep(1000);

        Console.WriteLine("\n=== STOP UNDO ===");
        // foreach (var t in undoWorkers) t.Interrupt(); // будить потік і каже зупинись

        // сигнал: кажемо, що  більше не буде нових дій
        producersFinished = true;

        foreach (var t in undoWorkers) t.Join(); // чекаэмо завершення потоків undo

        Console.WriteLine("\nRemaining actions in stack:");
        foreach (var a in actions)
        {
            Console.WriteLine(a);
        }
    }

    // ---------------- ACTIONS ----------------
    static void DoActions(int id)
    {
        for (int i = 0; i < 10; i++)
        {
            string action = possibleActions[GetRandom(0, possibleActions.Length)];

            actions.Push(action); // пушимо випадково обрану команду (WriteText, ...)
            SafeWrite($"Worker {id} -> {action}"); // вивели на екран у кольорі

            Thread.Sleep(GetRandom(100, 200)); 
        }
    }

    // ---------------- UNDO ----------------
    static void UndoActions(int id)
    {
        try
        {
            while (!producersFinished || !actions.IsEmpty) // чи не завершили працювати потоки з DoAction() або доки в стеку є якісь дії
            {
                if (actions.TryPop(out string action)) // намагаємося витягнути із стеку команду (Write Text, ...)
                {

                    SafeWrite($"\tUndo {id} -> {action}", ConsoleColor.Yellow);
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }
        catch (ThreadInterruptedException)
        {
            Console.WriteLine($"\tUndo worker {id} stopped");
        }
    }

    static int GetRandom(int min, int max)
    {
        lock (rndLock)
        {
            return rnd.Next(min, max);
        }
    }
  
    static void SafeWrite( string text, ConsoleColor color = ConsoleColor.Green)
    {
        lock (consoleLock)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}

/*
 volatile - Цю змінну можуть змінювати різні потоки, тому не кешуй її значення — завжди читай напряму з пам’яті.


Без volatile:
один потік може “запам’ятати” значення змінної
і не побачити, що інший потік її змінив

З volatile:
кожне читання = свіже значення
кожен запис = одразу видно іншим потокам
 
 */
