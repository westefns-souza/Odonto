using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Odonto.Models;

namespace Odonto.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Profissional> Profissionais { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<Procedimento> Procedimentos { get; set; }
        public DbSet<Marcacao> Marcacoes { get; set; }
    }
}
