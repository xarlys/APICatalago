using APICatalago.DTOs;
using APICatalago.Filters;
using APICatalago.Pagination;
using APICatalago.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APICatalago.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] Parameters produtosParameter)
        {

            var resultProdutos = _uof.ProdutoRepository.GetProdutos(produtosParameter);

            var metadata = new { resultProdutos.TotalCount, resultProdutos.PageSize, resultProdutos.CurrentPage, resultProdutos.TotalPages, resultProdutos.HasNext, resultProdutos.HasPrevious };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
            
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(resultProdutos);
            return produtosDTO;

            //var produtos = _uof.ProdutoRepository.Get().ToList();
            //var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            //return produtosDTO;

            //return _uof.ProdutoRepository.Get().ToList();
            //return _context.Produtos.AsNoTracking().ToList();
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetId(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return produtoDTO;
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
        public ActionResult Post([FromBody] ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO);
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            var returnProdutoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.Id }, returnProdutoDTO);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.Id)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            //var produto = _uof.Produtos.FirstOrDefault(p => p.Id == id); //sempre vai no banco de dados
            var produto = _uof.ProdutoRepository.GetById(p => p.Id == id); // vai na memória primeiro, mas só pode ser usado se o Id for a chave primária

            if (produto == null)
            {
                return NotFound();
            }

            var returnProdutoDTO = _mapper.Map<ProdutoDTO>(produto);

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return returnProdutoDTO;
        }

    }
}
