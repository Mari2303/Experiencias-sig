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
        public string TypeEvaluation { get; set; } = string.Empty;  
        public string Comments { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } 
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int StateId { get; set; } 
        public string StateName { get; set; } = string.Empty;
        public int ExperiencieId { get; set; }

        public string ExperiencieName { get; set; } = string.Empty;
    }
}
