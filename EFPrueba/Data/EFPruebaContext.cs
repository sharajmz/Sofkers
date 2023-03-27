using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace EFPrueba.Data
{
    public class EFPruebaContext : DbContext
    {
        public EFPruebaContext (DbContextOptions<EFPruebaContext> options)
            : base(options)
        {
        }

        public DbSet<Domain.Sofker> Sofker { get; set; } = default!;
    }
}
