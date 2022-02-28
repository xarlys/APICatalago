using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalago
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string? Nome { get; set; }

        [Required]
        [MaxLength(300)]
        public string? Descricao { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10,2)")]
        //[Range(1,100000), ErrorMessage = "O Preço deve estar entre {1} e {2}"]
        public decimal Preco { get; set; }

        [Required]
        [MaxLength(500)]
        public string? ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        //Chave estrageiras 1:N - Produtos está relacionado com categoria
        [JsonIgnore]
        public Categoria? Categoria { get; set; } 
        public int CategoriaID { get; set; }    

    }
}