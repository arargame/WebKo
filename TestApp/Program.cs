using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using WebKo.Model.Settings;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            AppConfiguration.SetUp();

            Console.ReadLine();
        }
    }
}
