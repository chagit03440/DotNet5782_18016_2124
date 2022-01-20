using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public struct User
    { 
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool Worker;
            public bool IsActive ;
            //public IEnumerable<User> ListOfUser { get; set; }

            //public override string ToString() => this.ToStringProperty();
      }
}
 
 
