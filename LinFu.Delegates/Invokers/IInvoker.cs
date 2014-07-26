using System.Collections.Generic;
using System.Reflection;

namespace LinFu.Delegates.Invokers
{
    public interface IInvoker
    {
        object Invoke(object target, MethodBase targetMethod, IEnumerable<object> curriedArguments,
            IEnumerable<object> invokeArguments);
    }
}
