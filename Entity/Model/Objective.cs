using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public  class Objective
    {
        public int Id { get; set; }
         
         public string ObjectiveDescription { get; set; }

         public string innovation { get; set; }

         public string results { get; set; }

         public string sustainability { get; set; }

         public int ExperienceId { get; set; }
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}