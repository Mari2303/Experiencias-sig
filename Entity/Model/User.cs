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
        public string name { get; set; }

        public string email { get; set; }
        public string Password { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}