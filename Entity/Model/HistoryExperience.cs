using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class HistoryExperience
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int ExperiencieId { get; set; }
        public Experiencie Experiencie { get; set; }

        public string ExperiencieName { get; set; } = string.Empty;
        public bool Action { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string UserName { get; set; } = string.Empty;



    }
}