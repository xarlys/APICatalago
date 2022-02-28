using APICatalago.Pagination;
using APICatalago.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace APICatalago.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper? _mapper;

        public CategoriasController(IMapper mapper, IUnitOfWork context, IConfiguration config, ILogger<CategoriasController> logger)
        {
            _uof = context;
            _configuration = config;
            _logger = logger;
        }

        [HttpGet("autor")]
        public string GetAutor()
        {
            var autor = _configuration["autor"];
            var conexao = _configuration["ConnectionStrings:DefaultConnection"];

            return $"Autor: {autor} - Conexão: {conexao}";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get([FromQuery] Parameters categoriasParameters)
        {
            _logger.LogInformation("<===========GET api/categorias ====================>");

            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
                
                return categorias;
                //return await _uof.CategoriaRepository.Get().ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados!");
            }
        }

        [HttpGet("produtos")]

        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
        {
            var categorias = await _uof.CategoriaRepository
                             .GetCategoriasProdutos();

            var resultCategorias = _mapper.Map<List<Categoria>>(categorias);

            return resultCategorias;
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> GetId(int id)
        {
            _logger.LogInformation($"<===========GET api/categorias/id = {id} ====================>");

            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(x => x.Id == id);
                if (categoria == null)
                {
                    _logger.LogInformation($"<===========GET api/categorias/id = {id} NOT FOUND ====================>");
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter a categoria do banco de dados!");
            }

        }

       

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Categoria categoria)
        {
            try
            {
                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar criar uma nova categoria!");
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.Id)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com id={id}");
                }

                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();
                return Ok($"Categoria com id={id} foi atualizada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar atualizar a categoria!");
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            try
            {
                //var categoria = _uof.Categorias.FirstOrDefault(p => p.Id == id); //sempre vai no banco de dados
                var categoria = await _uof.CategoriaRepository.GetById(c => c.Id == id); // vai na memória primeiro, mas só pode ser usado se o Id for a chave primária

                if (categoria == null)
                {
                    return NotFound($"Categoria com id={id} não foi encontrada!");
                }

                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir a categoria do banco de dados!");
            }
        }

    }
}
















// Código de listagem asyncrona

//[HttpGet("saudacao/{nome}")]
//public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuservico,
//   string nome)
//{
//    return meuservico.Saudacao(nome);
//}

//[HttpGet]
//public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
//{
//    _logger.LogInformation("<===========GET api/categorias ====================>");

//    try
//    {
//        return await _uof.Categorias.AsNoTracking().ToListAsync();
//    }
//    catch (Exception)
//    {
//        return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados!");
//    }
//}

//[HttpGet("produtos")]
//public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
//{
//    _logger.LogInformation("<===========GET api/categorias/produtos ====================>");

//    return await _uof.Categorias.Include(x=> x.Produtos).ToListAsync();
//}

//[HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
//public async Task<ActionResult<Categoria>> GetIdAsync(int id)
//{
//    _logger.LogInformation($"<===========GET api/categorias/id = {id} ====================>");

//    try
//    {
//        var categoria = await _uof.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
//        if (categoria == null)
//        {
//            _logger.LogInformation($"<===========GET api/categorias/id = {id} NOT FOUND ====================>");
//            return NotFound($"A categoria com id={id} não foi encontrada");
//        }
//        return categoria;
//    }
//    catch (Exception)
//    {
//        return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter a categoria do banco de dados!");
//    }

//}
