using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using WebKo.Settings;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var x  = CultureInfo.CurrentCulture;

            AppConfiguration.SetUp();

            var list = JsonConfigurationProvider.GetList;

            Console.ReadLine();
        }
    }
}
