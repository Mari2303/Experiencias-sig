using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Entity.Model
{
  public class User
    {
        public int Id { get; set; }

        public bool Active { get; set; } = true;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int PersonId { get; set; }
        public Person Person { get; set; }
       
    }
}