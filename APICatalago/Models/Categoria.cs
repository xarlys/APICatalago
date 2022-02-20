using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalago
{
    public class Categoria
    {
        // inicialização da coleção / collection
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string? Nome { get; set; }

        [Required]
        [MaxLength(300)]
        public string? ImagemUrl { get; set; }

        
        //Definindo propriedades de navegação
        public ICollection<Produto> Produtos { get; set; }
    }
}