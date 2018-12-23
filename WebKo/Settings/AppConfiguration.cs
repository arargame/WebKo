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

            var provider = configuration.Providers.FirstOrDefault();
            


            var categoryList = configuration.GetChildren().Select(c => new
            {
                Name = c.Key
            }).ToList();

            foreach (var category in categoryList)
            {
                var child = configuration.GetSection(category.Name).GetChildren();

                if (child.Count() > 0)
                {
                    foreach (var item in child)
                    {
                        JsonConfigurationSection.SetValue(item.Key, item.Value, category.Name);
                    }
                }
                else
                {
                    var configurationSection = configuration.GetSection(category.Name);

                    JsonConfigurationSection.SetValue(configurationSection.Key, configurationSection.Value, category.Name);
                }
            }

            //foreach (var item in list)
            //{
            //    JsonConfigurationProvider.SetValue(item.Key, item.Value);
            //}

            foreach (var item in configuration.GetSection("ConnectionStrings").GetChildren())
            {
                //CustomConnection.ConnectionStrings.Add(item.Key, item.Value);
            }
        }
    }
}
