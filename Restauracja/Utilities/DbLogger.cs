using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Utilities
{
    public class DbLogger : LogBase
    {
        public string connStr = "";

        public override void Log(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
