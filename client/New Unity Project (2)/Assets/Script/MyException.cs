using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script
{
    class MyException:Exception
    {
        public MyException(string msg)
            : base(msg)
        {
 
        }
    }
}
