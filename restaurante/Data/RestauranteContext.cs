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
    public DbSet<VentaCerrada> VentasCerradas { get; set; }
    public DbSet<DetalleVentaCerrada> DetallesVentasCerradas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Obtener la ruta base de la aplicación (donde está el ejecutable)
        string appPath = AppDomain.CurrentDomain.BaseDirectory;
        string dbPath = Path.Combine(appPath, "restaurante.db");
        
        // Si la base de datos no existe en la carpeta de ejecución pero existe en la raíz del proyecto,
        // copiarla automáticamente (útil para la primera ejecución en Debug)
        string projectRootDbPath = Path.Combine(appPath, @"..\..\..\restaurante.db");
        if (!File.Exists(dbPath) && File.Exists(projectRootDbPath))
        {
            File.Copy(projectRootDbPath, dbPath, overwrite: false);
        }
        
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
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

        modelBuilder.Entity<DetalleVentaCerrada>()
            .HasOne(d => d.VentaCerrada)
            .WithMany(v => v.Detalles)
            .HasForeignKey(d => d.VentaCerradaId);

        // DATOS SEMILLA COMENTADOS: La base de datos real ya contiene todos los datos necesarios
        // No se necesitan datos iniciales porque ya existe restaurante.db con información completa
        
        /*
        // Datos iniciales
        modelBuilder.Entity<Configuracion>().HasData(
            new Configuracion { Id = 1 }
        );

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Camarones", ColorHex = "#e74c3c" },
            new Categoria { Id = 2, Nombre = "Filetes", ColorHex = "#3498db" },
            new Categoria { Id = 3, Nombre = "Postres", ColorHex = "#f1c40f" },
            new Categoria { Id = 4, Nombre = "Bebidas", ColorHex = "#16a085" },
            new Categoria { Id = 5, Nombre = "Cervezas", ColorHex = "#e67e22" }
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
        */
    }

    // Método para actualizar la base de datos automáticamente
    public void ActualizarBaseDatos()
    {
        try
        {
            // Verificar si las tablas VentasCerradas y DetallesVentasCerradas existen
            var connection = Database.GetDbConnection();
            connection.Open();
            
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='VentasCerradas';";
            var result = command.ExecuteScalar();
            
            if (result == null)
            {
                // Las tablas no existen, crearlas
                Database.ExecuteSqlRaw(@"
                    CREATE TABLE VentasCerradas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        NumeroMesa INTEGER NOT NULL,
                        FechaApertura TEXT NOT NULL,
                        FechaCierre TEXT NOT NULL,
                        Mesero TEXT NOT NULL,
                        Subtotal REAL NOT NULL,
                        Descuento REAL NOT NULL,
                        Total REAL NOT NULL
                    );
                ");

                Database.ExecuteSqlRaw(@"
                    CREATE TABLE DetallesVentasCerradas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        VentaCerradaId INTEGER NOT NULL,
                        NombrePlatillo TEXT NOT NULL,
                        Cantidad INTEGER NOT NULL,
                        PrecioUnitario REAL NOT NULL,
                        Total REAL NOT NULL,
                        FOREIGN KEY (VentaCerradaId) REFERENCES VentasCerradas(Id) ON DELETE CASCADE
                    );
                ");
                
                Database.ExecuteSqlRaw("CREATE INDEX IX_DetallesVentasCerradas_VentaCerradaId ON DetallesVentasCerradas(VentaCerradaId);");
                
                MessageBox.Show("? Base de datos actualizada correctamente con las nuevas tablas de ventas cerradas.", 
                    "Actualización", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            connection.Close();
        }
        catch (Exception ex)
        {
            // Mostrar error si hay problemas
            MessageBox.Show($"?? Error al actualizar base de datos: {ex.Message}\n\n{ex.InnerException?.Message}", 
                "Error de Actualización", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
