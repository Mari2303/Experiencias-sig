using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class Experience
    {
        public int Id { get; set; }
        public string nameExperience { get; set; }

        public string dataStart { get; set; }

        public string durationDays { get; set; }

        public string durationMonths { get; set; }

        public string durationYears { get; set; }

        public string  summary { get; set; }

        public string methodologies { get; set; }

        public string transfe  {get; set; }

        public string dataRegistration { get; set; }

        public int UserId { get; set; }

        public int InstitutionId { get; set; }
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}