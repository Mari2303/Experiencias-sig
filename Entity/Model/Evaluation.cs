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
        public string TypeEvaluation  { get; set; }

        public string Comments { get; set; }

        public DateTime DateTime { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int StateId { get; set; }
        public State State { get; set; }

        public int ExperienceId { get; set; }
        public Experience Experience { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}