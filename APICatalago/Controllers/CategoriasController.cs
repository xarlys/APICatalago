using APICatalago.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalago.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork context, IConfiguration config, ILogger<CategoriasController> logger)
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
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            _logger.LogInformation("<===========GET api/categorias ====================>");

            try
            {
                return _uof.CategoriaRepository.Get().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados!");
            }
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("<===========GET api/categorias/produtos ====================>");

            return _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetId(int id)
        {
            _logger.LogInformation($"<===========GET api/categorias/id = {id} ====================>");

            try
            {
                var categoria = _uof.CategoriaRepository.GetById(x => x.Id == id);
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

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.Id }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar criar uma nova categoria!");
            }

        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.Id)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com id={id}");
                }

                _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();
                return Ok($"Categoria com id={id} foi atualizada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar atualizar a categoria!");
            }

        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                //var categoria = _uof.Categorias.FirstOrDefault(p => p.Id == id); //sempre vai no banco de dados
                var categoria = _uof.CategoriaRepository.GetById(c => c.Id == id); // vai na memória primeiro, mas só pode ser usado se o Id for a chave primária

                if (categoria == null)
                {
                    return NotFound($"Categoria com id={id} não foi encontrada!");
                }

                _uof.CategoriaRepository.Delete(categoria);
                _uof.Commit();
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir a categoria do banco de dados!");
            }
        }

    }
}
