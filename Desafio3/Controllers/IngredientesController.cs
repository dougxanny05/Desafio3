using Desafio3.Data;
using Desafio3.DTOs;
using Desafio3.Identity;
using Desafio3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Desafio3.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IngredientesController(MiDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IngredienteResponse>>> GetAll(
        [FromQuery] int? recetaId,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Ingredientes.AsNoTracking();
        if (recetaId.HasValue)
        {
            query = query.Where(ingrediente => ingrediente.RecetaId == recetaId.Value);
        }

        var ingredientes = await query
            .OrderBy(ingrediente => ingrediente.RecetaId)
            .ThenBy(ingrediente => ingrediente.Id)
            .ToListAsync(cancellationToken);

        return Ok(ingredientes.Select(Map));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IngredienteResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var ingrediente = await dbContext.Ingredientes
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        return ingrediente is null ? NotFound() : Ok(Map(ingrediente));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<IngredienteResponse>> Create(
        IngredienteRequest request,
        CancellationToken cancellationToken)
    {
        if (!await dbContext.Recetas.AnyAsync(receta => receta.Id == request.RecetaId, cancellationToken))
        {
            return BadRequest($"La receta {request.RecetaId} no existe.");
        }

        var ingrediente = new Ingrediente
        {
            Nombre = request.Nombre,
            Cantidad = request.Cantidad,
            UnidadMedida = request.UnidadMedida,
            RecetaId = request.RecetaId
        };

        dbContext.Ingredientes.Add(ingrediente);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = ingrediente.Id }, Map(ingrediente));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Update(
        int id,
        IngredienteRequest request,
        CancellationToken cancellationToken)
    {
        var ingrediente = await dbContext.Ingredientes.FindAsync([id], cancellationToken);
        if (ingrediente is null)
        {
            return NotFound();
        }

        if (!await dbContext.Recetas.AnyAsync(receta => receta.Id == request.RecetaId, cancellationToken))
        {
            return BadRequest($"La receta {request.RecetaId} no existe.");
        }

        ingrediente.Nombre = request.Nombre;
        ingrediente.Cantidad = request.Cantidad;
        ingrediente.UnidadMedida = request.UnidadMedida;
        ingrediente.RecetaId = request.RecetaId;
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var ingrediente = await dbContext.Ingredientes.FindAsync([id], cancellationToken);
        if (ingrediente is null)
        {
            return NotFound();
        }

        dbContext.Ingredientes.Remove(ingrediente);
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static IngredienteResponse Map(Ingrediente ingrediente) => new()
    {
        Id = ingrediente.Id,
        Nombre = ingrediente.Nombre,
        Cantidad = ingrediente.Cantidad,
        UnidadMedida = ingrediente.UnidadMedida,
        RecetaId = ingrediente.RecetaId
    };
}