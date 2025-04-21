using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
  public  class EvaluationCriteriaDTO
    {
        public int Id { get; set; }
        public string Score { get; set; } = string.Empty;
        public int EvaluationId { get; set; }
        public string EvaluationName { get; set; } = string.Empty;
        public int CriteriaId { get; set; }
        public string CriteriaName { get; set; } = string.Empty;
    }
}
