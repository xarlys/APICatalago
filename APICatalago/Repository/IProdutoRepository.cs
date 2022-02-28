using APICatalago.Pagination;

namespace APICatalago.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(Parameters produtos);
        // metodo específico do repository
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
