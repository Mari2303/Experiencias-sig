using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
  public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string Active { get; set; }
       
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}