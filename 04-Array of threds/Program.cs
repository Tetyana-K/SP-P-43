int numThreads = 3;

// створення масиву потоків
Thread[] threads = new Thread[numThreads];

char startLetter = 'A', endLetter = 'M';

// розподіл діапазону букв між потоками
int size = (endLetter - startLetter + 1) / numThreads; // кількість букв на потік
Console.WriteLine($"Each thread will process {size} letters.");

for (int i = 0; i < numThreads; i++)
{
    char localStart = startLetter; // локальна копія початкової букви для потоку
    char localEnd = (char)(localStart + size - 1); // локальна копія кінцевої букви для потоку
    // останній потік отримує всі залишкові букви
    if (i == numThreads - 1)
    {
        localEnd = endLetter;
    }
    Console.WriteLine($"Start letter {localStart}, End letter {localEnd} for thread #{i}");
    threads[i] = new Thread(() => PrintLetters(localStart, localEnd)); // використання лямбда-виразу для передачі параметрів у потік
    startLetter = (char)(localEnd + 1); // оновлення початкової букви для наступного потоку
}

for (int i = 0; i < numThreads; i++) // запуск усіх потоків
{
    threads[i].Start();
}

void PrintLetters(char start, char end)
{
    for (char c = start; c <= end; c++)
    {
        Console.WriteLine($"\t\t{c} in thread {Thread.CurrentThread.ManagedThreadId}"); // ManagedThreadId - унікальний ідентифікатор потока, який присвоюється операційною системою при створенні потока
        Thread.Sleep(100); // Затримка для наочності
    }
}
