   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string PermissionType { get; set; }

        public static void Add(PermissionDTO permissionDTO)
        {
            throw new NotImplementedException();
        }
    }
}
