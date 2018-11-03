using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebKo.Data
{

    /// <summary>
    /// https://docs.microsoft.com/en-us/ef/core/providers/index
    /// Install-Package Microsoft.EntityFrameworkCore.SqlServer => UseSqlServer
    /// @"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;"
    /// </summary>
    public class MyContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CustomConnection.);
        }
    }
}
