using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinFu.Delegates
{
    public class InterfaceMethodInfo
    {
        public string MethodName { get; set; }

        public Type ReturnType { get; set; }

        public Type[] ArgumentTypes { get; set; }
    }
}
