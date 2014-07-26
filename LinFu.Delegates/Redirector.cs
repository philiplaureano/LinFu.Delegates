using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinFu.DynamicProxy;

namespace LinFu.Delegates
{
    internal class Redirector : Interceptor
    {
        private readonly Func<object[], object> _target;
        public Redirector(Func<object[], object> target)
        {
            _target = target;
        }
        public override object Intercept(InvocationInfo info)
        {
            return _target(info.Arguments);
        }
    }
}
