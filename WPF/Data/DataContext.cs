using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF.Models;

namespace WPF.Data
{
    public class DataContext : DbContext
    {
        private string? _connectionString = "Data Source=REPORTINGPC\\REPORTINGPCSQL;Initial Catalog=MDLDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public DbSet<NoteObject> Notes { get; set; }
        public DbSet<ApprovedMatchObject> ApprovedMatches { get; set; }
        public DbSet<ImageObject> Images { get; set; }
        public DbSet<MaterialObject> Materials { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
