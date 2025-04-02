using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class HistoryExperience
    {
        public int Id { get; set; }
       
       public  string action { get; set; }

       public DateTime dateTime { get; set; }

       public  int userId { get; set; }

           public DateTime DeleteAt { get; set;  }     
            public DateTime CreateAt { get; set; }

    }
}