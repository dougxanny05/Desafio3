using System.ComponentModel.DataAnnotations;

namespace Desafio3.Models;

public class Ingrediente
{
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.001", "999999999999999.999")]
    [Desafio3.Validation.DecimalScale(3)]
    public decimal Cantidad { get; set; }

    [Required]
    [StringLength(30)]
    public string UnidadMedida { get; set; } = string.Empty;

    public int RecetaId { get; set; }
    public Receta Receta { get; set; } = null!;
}
