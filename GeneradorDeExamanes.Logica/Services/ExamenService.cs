using GeneradorDeExamenes.Entidades;
using GeneradorDeExamenes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GeneradorDeExamanes.Logica.Services;

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
    private readonly ExamenIaContext _context;
    private readonly ILogger<ExamenService> _logger;

    public ExamenService(ExamenIaContext context, ILogger<ExamenService> logger)
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

        try
        {
            return await _context.Examen.Include(e => e.Pregunta).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al obtener todos los exámenes: {ex.Message}");
            return new List<Examan>();
        }



    }

    public async Task<Examan?> GetExamenByIdAsync(int id)
    {
        try
        {
            // Verifica que _context no sea nulo
            if (_context == null)
            {
                throw new InvalidOperationException("Database context is not initialized.");
            }

            var examen = await _context.Examen
                                       .Include(e => e.Pregunta) // Asegúrate de que Pregunta es una propiedad de navegación válida
                                       .FirstOrDefaultAsync(e => e.IdExamen == id);

            // Verifica que examen no sea nulo
            if (examen == null)
            {
                _logger.LogWarning($"No se encontró ningún examen con el ID: {id}");
            }

            return examen;
        }
        catch (Exception ex)
        {
            // Verifica que _logger no sea nulo
            _logger?.LogError($"Error al obtener el examen por ID: {ex.Message}");
            return null;
        }
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