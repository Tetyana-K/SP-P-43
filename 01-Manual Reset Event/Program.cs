// ManualResetEvent, AutoResetEvent - це  сигнальні події (event) у багатопотоковому середовищі.
//Потоки можуть очікувати сигнал або передавати сигнал іншим потокам.


//Використовуються для координації потоків, щоб один потік чекав на завершення іншого або на певну умову.

/*Як це працює (ідея)
    Потік A: виконує роботу
    Потік B: чекає(WaitOne)
    Потік A: подає сигнал(Set)
    Потік B: продовжує виконання
*/

//-----------------ManualResetEvent
//Коли викликаємо Set(), ВСІ ПОТОКИ, що чекають, розблоковуються.
//Потік не зникає після сигналу — подія залишається встановленою, поки не викличемо Reset().
//Підходить, коли кілька потоків мають чекати одну і ту ж подію


ManualResetEvent mre = new ManualResetEvent(false); // створення події, яка спочатку не встановлена (не сигналізує)

Console.OutputEncoding = System.Text.Encoding.UTF8;

Thread t = new Thread(Worker); // створили потік, у якому буде запускатися функція Worker()
t.Start(); // запускається потік t

Console.WriteLine("Головний потік робить щось...");
Thread.Sleep(2000);

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("Головний потік сигналізує робітнику!");
Console.ResetColor();
mre.Set(); // голоний потік випускає( ініціює) сигнал для робітника
t.Join();
Console.WriteLine("Програма завершена.");


void Worker()
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("Робітник чекає сигнал...");
    mre.WaitOne(); // чекаємо сигналу
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("Робітник отримав сигнал і працює далі!");
    Console.ResetColor();
}