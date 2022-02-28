using APICatalago.Pagination;

namespace APICatalago.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategorias(Parameters produtos);
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
