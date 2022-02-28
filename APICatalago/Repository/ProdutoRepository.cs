using APICatalago.Models.Context;
using APICatalago.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public async Task<PagedList<Produto>> GetProdutos(Parameters parameters)
        {
            return await PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.Id), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(c => c.Preco).ToListAsync();
        }
      
    }
}
