using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATPatients.Models
{
    public class UserInRole
    {
        public UserInRole()
        {
            Users = new List<string>();
        }
        public List<string> Users { get; set; }
    }
}
