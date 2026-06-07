// ParameterizedThreadStart - делегат, який дозволяє передавати параметр у потік,  delegate void ParameterizedThreadStart(object? obj)

ParameterizedThreadStart threadStart = new ParameterizedThreadStart(PrintLetters); // створення делегата, який посилається на метод PrintLetters, який приймає параметр
Thread thread = new Thread(threadStart); // створення нового потоку, який виконує метод PrintLetters
thread.Name = "thread 1"; // встановлення імені потока для зручності відладки
thread.Start('O');

int left = 1, right = 50;
// створення нового потоку, який виконує анонімну функцію (лямбда-вираз), яка виводить числа від left до right,
// ця анонімна функція не приймає параметрів, тому ми можемо використовувати замикання (closure) для доступу до змінних left і right, які визначені в зовнішній області видимості
Thread thread2 = new Thread(() =>
{
    for (int i = left; i < right; i++) // захоплення змінних left і right у лямбда-виразі, який виконується у потоці thread2
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\t\t\t\t{i} in thread 2");
        Console.ResetColor();

        Thread.Sleep(100); // Затримка для наочності
    }
});
thread2.Start(); // запуск потока thread2, який виконує лямбда-вираз, що виводить числа від left до right

// функція, яка приймає параметр типу object
//   'end' — це параметр, який передається у потік під час виклику thread.Start('Z')
// тип object використовується, бо ParameterizedThreadStart дозволяє передавати лише ОДИН параметр типу object
// у цьому методі ми перетворюємо його на char: (char)end, бо приходить символ, запакований як object
void PrintLetters(object ? end)
{
    for (char c = 'A'; c <= (char)end; c++)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\t\t\t{c} in {Thread.CurrentThread.Name}");
        Console.ResetColor();
        Thread.Sleep(100); //затримка вторинного потоку на 100 мс для наочності
    }
}

// функція, яка приймає два параметри типу char
void PrintSmallLetters(char start, char end)
{
    for (char i = char.ToLower(start); i <= char.ToLower(end); i++)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"\t\t\t\t\t\t{i} in thread {Thread.CurrentThread.Name}");
        Console.ResetColor();
        Thread.Sleep(100); // Затримка для наочності
    }
}
//тут ще немає синхронізації потоків, тому вивід може бути змішаним і не впорядкованим, що є нормальним для багатопотокових програм без синхронізації