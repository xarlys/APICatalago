using APICatalago.Filters;
using APICatalago.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalago.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            
            return produtos;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _uof.ProdutoRepository.Get().ToList();
            //return _context.Produtos.AsNoTracking().ToList();
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> GetId(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.Id == id);
            if (produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        // metodo asyncrono
        //[HttpGet]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        //public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        //{
        //    return await _uof.Produtos.AsNoTracking().ToListAsync();
        //}

        //[HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        //public async Task<ActionResult<Produto>> GetIdAsync(int id)
        //{
        //    var produto = await _uof.Produtos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        //    if (produto == null)
        //    {
        //        return NotFound();
        //    }
        //    return produto;
        //}

        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            //var produto = _uof.Produtos.FirstOrDefault(p => p.Id == id); //sempre vai no banco de dados
            var produto = _uof.ProdutoRepository.GetById(p => p.Id == id); // vai na memória primeiro, mas só pode ser usado se o Id for a chave primária

            if (produto == null)
            {
                return NotFound();
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return produto;
        }

    }
}
