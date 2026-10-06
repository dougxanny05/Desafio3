using System.ComponentModel.DataAnnotations;

namespace Desafio3.DTOs;

public class RecetaRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Descripcion { get; set; }

    [Range(1, int.MaxValue)]
    public int TiempoPreparacion { get; set; }
}

public class RecetaResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int TiempoPreparacion { get; set; }
    public List<IngredienteResponse> Ingredientes { get; set; } = [];
    public List<PasoResponse> PasosPreparacion { get; set; } = [];
}
