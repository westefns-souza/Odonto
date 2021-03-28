using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Odonto.Models
{
    public class Agenda
    {
        public Agenda()
        {
            Marcacoes = new List<Marcacao>();
            Disponibilidades = new List<Marcacao>();
        }

        [Key]
        public int AgendaId { get; set; }

        [Required(ErrorMessage = "O Profissional é obrigatório")]
        [Display(Name = "Profissional")]
        public int ProfissionalId { get; set; }

        [ForeignKey(nameof(ProfissionalId))]
        public Profissional Profissional { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatório")]
        [Display(Name = "Data e Hora de início do atendimento")]
        public DateTime DataDeInicio { get; set; }

        [Required(ErrorMessage = "A data de fim é obrigatório")]
        [Display(Name = "Data e Hora de fim de atendimento")]
        public DateTime DataDeFim { get; set; }

        public List<Marcacao> Marcacoes { get; set; }

        [NotMapped]
        public List<Marcacao> Disponibilidades { get; set; }

        public void GerarHorariosDisponiveisPorDuracao(int duracaoEmMinutos)
        {
            var disponibilidades = new List<Marcacao>();

            var data = DataDeInicio;
            var dataFim = DataDeInicio.AddMinutes(duracaoEmMinutos);

            while (dataFim <= DataDeFim)
            {
                var jaMarcado = Marcacoes.Where(x =>
                    (x.DataDeInicio >= data && x.DataDeFim <= dataFim) 
                    || (data >= x.DataDeInicio && dataFim <= x.DataDeFim)
                ).Any();

                if (!jaMarcado)
                    disponibilidades.Add(new Marcacao(AgendaId, data, dataFim));

                data = dataFim;
                dataFim = dataFim.AddMinutes(duracaoEmMinutos);
            }

            Disponibilidades = disponibilidades;
        }
    }
}
