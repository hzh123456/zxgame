using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class user
    {
        public user(string username,string lastname)
        {
            this.username = username;
            this.lastname = lastname;
        }

        public string username
        {
            get;
            set;
        }
        public string lastname
        {
            get;
            set;
        }
    }
}
