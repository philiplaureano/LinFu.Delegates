using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LinFu.Delegates
{
    public class InterfaceBuilder
    {
        private string _interfaceName;
        private readonly List<InterfaceMethodInfo> _methods = new List<InterfaceMethodInfo>();
        private static readonly ConcurrentDictionary<InterfaceInfo, Type> Cache = new ConcurrentDictionary<InterfaceInfo, Type>();

        public InterfaceBuilder(string interfaceName)
        {
            InterfaceName = interfaceName;
        }

        public List<InterfaceMethodInfo> Methods
        {
            get { return _methods; }
        }

        public string InterfaceName
        {
            get { return _interfaceName; }
            set { _interfaceName = value; }
        }

        public Type CreateInterface()
        {
            var assemblyName = new AssemblyName
            {
                Name = Guid.NewGuid().ToString()
            };

            var moduleName = assemblyName.Name;
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName,
                AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName, String.Format("{0}.mod", moduleName),
                true);

            var attributes = TypeAttributes.Public | TypeAttributes.Interface |
                             TypeAttributes.AutoClass | TypeAttributes.AnsiClass
                             | TypeAttributes.Abstract;

            var typeBuilder = moduleBuilder.DefineType(_interfaceName, attributes);

            var methodAttributes = MethodAttributes.Public | MethodAttributes.NewSlot
                                   | MethodAttributes.HideBySig | MethodAttributes.Virtual |
                                   MethodAttributes.Abstract;

            foreach (var info in _methods)
            {
                var method = typeBuilder.DefineMethod(info.MethodName, methodAttributes, info.ReturnType,
                    info.ArgumentTypes);
                method.SetImplementationFlags(MethodImplAttributes.Managed | MethodImplAttributes.IL);
            }

            return typeBuilder.CreateType();
        }

        public void AddMethod(string methodName, Type returnType, Type[] parameters)
        {
            var info = new InterfaceMethodInfo
            {
                MethodName = methodName,
                ReturnType = returnType,
                ArgumentTypes = parameters
            };

            _methods.Add(info);
        }

        public static Type DefineInterfaceMethod(Type returnType, Type[] parameters)
        {
            // Reuse the previously cached results
            var cacheKey = new InterfaceInfo(returnType, parameters);
            if (Cache.ContainsKey(cacheKey))
                return Cache[cacheKey];

            var result = CreateInterface(returnType, parameters);

            // Cache the results
            if (result != null)
                Cache[cacheKey] = result;

            return result;
        }

        private static Type CreateInterface(Type returnType, Type[] parameters)
        {
            var interfaceName = "IAnonymous";
            var methodName = "AnonymousMethod";
            return CreateInterface(interfaceName, methodName, returnType, parameters);
        }

        private static Type CreateInterface(string interfaceName, string methodName, Type returnType,
            Type[] parameters)
        {
            var builder = new InterfaceBuilder(interfaceName);
            builder.AddMethod(methodName, returnType, parameters);

            return builder.CreateInterface();
        }
    }
}
