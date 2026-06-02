using System.Net.Http.Headers;
using System.Runtime.InteropServices;

[DllImport("User Func project.dll", CallingConvention = CallingConvention.Cdecl)]
static extern int sum(int a, int b);

[DllImport("User Func project.dll", CallingConvention = CallingConvention.Cdecl)]
static extern int product(int a, int b);


int result = sum(5, 70);
Console.WriteLine($"Виклик C++ функції sum(5, 70): {result}");

result = product(5, 70);
Console.WriteLine($"Виклик C++ функції product(5, 70): {result}");
