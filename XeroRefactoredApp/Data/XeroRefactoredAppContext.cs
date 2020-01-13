using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace XeroRefactoredApp.Models
{
    public class XeroRefactoredAppContext : DbContext
    {
        public XeroRefactoredAppContext (DbContextOptions<XeroRefactoredAppContext> options)
            : base(options)
        {
        }

        public DbSet<XeroRefactoredApp.Models.Product> Product { get; set; }
        public DbSet<XeroRefactoredApp.Models.ProductOption> ProductOption { get; set; }
    }
}
