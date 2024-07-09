using Microsoft.EntityFrameworkCore;
using ApiSiad.Domain.Model;

namespace ApiSiad.Infraestrutura
{
    public class ConnectionContext : DbContext
    {
        public DbSet<Turmas> Turmas { get; set; }
        public DbSet<Alunos> Alunos { get; set; }
        public DbSet<Disciplinas> Disciplinas { get; set; }
        public DbSet<Notas> Notas { get; set; }
   
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost;" +
                "Port=5432;Database=SIAD;" +
                "User Id=postgres;" +
                "Password=Staff4912;");
        }
    }
}
