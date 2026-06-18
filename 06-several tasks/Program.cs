double[] arr = new double[] { 1, 2, 3.5, 4, 5, 6, 7, 8, 9 };


Task<double>[] tasks = new Task<double>[2];
tasks[0] = Task.Run(() => Sum(arr));
tasks[1] = Task.Run(() => Product(arr));

// Демонстрація початкового стану (таски  запустилися)
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Статус Task 0 (Sum) на старті: {tasks[0].Status}");
Console.WriteLine($"Статус Task 1 (Product) на старті: {tasks[1].Status}");
Console.ResetColor();

double[] results = await Task.WhenAll(tasks); // чекаємо, не  блокуючи потік

// Демонстрація фінального стану (робота завершена)
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"\nСтатус Task 0 після завершення: {tasks[0].Status}");
Console.WriteLine($"Статус Task 1 після завершення: {tasks[1].Status}");
Console.ResetColor();

Console.WriteLine($"\nResult of task 0 (sum) : {results[0]}");     // 45.5
Console.WriteLine($"Result of task 1 (product) : {results[1]}"); // 362880
double Product(double[] array)
{
    double product = 1;
    foreach (var item in array)
    {
        product *= item;
        Thread.Sleep(100); // Затримка для наочності
    }
    return product;
}
double Sum(double[] array)
{
    double sum = 0;
    foreach (var item in array)
    {
        sum += item;
        Thread.Sleep(100); // Затримка для наочності
    }
    return sum;
}
// Ми не робимо ці методи асинхронними (через async Task<double> та await Task.Delay), тому що вони виконують CPU-bound роботу
/*
 * Асинхронність (async / await) створена для очікування зовнішніх подій (I/O-bound операцій): 
       поки прийде пакет із мережі, поки диск прочитає файл. 
У цей момент процесор відпочиває. Натомість обхід масиву, додавання та множення чисел — це чиста робота для процесора. 
Процесор не може «асинхронно чекати» результат додавання $2 + 2$, він повинен фізично виконати цю команду в колі за колом.
Thread.Sleep(100) у нашому випадку якраз імітує те, що потік зайнятий реальними обчисленнями.*/