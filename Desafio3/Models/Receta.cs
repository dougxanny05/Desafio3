using System.ComponentModel.DataAnnotations;

namespace Desafio3.Models;

public class Receta
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue)]
    public int TiempoPreparacion { get; set; }

    public ICollection<Ingrediente> Ingredientes { get; set; } = new List<Ingrediente>();
    public ICollection<PasoPreparacion> PasosPreparacion { get; set; } = new List<PasoPreparacion>();
}
