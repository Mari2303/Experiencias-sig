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
       
        public string Action { get; set; }
        public DateTime dateTime { get; set; }
        public string tableName { get; set; }
        public int UserId { get; set; }
       

    }
}
