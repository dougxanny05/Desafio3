using Desafio3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Desafio3.Data;

public class MiDbContext(DbContextOptions<MiDbContext> options) : IdentityDbContext<Usuario>(options)
{
    public DbSet<Receta> Recetas => Set<Receta>();
    public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();
    public DbSet<PasoPreparacion> PasosPreparacion => Set<PasoPreparacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Receta>(entity =>
        {
            entity.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Recetas_Nombre_MinLength",
                    "LEN(LTRIM(RTRIM([Nombre]))) >= 3");
                table.HasCheckConstraint(
                    "CK_Recetas_TiempoPreparacion_Positive",
                    "[TiempoPreparacion] > 0");
            });

            entity.Property(receta => receta.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(receta => receta.Descripcion)
                .HasMaxLength(2000);

            entity.Property(receta => receta.TiempoPreparacion)
                .IsRequired();
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Ingredientes_Nombre_MinLength",
                    "LEN(LTRIM(RTRIM([Nombre]))) >= 3");
                table.HasCheckConstraint(
                    "CK_Ingredientes_Cantidad_Positive",
                    "[Cantidad] > 0");
                table.HasCheckConstraint(
                    "CK_Ingredientes_UnidadMedida_NotBlank",
                    "LEN(LTRIM(RTRIM([UnidadMedida]))) > 0");
            });

            entity.Property(ingrediente => ingrediente.Nombre)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(ingrediente => ingrediente.Cantidad)
                .HasPrecision(18, 3)
                .IsRequired();

            entity.Property(ingrediente => ingrediente.UnidadMedida)
                .HasMaxLength(30)
                .IsRequired();

            entity.HasOne(ingrediente => ingrediente.Receta)
                .WithMany(receta => receta.Ingredientes)
                .HasForeignKey(ingrediente => ingrediente.RecetaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PasoPreparacion>(entity =>
        {
            entity.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_PasosPreparacion_Descripcion_MinLength",
                    "LEN(LTRIM(RTRIM([Descripcion]))) >= 10");
                table.HasCheckConstraint(
                    "CK_PasosPreparacion_Orden_Positive",
                    "[Orden] > 0");
            });

            entity.Property(paso => paso.Descripcion)
                .HasMaxLength(2000)
                .IsRequired();

            entity.Property(paso => paso.Orden)
                .IsRequired();

            entity.HasOne(paso => paso.Receta)
                .WithMany(receta => receta.PasosPreparacion)
                .HasForeignKey(paso => paso.RecetaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(paso => new { paso.RecetaId, paso.Orden })
                .IsUnique();
        });

        modelBuilder.Entity<Receta>().HasData(
            new Receta
            {
                Id = 1,
                Nombre = "Ensalada César",
                Descripcion = "Ensalada clásica con pollo, lechuga y aderezo César.",
                TiempoPreparacion = 20
            },
            new Receta
            {
                Id = 2,
                Nombre = "Pasta Carbonara",
                Descripcion = "Pasta con salsa de crema, huevo y queso parmesano.",
                TiempoPreparacion = 30
            },
            new Receta
            {
                Id = 3,
                Nombre = "Sopa de Tomate",
                Descripcion = "Sopa ligera de tomate con albahaca.",
                TiempoPreparacion = 40
            });

        modelBuilder.Entity<Ingrediente>().HasData(
            new Ingrediente { Id = 1, Nombre = "Lechuga romana", Cantidad = 1, UnidadMedida = "Unidad", RecetaId = 1 },
            new Ingrediente { Id = 2, Nombre = "Pollo a la parrilla", Cantidad = 200, UnidadMedida = "Gramos", RecetaId = 1 },
            new Ingrediente { Id = 3, Nombre = "Aderezo César", Cantidad = 50, UnidadMedida = "Mililitros", RecetaId = 1 },
            new Ingrediente { Id = 4, Nombre = "Pasta espagueti", Cantidad = 250, UnidadMedida = "Gramos", RecetaId = 2 },
            new Ingrediente { Id = 5, Nombre = "Crema de leche", Cantidad = 100, UnidadMedida = "Mililitros", RecetaId = 2 },
            new Ingrediente { Id = 6, Nombre = "Huevo", Cantidad = 1, UnidadMedida = "Unidad", RecetaId = 2 },
            new Ingrediente { Id = 7, Nombre = "Queso parmesano", Cantidad = 50, UnidadMedida = "Gramos", RecetaId = 2 },
            new Ingrediente { Id = 8, Nombre = "Tomates frescos", Cantidad = 500, UnidadMedida = "Gramos", RecetaId = 3 },
            new Ingrediente { Id = 9, Nombre = "Albahaca", Cantidad = 5, UnidadMedida = "Hojas", RecetaId = 3 });

        modelBuilder.Entity<PasoPreparacion>().HasData(
            new PasoPreparacion { Id = 1, Descripcion = "Lavar la lechuga romana y cortarla en trozos.", Orden = 1, RecetaId = 1 },
            new PasoPreparacion { Id = 2, Descripcion = "Asar el pollo y cortarlo en tiras.", Orden = 2, RecetaId = 1 },
            new PasoPreparacion { Id = 3, Descripcion = "Mezclar la lechuga, el pollo y el aderezo César.", Orden = 3, RecetaId = 1 },
            new PasoPreparacion { Id = 4, Descripcion = "Cocinar la pasta en agua hirviendo con sal.", Orden = 1, RecetaId = 2 },
            new PasoPreparacion { Id = 5, Descripcion = "Mezclar el huevo, la crema y el queso parmesano.", Orden = 2, RecetaId = 2 },
            new PasoPreparacion { Id = 6, Descripcion = "Añadir la mezcla a la pasta caliente.", Orden = 3, RecetaId = 2 },
            new PasoPreparacion { Id = 7, Descripcion = "Cortar los tomates y hervirlos hasta que se ablanden.", Orden = 1, RecetaId = 3 },
            new PasoPreparacion { Id = 8, Descripcion = "Licuar los tomates y agregar la albahaca.", Orden = 2, RecetaId = 3 },
            new PasoPreparacion { Id = 9, Descripcion = "Cocinar por 10 minutos más y servir caliente.", Orden = 3, RecetaId = 3 });
    }
}
