// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

string pathChild = "C:\\Users\\Ryzen\\source\\repos\\SP P-43\\child-process\\bin\\Debug\\net9.0-windows\\child-process.exe";
//Process process = Process.Start(
//    new ProcessStartInfo()
//    {
//        FileName = "C:\\Users\\Ryzen\\source\\repos\\SP P-43\\child-process\\bin\\Debug\\net9.0-windows\\child-process.exe",
//        Arguments = "ONE TWO",
//        UseShellExecute = false
//    });

Process process = Process.Start(pathChild, "ONE TWO THREE 4");
Console.WriteLine($"Process started with PID: {process.Id}  ");
process.WaitForExit();
Console.WriteLine($"Child process closed");