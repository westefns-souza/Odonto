using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Odonto.Models
{
    public class Profissional
    {
        [Key]
        public int ProfissionalId { get; set; }
        public string Nome { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public override string ToString() => Nome;
    }
}
