using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public    class RolFromPermissionDTO
    {
     
        public int Id { get; set; }
        public int RolId { get; set; }
        public int FromId { get; set; }
        public int PermissionId { get; set; }
       
    }
}
