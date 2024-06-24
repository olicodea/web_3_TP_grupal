using GeneradorDeExamenes.Entidades;

namespace GeneradorDeExamanes.Logica.Services
{
    public interface ICategoriaService
    {
        public List<Categoria> Listar();
        public string GetCategoriaPorId(int id);
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
        public string GetCategoriaPorId(int id)
        {
            return _context.Categoria
                .Where(c => c.IdCategoria == id)
                .Select(c => c.Nombre)
                .FirstOrDefault();
        }

    }
}
