//ConcurrentDictionary < TKey, TValue > — це потокобезпечний словник (hash map) у C#,
//який дозволяє кільком потокам одночасно читати й записувати дані без lock.

using System.Collections.Concurrent;
//Кожен потік імітує окремого користувача,
//потрібно порахувати скільки разів кожна сторінка була відкрита

ConcurrentDictionary<string, int> visits = new ConcurrentDictionary<string, int>();

string[] pages = { "home", "about", "contact", "products", "blog" };
Random rnd = new Random();
object lockObj = new object();


Thread[] users = new Thread[5];

for (int i = 0; i < users.Length; i++)
{
    int id = i + 1;
    users[i] = new Thread(() => SimulateUser(id));
}

foreach (var t in users)
    t.Start();

foreach (var t in users)
    t.Join();

Console.WriteLine("\n=== FINAL STATISTICS ===");
foreach (var item in visits)
{
    Console.WriteLine($"{item.Key,10} : {item.Value} visits");
}


void SimulateUser(int id)
{
    for (int i = 0; i < 20; i++)
    {
        string page = pages[GetRandom(0, pages.Length)];

        // це атомарна операція:   або створює нову пару, або поновлює значення існуючої пари
        
        visits.AddOrUpdate(
            page,
            1,                      // якщо сторінки ще нема - створюэться нова пара
            (key, oldValue) => oldValue + 1 // якщо вже пара є - поновлюмо лычильник
        );

        Console.WriteLine($"User {id} visited {page}");

        Thread.Sleep(GetRandom(50, 150));
    }
}

int GetRandom(int min, int max)
{
    // об'єкт rnd (Random) НЕ потокобезпечний - тому lock
    lock (lockObj)
    {
        return rnd.Next(min, max);
    }
}
