using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
   public class StateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HistoryExperienceName { get; set; } = string.Empty;
        public int HistoryExperienceId { get; set; }
        public bool Active { get; set; }
    }
}
