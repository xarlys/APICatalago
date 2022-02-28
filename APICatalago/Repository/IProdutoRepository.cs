using APICatalago.Pagination;

namespace APICatalago.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutos(Parameters produtos);
        // metodo específico do repository
        Task<IEnumerable<Produto>> GetProdutosPorPreco();
    }
}
