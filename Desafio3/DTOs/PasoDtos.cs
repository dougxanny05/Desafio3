using System.ComponentModel.DataAnnotations;

namespace Desafio3.DTOs;

public class PasoRequest
{
    [Required]
    [MinLength(10)]
    [StringLength(2000)]
    public string Descripcion { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Orden { get; set; }

    [Range(1, int.MaxValue)]
    public int RecetaId { get; set; }
}

public class PasoResponse
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int Orden { get; set; }
    public int RecetaId { get; set; }
}
