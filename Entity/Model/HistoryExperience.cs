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
        public DateTime DateTime { get; set; }
        public string TableName { get; set; }
        public string Observation { get; set; }
        public string Afected { get; set; }
        public bool Active { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

<<<<<<< HEAD
        public DateTime DeleteAt { get; set;  }     
        public DateTime CreateAt { get; set; }
=======
       public DateTime dateTime { get; set; }

       public  int UserId { get; set; }
        public User User { get; set; }

           public DateTime DeleteAt { get; set;  }     
            public DateTime CreateAt { get; set; }
>>>>>>> 204fb42e073a15ddd1025a029cf0b0d571d41788

    }
}