using APICatalago.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categorias.AsNoTracking().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados!");
            }
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(x=> x.Produtos).ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (categoria == null)
                {
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
