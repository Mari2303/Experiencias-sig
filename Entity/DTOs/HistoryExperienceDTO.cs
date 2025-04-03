using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
 public   class HistoryExperienceDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string TableName { get; set; }
        public string Observation { get; set; }
        public string Afected { get; set; }
        public bool Active { get; set; }

    }
}
