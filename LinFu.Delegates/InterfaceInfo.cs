using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinFu.Delegates
{
    internal struct InterfaceInfo
    {
        public InterfaceInfo(Type returnType, Type[] parameters)
        {
            ReturnType = returnType;
            Parameters = parameters;
        }

        public Type ReturnType;
        public Type[] Parameters;
    }
}
