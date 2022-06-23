using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Context
{
    public class PhidelisContext : DbContext
    {
        public PhidelisContext(DbContextOptions<PhidelisContext> options) : base(options) { }

        public virtual DbSet<Alunos> Alunos { get; set; }
    }
}
