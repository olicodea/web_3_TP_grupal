using GeneradorDeExamenes.Entidades;
using GeneradorDeExamenes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    private readonly IIaService _iaService;

    public ExamenService(IIaService iaService, ExamenIaContext context, ILogger<ExamenService> logger)
    {
        _context = context;
        _logger = logger;
        _iaService = iaService;
    }

    public async Task AddExamenAsync(ExamenViewModel examenModel)
    {
        try
        {
            
            string feedback = examenModel.Feedback;
            string categoria = await _iaService.ClasificarCategoriaAsync(feedback);

            if (string.IsNullOrEmpty(categoria))
            {
                _logger.LogWarning("No se pudo clasificar la categoría del examen.");
                return;
            }

          
            int idCategoria = await GetCategoriaIdPorNombreAsync(categoria);
            if (idCategoria == 0)
            {
                _logger.LogError($"La categoría '{categoria}' no existe en la base de datos.");
                return;
            }

            
            var examen = examenModel.MapearAEntidad();
            examen.IdCategoria = idCategoria;

          
            _context.Examen.Add(examen);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            
            var innerException = ex.InnerException;
            while (innerException != null && innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
            }

            _logger.LogError($"Error al agregar el examen: {ex.Message}. Detalle: {innerException?.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error general al agregar el examen: {ex.Message}");
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

    public async Task<int> GetCategoriaIdPorNombreAsync(string nombreCategoria)
    {
        try
        {
            
            string nombreCategoriaLower = nombreCategoria.ToLower();

           
            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombreCategoriaLower);

            if (categoria != null)
            {
                return categoria.IdCategoria;
            }
            else
            {
                _logger.LogError($"La categoría '{nombreCategoria}' no existe en la base de datos.");
                return 0; 
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al obtener el ID de la categoría por nombre: {ex.Message}");
            return 0; 
        }
    }

}