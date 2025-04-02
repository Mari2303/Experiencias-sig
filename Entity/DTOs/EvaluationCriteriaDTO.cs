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
        public string Score { get; set; }
        public int EvaluationId { get; set; }

        public int CriteriaId { get; set; }
    }
}
