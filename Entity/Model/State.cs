using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
 public  class State
    {
        public int Id { get; set; }

        public int HistoryExperienceId { get; set; }

        public HistoryExperience HistoryExperience { get; set; }
        
       public DateTime CreateAt { get; set; }
        public DateTime DeleteAt { get; set; }
        public string Name { get; set; }        
        

    }
}