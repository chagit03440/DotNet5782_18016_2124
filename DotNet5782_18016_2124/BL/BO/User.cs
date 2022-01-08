using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class User
    { 
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool Worker;
            public bool IsActive = true;
            public IEnumerable<User> ListOfUser { get; set; }

            //public override string ToString() => this.ToStringProperty();
      }
}
 
 
