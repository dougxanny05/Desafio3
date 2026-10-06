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
public class PasosPreparacionController(MiDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PasoResponse>>> GetAll(
        [FromQuery] int? recetaId,
        CancellationToken cancellationToken)
    {
        var query = dbContext.PasosPreparacion.AsNoTracking();
        if (recetaId.HasValue)
        {
            query = query.Where(paso => paso.RecetaId == recetaId.Value);
        }

        var pasos = await query
            .OrderBy(paso => paso.RecetaId)
            .ThenBy(paso => paso.Orden)
            .ToListAsync(cancellationToken);

        return Ok(pasos.Select(Map));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PasoResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var paso = await dbContext.PasosPreparacion
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        return paso is null ? NotFound() : Ok(Map(paso));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<PasoResponse>> Create(
        PasoRequest request,
        CancellationToken cancellationToken)
    {
        if (!await dbContext.Recetas.AnyAsync(receta => receta.Id == request.RecetaId, cancellationToken))
        {
            return BadRequest($"La receta {request.RecetaId} no existe.");
        }

        if (await ExisteOrdenAsync(request.RecetaId, request.Orden, null, cancellationToken))
        {
            return Conflict("Ya existe un paso con ese orden para la receta.");
        }

        var paso = new PasoPreparacion
        {
            Descripcion = request.Descripcion,
            Orden = request.Orden,
            RecetaId = request.RecetaId
        };

        dbContext.PasosPreparacion.Add(paso);
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            dbContext.Entry(paso).State = EntityState.Detached;
            if (await ExisteOrdenAsync(request.RecetaId, request.Orden, null, cancellationToken))
            {
                return Conflict("Ya existe un paso con ese orden para la receta.");
            }

            throw;
        }

        return CreatedAtAction(nameof(GetById), new { id = paso.Id }, Map(paso));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Update(
        int id,
        PasoRequest request,
        CancellationToken cancellationToken)
    {
        var paso = await dbContext.PasosPreparacion.FindAsync([id], cancellationToken);
        if (paso is null)
        {
            return NotFound();
        }

        if (!await dbContext.Recetas.AnyAsync(receta => receta.Id == request.RecetaId, cancellationToken))
        {
            return BadRequest($"La receta {request.RecetaId} no existe.");
        }

        if (await ExisteOrdenAsync(request.RecetaId, request.Orden, id, cancellationToken))
        {
            return Conflict("Ya existe un paso con ese orden para la receta.");
        }

        paso.Descripcion = request.Descripcion;
        paso.Orden = request.Orden;
        paso.RecetaId = request.RecetaId;
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            dbContext.Entry(paso).State = EntityState.Detached;
            if (await ExisteOrdenAsync(request.RecetaId, request.Orden, id, cancellationToken))
            {
                return Conflict("Ya existe un paso con ese orden para la receta.");
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var paso = await dbContext.PasosPreparacion.FindAsync([id], cancellationToken);
        if (paso is null)
        {
            return NotFound();
        }

        dbContext.PasosPreparacion.Remove(paso);
        await dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    private Task<bool> ExisteOrdenAsync(
        int recetaId,
        int orden,
        int? idExcluir,
        CancellationToken cancellationToken)
    {
        return dbContext.PasosPreparacion.AnyAsync(
            paso => paso.RecetaId == recetaId
                && paso.Orden == orden
                && (!idExcluir.HasValue || paso.Id != idExcluir.Value),
            cancellationToken);
    }

    private static PasoResponse Map(PasoPreparacion paso) => new()
    {
        Id = paso.Id,
        Descripcion = paso.Descripcion,
        Orden = paso.Orden,
        RecetaId = paso.RecetaId
    };
}
