using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LinFu.Delegates.Invokers;

namespace LinFu.Delegates.Partial
{
    public class PartialApplication : IDeferredArgument
    {
        private readonly MulticastDelegate _target;
        private readonly IInvoker _invoker = new DefaultInvoker();
        private readonly List<object> _suppliedArguments = new List<object>();

        public PartialApplication()
        {
        }

        public PartialApplication(Func<object[], object> body, Type returnType,
            Type[] parameterTypes, params object[] suppliedArguments)
        {
            var targetDelegate = DelegateFactory.DefineDelegate(body, returnType, parameterTypes);
            _target = targetDelegate;

            if (suppliedArguments == null || suppliedArguments.Length == 0)
                return;

            _suppliedArguments.AddRange(suppliedArguments);
        }

        public PartialApplication(Func<object[], object> target, params object[] suppliedArguments)
            : this((MulticastDelegate)target)
        {
            var customDelegate = target;
            _invoker = new DelegateInvoker(customDelegate);
            _suppliedArguments.AddRange(suppliedArguments);
        }

        public PartialApplication(MethodInfo staticMethod,
            params object[] suppliedArguments)
        {
            if (!staticMethod.IsStatic)
                throw new ArgumentException("The target method must be static and it cannot be an instance method",
                    "staticMethod");

            var target = DelegateFactory.DefineDelegate(null, staticMethod);
            _target = target;

            if (suppliedArguments == null || suppliedArguments.Length == 0)
                return;

            _suppliedArguments.AddRange(suppliedArguments);
        }

        public PartialApplication(object instance, MethodInfo targetMethod,
            params object[] suppliedArguments)
        {
            var target = DelegateFactory.DefineDelegate(instance, targetMethod);
            _target = target;

            if (suppliedArguments == null || suppliedArguments.Length == 0)
                return;

            _suppliedArguments.AddRange(suppliedArguments);
        }

        public PartialApplication(MulticastDelegate target)
        {
            _target = target;
        }

        public PartialApplication(MulticastDelegate target, params object[] suppliedArguments)
        {
            _target = target;

            if (suppliedArguments == null || suppliedArguments.Length == 0)
                return;


            _suppliedArguments.AddRange(suppliedArguments);
        }

        public List<object> Arguments
        {
            get { return _suppliedArguments; }
        }

        public MulticastDelegate Target { get; set; }

        public IInvoker Invoker { get; set; }

        public object Invoke(params object[] args)
        {
            if (_target == null)
                throw new NotImplementedException();

            if (_invoker == null)
                throw new NotImplementedException();

            return _invoker.Invoke(_target.Target, _target.Method, _suppliedArguments, args);
        }

        public object Evaluate()
        {
            return Invoke();
        }

        public TDelegate AdaptTo<TDelegate>()
            where TDelegate : class
        {
            return AdaptTo(typeof(TDelegate)) as TDelegate;
        }

        public MulticastDelegate AdaptTo(Type delegateType)
        {
            if (!typeof(MulticastDelegate).IsAssignableFrom(delegateType))
                throw new ArgumentException("Generic parameter 'TDelegate' must be derived from MulticastDelegate");

            // Create a 'fake' delegate that redirects its
            // calls back to this closure
            Func<object[], object> body = Invoke;

            var result = DelegateFactory.DefineDelegate(delegateType, body);
            return result;
        }
    }
}
