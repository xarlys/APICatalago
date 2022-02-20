using APICatalago.Models.Context;
using APICatalago.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, IConfiguration config, ILogger<CategoriasController> logger)
        {
            _context = context;
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

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuservico,
           string nome)
        {
            return meuservico.Saudacao(nome);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            _logger.LogInformation("<===========GET api/categorias ====================>");

            try
            {
                return await _context.Categorias.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados!");
            }
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            _logger.LogInformation("<===========GET api/categorias/produtos ====================>");

            return await _context.Categorias.Include(x=> x.Produtos).ToListAsync();
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> GetIdAsync(int id)
        {
            _logger.LogInformation($"<===========GET api/categorias/id = {id} ====================>");

            try
            {
                var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
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
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                _context.Categorias.Add(categoria);
                _context.SaveChanges();

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

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();
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
                //var categoria = _context.Categorias.FirstOrDefault(p => p.Id == id); //sempre vai no banco de dados
                var categoria = _context.Categorias.Find(id); // vai na memória primeiro, mas só pode ser usado se o Id for a chave primária

                if (categoria == null)
                {
                    return NotFound($"Categoria com id={id} não foi encontrada!");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir a categoria do banco de dados!");
            }
        }

    }
}
