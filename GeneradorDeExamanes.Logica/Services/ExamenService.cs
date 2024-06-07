using GeneradorDeExamenes.Entidades;
using GeneradorDeExamenes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeneradorDeExamenes.Logica.Services;

public interface IExamenService
{
    Task<List<Examan>> GetAllExamenesAsync();
    Task<Examan> GetExamenByIdAsync(int id);
    Task AddExamenAsync(ExamenViewModel examen);
    Task UpdateExamenAsync(Examan examen);
    Task DeleteExamenAsync(int id);
}
public class ExamenService : IExamenService
{
    private readonly ExamenIAContext _context;
    private readonly ILogger<ExamenService> _logger;

    public ExamenService(ExamenIAContext context, ILogger<ExamenService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddExamenAsync(ExamenViewModel examenModel)
    {
        try
        {
            var examen = new Examan();
            examen = examenModel.MapearAEntidad();
            _context.Examen.Add(examen);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al agregar un nuevo examen: {ex.Message}");
        }
    }

    //TODO: Probar y modificar metodos
    public async Task<List<Examan>> GetAllExamenesAsync()
    {
        /*
        try
        {
            return await _context.Examen.Include(e => e.Pregunta).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al obtener todos los exámenes: {ex.Message}");
            return new List<Examan>();
        }
        */

        return null;
    }

    public async Task<Examan> GetExamenByIdAsync(int id)
    {
        /*
        try
        {
            return await _context.Examen.Include(e => e.Pregunta).FirstOrDefaultAsync(e => e.IdExamen == id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al obtener el examen por ID: {ex.Message}");
            return null;
        }
        */

        return null;
    }

    public async Task UpdateExamenAsync(Examan examen)
    {
        /*
        try
        {
            _context.Examen.Update(examen);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al actualizar el examen: {ex.Message}");
        }
        */
    }

    public async Task DeleteExamenAsync(int id)
    {
        /*
        try
        {
            var examen = await _context.Examen.FirstOrDefaultAsync(e => e.IdExamen == id);
            if (examen != null)
            {
                _context.Examen.Remove(examen);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al eliminar el examen: {ex.Message}");
        }
        */
    }
}