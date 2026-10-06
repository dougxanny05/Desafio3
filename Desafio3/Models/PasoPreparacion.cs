using System.ComponentModel.DataAnnotations;

namespace Desafio3.Models;

public class PasoPreparacion
{
    public int Id { get; set; }

    [Required]
    [MinLength(10)]
    [StringLength(2000)]
    public string Descripcion { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Orden { get; set; }

    public int RecetaId { get; set; }
    public Receta Receta { get; set; } = null!;
}
