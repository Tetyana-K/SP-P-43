Console.WriteLine("__________________Task throws exception and task.Wait()");

var task1 = Task.Run(() => // запускаємо таску з функцією PrintHelloWithException(), яка кидає exception
{
    PrintHelloWithException();
});

try
{
    task1.Wait(); // очікуємо завершення таски. Якщо в тасці виникне exception, він буде обгорнутий в AggregateException
}

catch (AggregateException ex) // Wait() обгортає exception в AggregateException
{
    foreach (var inner in ex.InnerExceptions) // InnerExceptions - колекція всіх exception, які виникли в тасці
    {
        Console.WriteLine($"Exception: {inner.Message}");
    }
}
Console.WriteLine();
Console.ReadKey();

Console.WriteLine("\n__________________Task throws exception and  await task");
var task2 = Task.Run(() =>
{
    Console.WriteLine($"Enter to task");
    Console.WriteLine($"Task throws InvalidOperationException");
    throw new InvalidOperationException("Exception from task with lambda");
});
try
{
    await task2; // Await не обгортається в AggregateException як Wait() та кидається оригінальний  exception (тут InvalidOPertaionException)
}
catch (InvalidOperationException ex) // обробляємо оригінальний exception
{
    Console.WriteLine($"Exception: {ex.Message}");
}
Console.WriteLine();
Console.ReadKey();


Console.WriteLine($"\n_______________Several exceptions in tasks");

Task task3 = Task.Run(() => throw new Exception("Error in task #3"));
Task task4 = Task.Run(() => throw new Exception("Error in task #4"));

try
{
    Task.WaitAll(task3, task4);
}
catch (AggregateException ex) // WaitAll обгортає всі exception в AggregateException
{
    Console.WriteLine("We have several errors:");
    foreach (var inner in ex.InnerExceptions)
    {
        Console.WriteLine("\t" + inner.Message);
    }
}

Console.WriteLine("\n________________Fault in Task  and handling by ContinueWith");
Task task5 = Task.Run(() =>
{
    Console.WriteLine("In task #5");
    throw new Exception("Error in task # 5");
})
.ContinueWith(t => // t = це task5, попередня таска
{
    if (t.IsFaulted)
    {
        Console.WriteLine($"Error handling in  ContinueWith: {t?.Exception?.InnerException?.Message}");
    }
    else
    {
        Console.WriteLine("Continuation task : Task #5 completed successfully.");
    }
});

void PrintHelloWithException()
{
    Console.WriteLine("Hello");
    throw new Exception("Test exception from Print Hello function");
}