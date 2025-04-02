using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
  public  class Evaluation
    {
        public int Id { get; set; }
        public string typeEvaluation  { get; set; }

        public string comments { get; set; }

        public DateTime dataTime { get; set; }

        public int UserId { get; set; }

        public int StateId { get; set; }

        public int ExperienceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}