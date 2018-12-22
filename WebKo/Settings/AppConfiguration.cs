using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Reporting.Core.Data;

namespace WebKo.Settings
{
    public class AppConfiguration
    {
        public static void SetUp()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            //Console.WriteLine(configuration.GetSection("ConnectionStrings").GetChildren().Where(c => c.Key == "MsSqlConnectionString").FirstOrDefault().Value);

            var list = configuration.GetChildren().Select(c => new
            {
                Key = c.Key,
                Value = c.Value
            });

            foreach (var item in list)
            {
                JsonConfigurationProvider.SetValue(item.Key, item.Value);
            }

            foreach (var item in configuration.GetSection("ConnectionStrings").GetChildren())
            {
                //CustomConnection.ConnectionStrings.Add(item.Key, item.Value);
            }
        }
    }
}
