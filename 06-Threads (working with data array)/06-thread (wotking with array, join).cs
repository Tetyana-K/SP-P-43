// Приклад демонструє використання двох потоків для обчислення суми та добутку елементів масиву

Console.Write("Input size of array: ");
int size = int.Parse(Console.ReadLine()!);
int[] arr = Enumerable.Range(1, size).ToArray(); // створення масиву з числами від 1 до size
Console.WriteLine(String.Join(", ", arr));

int sum =0; // Змінна для збереження суми, буде використовуватися в одному з потоків
long product = 1;

// Створюємо два потоки: один для обчислення суми, інший для обчислення добутку
Thread sumThread = new Thread(() => SumArray(arr)); // Створення потока для обчислення суми
Thread productThread = new Thread(() => ProductArray(arr)); //

sumThread.Start(); // Запуск потока для обчислення суми
productThread.Start(); // Запуск потока для обчислення добутку


// Чекаємо завершення обох потоків
sumThread.Join();  // Чекаємо завершення потока для обчислення суми
productThread.Join(); // Чекаємо завершення потока для обчислення добутку

// Виводимо результати
Console.WriteLine("_________________________");
Console.ForegroundColor = ConsoleColor.DarkMagenta;
Console.WriteLine($"Sum = {sum}");
Console.WriteLine($"Product = {product}");
Console.ResetColor();

void SumArray(int[] arr)
{
    sum = 0;
    foreach (int num in arr)
    {
        sum += num;
        Console.WriteLine($"Sum + {num} = {sum}");

        Thread.Sleep(50); // Затримка для наочності
    }
    // Console.WriteLine($"Sum = {sum}");
}

void ProductArray(int[] arr)
{
    product = 1;
    foreach (int num in arr)
    {
        product *= num;
        Console.WriteLine($"Product * {num} = {product}");
        Thread.Sleep(70); // Затримка для наочності
    }
    //Console.WriteLine($"Product = {product}");

}
