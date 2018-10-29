using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace WebKo.Model.Settings
{
    public class AppConfiguration
    {
        public static void SetUp()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            Console.WriteLine(configuration.GetSection("ConnectionStrings").GetChildren().Where(c => c.Key == "MsSqlConnectionString").FirstOrDefault().Value);
        }
    }
}
