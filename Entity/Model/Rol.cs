using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class Rol
    {
        

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string typeRol { get; set; } = string.Empty;

        public bool Active { get; set; } 
    }
}