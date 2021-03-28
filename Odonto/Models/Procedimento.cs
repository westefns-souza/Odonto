using System.ComponentModel.DataAnnotations;

namespace Odonto.Models
{
    public class Procedimento
    {
        [Key]
        public int ProcedimentoId { get; set; }
        public string Nome { get; set; }

        [Display(Name = "Duração em Minutos")]
        public int DuracaoMinutos { get; set; }

        public override string ToString() => Nome;
    }
}
