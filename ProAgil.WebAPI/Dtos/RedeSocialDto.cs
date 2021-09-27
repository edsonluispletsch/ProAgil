using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class RedeSocialDto
    {
        public int ID { get; set; }
        [Required (ErrorMessage ="O campo {0} é obrigatório")]
        public string Nome { get; set; }
        [Required (ErrorMessage ="O campo {0} é obrigatório")]
        public string URL { get; set; }
    }
}