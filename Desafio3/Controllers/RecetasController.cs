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
public class RecetasController(MiDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecetaResponse>>> GetAll(
        [FromQuery] int? maxTiempo,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Recetas
            .AsNoTracking()
            .Include(receta => receta.Ingredientes)
            .Include(receta => receta.PasosPreparacion)
            .AsQueryable();

        if (maxTiempo.HasValue)
        {
            query = query.Where(receta => receta.TiempoPreparacion <= maxTiempo.Value);
        }

        var recetas = await query
            .OrderBy(receta => receta.Nombre)
            .ToListAsync(cancellationToken);

        return Ok(recetas.Select(Map));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RecetaResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var receta = await dbContext.Recetas
            .AsNoTracking()
            .Include(item => item.Ingredientes)
            .Include(item => item.PasosPreparacion)
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        return receta is null ? NotFound() : Ok(Map(receta));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<RecetaResponse>> Create(
        RecetaRequest request,
        CancellationToken cancellationToken)
    {
        var receta = new Receta
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            TiempoPreparacion = request.TiempoPreparacion
        };

        dbContext.Recetas.Add(receta);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = receta.Id }, Map(receta));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Update(
        int id,
        RecetaRequest request,
        CancellationToken cancellationToken)
    {
        var receta = await dbContext.Recetas.FindAsync([id], cancellationToken);
        if (receta is null)
        {
            return NotFound();
        }

        receta.Nombre = request.Nombre;
        receta.Descripcion = request.Descripcion;
        receta.TiempoPreparacion = request.TiempoPreparacion;
        await dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var receta = await dbContext.Recetas.FindAsync([id], cancellationToken);
        if (receta is null)
        {
            return NotFound();
        }

        dbContext.Recetas.Remove(receta);
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private static RecetaResponse Map(Receta receta) => new()
    {
        Id = receta.Id,
        Nombre = receta.Nombre,
        Descripcion = receta.Descripcion,
        TiempoPreparacion = receta.TiempoPreparacion,
        Ingredientes = receta.Ingredientes
            .OrderBy(ingrediente => ingrediente.Id)
            .Select(ingrediente => new IngredienteResponse
            {
                Id = ingrediente.Id,
                Nombre = ingrediente.Nombre,
                Cantidad = ingrediente.Cantidad,
                UnidadMedida = ingrediente.UnidadMedida,
                RecetaId = ingrediente.RecetaId
            })
            .ToList(),
        PasosPreparacion = receta.PasosPreparacion
            .OrderBy(paso => paso.Orden)
            .Select(paso => new PasoResponse
            {
                Id = paso.Id,
                Descripcion = paso.Descripcion,
                Orden = paso.Orden,
                RecetaId = paso.RecetaId
            })
            .ToList()
    };
}