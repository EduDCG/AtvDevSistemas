using Vendinha.Models;

namespace Vendinha.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class VendinhaDbContext : DbContext
{
    private string connectionString;
    
    public VendinhaDbContext(string connString)
    {
        this.connectionString = connString;
    }
    public VendinhaDbContext(DbContextOptions opt) : base (opt)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Divida> Dividas => Set<Divida>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityCliente = modelBuilder.Entity<Cliente>();
        // mapeamento
        entityCliente.ToTable("clientes");
        entityCliente.Property(c => c.Nome).HasColumnName("nome");
        entityCliente.Property(c => c.DataNascimento).HasColumnName("datanascimento");
        entityCliente.Property(c => c.Email).HasColumnName("email");
        entityCliente.HasKey(c => c.CPF);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrEmpty(connectionString))
        {
            optionsBuilder.UseNpgsql(connectionString)
                .LogTo(message => Debug.WriteLine(message))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
        base.OnConfiguring(optionsBuilder);
    }
}
