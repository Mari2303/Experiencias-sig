using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
  public  class EvaluationDTO
    {
        public int Id { get; set; }
        public string TypeEvaluation { get; set; }
        public string Comments { get; set; }
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }
        public int StateId { get; set; }
        public int ExperiencieId { get; set; }
    }
}
