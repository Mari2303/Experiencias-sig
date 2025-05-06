using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class FromModule
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public From From { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}