using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public    class RolPermissionDTO
    {
        public readonly int PermisionId;

        public int Id { get; set; }
        public int RolId { get; set; }
        public int PermissionId { get; set; }
    }
}
