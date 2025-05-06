using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public  class RolFromPermission
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }
   
        public int PermissionId { get; set; }
        public Permission  Permission  { get; set; }
        public int FormId { get; set; }
        public From Form { get; set; } = new From();
        public DateTime CreatedAt { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}