using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinFu.Delegates
{
    public static class Args
    {
        private static readonly OpenArgs OpenArgs = new OpenArgs();

        public static OpenArgs Open
        {
            get { return OpenArgs; }
        }
    }
}
