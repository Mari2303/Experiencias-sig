
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
  public  class EvaluationCriteria
    {
        public int Id { get; set; }
        public string Score { get; set; } = string.Empty;

        public int EvaluationId { get; set; }
        public Evaluation Evaluation { get; set; }
        public string EvaluationName { get; set; } = string.Empty;

        public int CriteriaId { get; set; }
        public  Criteria  Criteria { get; set; }

        public string CriteriaName { get; set; } = string.Empty;

        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}