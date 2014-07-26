using System.Collections.Generic;
using System.Reflection;

namespace LinFu.Delegates.Invokers
{
    internal class DefaultInvoker : IInvoker
    {
        public object Invoke(object target, MethodBase targetMethod, IEnumerable<object> curriedArguments,
            IEnumerable<object> invokeArguments)
        {
            var combinedArguments = GetCombinedArguments(targetMethod, curriedArguments, invokeArguments);

            var parameters = targetMethod.GetParameters();
            var parameterCount = parameters.Length;

            var args = new object[parameterCount];
            for (var i = 0; i < parameterCount; i++)
            {
                args[i] = combinedArguments[i];
            }

            // HACK: Coerce the argument list into an object array if
            // necessary
            var isObjectArray = parameters.Length == 1 &&
                                parameters[0].ParameterType == typeof(object[]);

            if (isObjectArray)
                args = combinedArguments.ToArray();

            object result = null;
            try
            {
                result = targetMethod.Invoke(target, args);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }

            return result;
        }

        private static List<object> GetCombinedArguments(MethodBase targetMethod,
            IEnumerable<object> curriedArguments, IEnumerable<object> invokeArguments)
        {
            var parameters = targetMethod.GetParameters();
            var parameterCount = parameters.Length;


            var combinedArguments = new List<object>();
            for (var i = 0; i < parameterCount; i++)
            {
                combinedArguments.Add(Args.Open);
            }

            AssignArguments(curriedArguments, combinedArguments);
            AssignArguments(invokeArguments, combinedArguments);

            return combinedArguments;
        }

        private static void AssignArguments(IEnumerable<object> sourceList, IList<object> targetList)
        {
            var source = new Queue<object>(sourceList);
            for (var i = 0; i < targetList.Count; i++)
            {
                if (source.Count == 0)
                    break;

                if (targetList[i] != Args.Open)
                    continue;

                var argument = source.Dequeue();

                if (argument is IDeferredArgument)
                {
                    var deferred = (IDeferredArgument)argument;
                    argument = deferred.Evaluate();
                }

                targetList[i] = argument;
            }
        }
    }
}