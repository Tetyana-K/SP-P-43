
Console.WriteLine("How many numbers : ");
int quantity = int.Parse(Console.ReadLine() ?? "0");

object consoleLock = new object(); // об'єкт для блокування доступу до консолі

ManualResetEvent mre = new ManualResetEvent(false); // створення події, яка спочатку не встановлена (не сигналізує)

// створюєємо потоки
Thread t1 = new Thread(() => GenerateNumbersFile("numbers1.txt", quantity)); // у потоці буде працювати функція генерування чисел та запису їх у файл
Thread t2 = new Thread(() => SumNumbersInFile("numbers1.txt"));
Thread t3 = new Thread(() => CounPositiveNumbersInFile("numbers1.txt"));
Thread t4 = new Thread(() => PrintFile("numbers1.txt"));

int[] arr = new int[10];

// запускаємо потоки
t1.Start();
t2.Start();
t3.Start();
t4.Start();


t1.Join();
t2.Join();
t3.Join();
t4.Join();

Console.WriteLine("All tasks done");
void GenerateNumbersFile(string filename, int count)
{
    
    Random rand = new Random();
    // пишемо у текстовий файл випадкові числа
    using (StreamWriter sw = new StreamWriter(filename))
    {
        for (int i = 0; i < count; i++)
        {
            int number = rand.Next(-1000, 1000);
            sw.WriteLine(number);
        }
    } // неявно працює Dispose() -  закриває файл

    //File.WriteAllLines("", lines);
    mre.Set(); // даємо сигнал, що файл створено
}
void PrintFile(string filename)
{
    mre.WaitOne(); // чекаємо сигналу, що файл створено
    if (!File.Exists(filename))
    {
        lock (consoleLock)
        {
            Console.WriteLine($"File {filename} not found.");
        }
        return;
    }
    lock (consoleLock)
    {
        Console.WriteLine("Numbers in file");
        //Thread.Sleep(1000); // щоб краще було видно
        //Console.WriteLine(File.ReadAllText(filename));
        var lines = File.ReadAllLines(filename);
        foreach (var item in lines)
        {
            Console.WriteLine(item);
            Thread.Sleep(100);
        }
    }
}
void SumNumbersInFile(string filename)
{
    mre.WaitOne(); // чекаємо сигналу, що файл створено
    if (!File.Exists(filename))
    {
        lock (consoleLock)
        {
            Console.WriteLine($"File {filename} not found.");
        }
        return;
    }
    long sum = 0;
    using (StreamReader sr = new StreamReader(filename))
    {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            if (long.TryParse(line, out long number))
            {
                sum += number;
            }
            else
            {
                lock (consoleLock)
                {
                    Console.WriteLine($"Invalid number format: {line}");
                }
            }
        }
    }
    Console.WriteLine($"Sum of numbers in file {filename}: {sum}");
}
void CounPositiveNumbersInFile(string filename)
{
    mre.WaitOne(); // чекаємо сигналу, що файл створено
    if (!File.Exists(filename))
    {
        Console.WriteLine($"File {filename} not found.");
        return;
    }
    int counter = 0;
    using (StreamReader sr = new StreamReader(filename))
    {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            if (long.TryParse(line, out long number))
            {
                if (number > 0)
                    ++counter;
            }
            else
            {
                lock (consoleLock)
                {
                    Console.WriteLine($"Invalid number format: {line}");
                }
            }
        }
    }
    Console.WriteLine($"Quantity of positive numbers in file {filename}: {counter}");
}