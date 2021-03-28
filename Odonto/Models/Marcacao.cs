using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Odonto.Models
{
    public class Marcacao
    {
        public Marcacao()
        {

        }

        public Marcacao(int agendaId, DateTime dataDeInicio, DateTime dataDeFim)
        {
            AgendaId = agendaId;
            DataDeInicio = dataDeInicio;
            DataDeFim = dataDeFim;
            Marcado = false;
        }

        [Key]
        public int MarcacaoId { get; set; }

        public int AgendaId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(AgendaId))]
        public Agenda Agenda { get; set; }
        
        [Display(Name = "Paciente")]
        public string UsuarioId { get; set; }

        [Display(Name = "Procedimento")]
        public int ProcedimentoId { get; set; }
        
        [ForeignKey(nameof(ProcedimentoId))]
        public Procedimento Procedimento { get; set; }

        [Display(Name = "Data e Hora da marcacao")]
        public DateTime DataDeInicio { get; set; }

        [Display(Name = "Data e Hora de fim da marcacao")]
        public DateTime DataDeFim { get; set; }

        public bool Marcado { get; set; }

        [NotMapped]
        public string NomeUsuario{ get; set; }
    }
}
