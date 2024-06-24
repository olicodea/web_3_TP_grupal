using GeneradorDeExamenes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorDeExamanes.Logica.Services
{
    public interface ICategoriaService
    {
        public List<Categoria> Listar();

    }
    public class CategoriaService : ICategoriaService
    {
        private ExamenIaContext _context;
        public CategoriaService(ExamenIaContext context)
        {
            _context = context;
        }

        public List<Categoria> Listar()
        {
            return _context.Categoria
                .OrderBy(c => c.Nombre)
                .ToList();
        }
    }
}
