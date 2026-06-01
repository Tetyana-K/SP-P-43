using System.Runtime.InteropServices;

[DllImport("User Func project.dll", CallingConvention = CallingConvention.Cdecl)]
static extern int sum(int a, int b);

int result = sum(5, 70);
Console.WriteLine($"Виклик C++ функції sum(5, 70): {result}");


