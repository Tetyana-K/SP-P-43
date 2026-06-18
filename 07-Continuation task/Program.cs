// Continuation Task (завдання-продовження) у .NET – це завдання, яке запускається після завершення іншого завдання.
// Тобто воно «підписується» на виконання Task і починається автоматично, коли попереднє завдання закінчилося.

Task<int> firstTask = Task.Run(() => // ініціюємо перше завдання
{
    Console.WriteLine("First task: calculatiog something ... (42)...");
    Thread.Sleep(1000);
    return 42;
});


// Продовження, яке виконується після завершення firstTask
Task continuation = firstTask.ContinueWith(prevTask =>
{
    Console.WriteLine($"Continuation Task received result: {prevTask.Result}"); // Використовуємо результат першого завдання t.Result (42)
    Console.WriteLine($"Continuation Task received result^2: {Math.Pow(prevTask.Result, 2)}"); // Використовуємо результат першого завдання t.Result (42)
});

await continuation; // Очікуємо завершення продовження

//ContinueWith дозволяє обробляти результат (t.Result) або помилки попереднього завдання.

// FluentApi
await Task.Run(() => "\n\nHello")
    .ContinueWith(t => t.Result + " from") // t.Result - це результат попередньої таски ('Hello')  + 'from
    .ContinueWith(t => t.Result + " Continuation Task") // t.result - це результат попередньої таски ('Hello from') + ' Continuation Task'
    .ContinueWith(t => Console.WriteLine(t.Result)); // t.Result - це результат попередньої таски ('Hello from Continuation Task') і роздруковуємо його

//Task<int> originalTask = Task.Run(() =>
//{
//    throw new Exception("Щось пішло не так!");
//    return 42;
//});

//// Це продовження запуститься ТІЛЬКИ у випадку помилки (OnlyOnFaulted)
//originalTask.ContinueWith(prevTask =>
//{
//    Console.WriteLine($"Логгер: Перша таска впала з помилкою: {prevTask.Exception?.InnerException?.Message}");
//}, TaskContinuationOptions.OnlyOnFaulted);

//// А це запуститься ТІЛЬКИ якщо все пройшло успішно (OnlyOnRanToCompletion)
//originalTask.ContinueWith(prevTask =>
//{
//    Console.WriteLine($"Успіх! Результат: {prevTask.Result}");
//}, TaskContinuationOptions.OnlyOnRanToCompletion);