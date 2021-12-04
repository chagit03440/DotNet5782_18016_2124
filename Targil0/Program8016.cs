using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome8016();
            Welcome1867();
            Console.ReadKey();
        }
        static partial void Welcome1867();

        static void Welcome8016()
        {
            Console.WriteLine("Enter your name: ");
            string username = Console.ReadLine();
            Console.WriteLine($"{username}, welcome to my first console application");
        }
    }
}
