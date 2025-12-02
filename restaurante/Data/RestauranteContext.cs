using Microsoft.EntityFrameworkCore;
using restaurante.Models;

namespace restaurante.Data;

public class RestauranteContext : DbContext
{
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Platillo> Platillos { get; set; }
    public DbSet<Mesa> Mesas { get; set; }
    public DbSet<DetallePedido> DetallesPedidos { get; set; }
    public DbSet<Configuracion> Configuraciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=restaurante.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar relaciones
        modelBuilder.Entity<Platillo>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Platillos)
            .HasForeignKey(p => p.CategoriaId);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(d => d.Mesa)
            .WithMany(m => m.Detalles)
            .HasForeignKey(d => d.MesaId);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(d => d.Platillo)
            .WithMany()
            .HasForeignKey(d => d.PlatilloId);

        // Datos iniciales
        modelBuilder.Entity<Configuracion>().HasData(
            new Configuracion { Id = 1 }
        );

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Camarones" },
            new Categoria { Id = 2, Nombre = "Filetes" },
            new Categoria { Id = 3, Nombre = "Postres" },
            new Categoria { Id = 4, Nombre = "Bebidas" },
            new Categoria { Id = 5, Nombre = "Cervezas" }
        );

        modelBuilder.Entity<Platillo>().HasData(
            new Platillo { Id = 1, Nombre = "Camarones Empanizados", NombreCorto = "C. Empanizados", Precio = 150.00m, CategoriaId = 1 },
            new Platillo { Id = 2, Nombre = "Camarones a la Diabla", NombreCorto = "C. Diabla", Precio = 160.00m, CategoriaId = 1 },
            new Platillo { Id = 3, Nombre = "Camarones al Mojo de Ajo", NombreCorto = "C. Mojo Ajo", Precio = 155.00m, CategoriaId = 1 },
            new Platillo { Id = 4, Nombre = "Filete a la Diabla", NombreCorto = "F. Diabla", Precio = 140.00m, CategoriaId = 2 },
            new Platillo { Id = 5, Nombre = "Filete Relleno", NombreCorto = "F. Relleno", Precio = 165.00m, CategoriaId = 2 },
            new Platillo { Id = 6, Nombre = "Filete Empanizado", NombreCorto = "F. Empanizado", Precio = 135.00m, CategoriaId = 2 }
        );

        // Crear las 13 mesas
        for (int i = 1; i <= 13; i++)
        {
            modelBuilder.Entity<Mesa>().HasData(
                new Mesa { Id = i, NumeroMesa = i, EstaActiva = false }
            );
        }

        // Mesa 14 para "Para Llevar"
        modelBuilder.Entity<Mesa>().HasData(
            new Mesa { Id = 14, NumeroMesa = 0, EstaActiva = false }
        );
    }
}
