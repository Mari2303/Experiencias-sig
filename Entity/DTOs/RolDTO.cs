using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RolDTO
    {
        public int Id { get; set; }
        public string TypeRol { get; set; } = string.Empty;
     
       
        public bool Active { get; set; }
    }
}
