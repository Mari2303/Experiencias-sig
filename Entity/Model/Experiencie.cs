using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class Experiencie
    {
        public int Id { get; set; }
        public string NameExperience { get; set; } = string.Empty;
        public string  Summary { get; set; } = string.Empty;

        public string Methodologies { get; set; } = string.Empty;

        public string Transfer  {get; set; } = string.Empty;

        public string DataRegistration { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int InstitutionId { get; set; }
        public Institucion Institucion { get; set; }
        public string InstitutionName { get; set; } = string.Empty;


        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}