namespace APICatalago.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        // metodo específico do repository
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
