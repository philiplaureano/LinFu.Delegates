using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinFu.Delegates
{
    public static class DelegateExtensions
    {
        public static TDelegate DefineDelegate<TDelegate>(this Func<object[], object> methodBody)
            where TDelegate : class
        {
            var delegateType = typeof(TDelegate);
            if (!typeof(MulticastDelegate).IsAssignableFrom(delegateType))
                throw new InvalidOperationException("TDelegate must derive from System.MulicastDelegate");

            return DefineDelegate(methodBody, delegateType) as TDelegate;
        }

        public static MulticastDelegate DefineDelegate(this Func<object[], object> methodBody, Type delegateType)
        {
            return DelegateFactory.DefineDelegate(delegateType, methodBody);
        }
    }
}
