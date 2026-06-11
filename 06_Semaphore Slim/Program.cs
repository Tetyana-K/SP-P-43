// SemaphoreSlim - легковаговий варіант Semaphore, який використовується для обмеження кількості одночасних доступів до ресурсу в межах одного процесу.

SemaphoreSlim semaphoreSlim = new SemaphoreSlim(initialCount: 2, maxCount: 2); // дозволяє одночасний доступ до ресурсу для 2 потоків

for (int i = 1; i <= 5; i++)
{
    int threadId = i; // наше threadId буде від 1 до 5, кожен потік отримає унікальний ідентифікатор для виводу інформації про те, який потік входить і виходить з кімнати
    new Thread(() => AccessRoom(threadId)).Start(); // створюємо і запускаємо потік, який виконує метод AccessRoom, передаючи йому threadId і запускаємо потік (Start())
}

void AccessRoom(int id)
{
    Console.WriteLine($"Thread {id} is waiting to enter the room...");
    semaphoreSlim.Wait(); // зменшує кількість дозволів на 1, якщо дозволів немає, потік буде блокований до тих пір, поки не з'явиться дозвіл
    Console.WriteLine($"\tThread {id} has entered the room.");
    Thread.Sleep(2000); // імітація роботи в кімнаті
    Console.WriteLine($"Thread {id} is leaving the room.");

    semaphoreSlim.Release(); // звільняє місце, збільшує кількість дозволів на 1, дозволяючи іншому потоку зайти в кімнату
}
//    Основні відмінності
//Характеристика	            SemaphoreSlim	    Semaphore
//Для потоків одного процесу	✅	                ✅
//Для різних процесів	        ❌	                ✅
//Швидкодія                 	✅ Вища          	❌ Нижча
//Підтримка async/await	        ✅ WaitAsync()	    ❌ Немає
//Використання ресурсів ОС      Менше	            Більше

/*
 * Semaphore використовує ядровий об'єкт Windows (kernel object).

Коли потік блокується: semaphore.WaitOne();
відбувається перехід у режим ядра ОС, що відносно дорого.

SemaphoreSlim спочатку намагається синхронізувати потоки на рівні .NET без звернення до ядра ОС. 
Лише за потреби використовуються важчі механізми.
 */