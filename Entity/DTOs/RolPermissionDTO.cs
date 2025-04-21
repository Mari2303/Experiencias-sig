using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public    class RolPermissionDTO
    {
     
        public int Id { get; set; }
        public int RolId { get; set; }
        public string RolName { get; set; } = string.Empty;
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
    }
}
