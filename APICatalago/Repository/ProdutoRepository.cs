using APICatalago.Models.Context;
using APICatalago.Pagination;

namespace APICatalago.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public PagedList<Produto> GetProdutos(Parameters parameters)
        {
            return PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.Id), parameters.PageNumber, parameters.PageSize);
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }
    }
}
