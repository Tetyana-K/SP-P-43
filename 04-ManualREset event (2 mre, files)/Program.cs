
Console.WriteLine("How many numbers : ");
int quantity = int.Parse(Console.ReadLine() ?? "0");

ManualResetEvent mreGenerate = new ManualResetEvent(false); // створення події, яка спочатку не встановлена (не сигналізує)
ManualResetEvent mrePrint = new ManualResetEvent(false); // створення події, яка спочатку не встановлена (не сигналізує)

object consoleLock = new object(); // об'єкт для блокування доступу до консолі

Thread t1 = new Thread(() => GenerateNumbersFile("numbers1.txt", quantity));
Thread t2 = new Thread(() => SumNumbersInFile("numbers1.txt"));
Thread t3 = new Thread(() => CounPositiveNumbersInFile("numbers1.txt"));
Thread t4 = new Thread(() => PrintFile("numbers1.txt"));


t1.Start();
t2.Start();
t3.Start();
t4.Start();
void GenerateNumbersFile(string filename, int count)
{
    Random rand = new Random();
    using (StreamWriter sw = new StreamWriter(filename))
    {
        for (int i = 0; i < count; i++)
        {
            int number = rand.Next(-1000, 1000);
            sw.WriteLine(number);
        }
    }
    mreGenerate.Set(); // даємо сигнал, що файл створено
}
void PrintFile(string filename)
{
    mreGenerate.WaitOne(); // чекаємо сигналу, що файл створено
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
        //Console.WriteLine(File.ReadAllText(v));
        var lines = File.ReadAllLines(filename);
        foreach (var item in lines)
        {
            Console.WriteLine(item);
            Thread.Sleep(100);
        }
    }
    mrePrint.Set(); // даємо сигнал, що файл виведено
}
void SumNumbersInFile(string filename)
{
    mrePrint.WaitOne(); // чекаємо сигналу, що файл створено
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
    mrePrint.WaitOne(); // чекаємо сигналу, що файл створено
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