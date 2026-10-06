using System.ComponentModel.DataAnnotations;

namespace Desafio3.DTOs;

public class IngredienteRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.001", "999999999999999.999")]
    [Desafio3.Validation.DecimalScale(3)]
    public decimal Cantidad { get; set; }

    [Required]
    [StringLength(30)]
    public string UnidadMedida { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int RecetaId { get; set; }
}

public class IngredienteResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;
    public int RecetaId { get; set; }
}
