using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    class UserDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
    }
}
