using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinFu.Delegates
{
    internal struct DelegateInfo
    {
        public DelegateInfo(Type returnType, Type[] parameterTypes)
        {
            if (returnType == null)
                throw new ArgumentNullException("returnType");

            if (parameterTypes == null)
                throw new ArgumentNullException("parameterTypes");

            ReturnType = returnType;
            Parameters = parameterTypes;
        }

        public readonly Type ReturnType;
        public readonly Type[] Parameters;

        public override bool Equals(object obj)
        {
            if (!(obj is DelegateInfo))
                return false;

            var info = (DelegateInfo)obj;

            return Compare(this, info);
        }

        public override int GetHashCode()
        {
            return ReturnType.GetHashCode() ^ Parameters.GetHashCode();
        }

        private static bool Compare(DelegateInfo lhs, DelegateInfo rhs)
        {
            return lhs.ReturnType == rhs.ReturnType &&
                   CompareParameters(lhs, rhs.Parameters);
        }

        private static bool CompareParameters(DelegateInfo info, Type[] parameterTypes)
        {
            if (info.Parameters == null && parameterTypes == null)
                return true;

            if (info.Parameters == null || info.Parameters.Length != parameterTypes.Length)
                return false;

            for (var position = 0; position < parameterTypes.Length; position++)
            {
                if (info.Parameters[position] != parameterTypes[position])
                    return false;
            }

            return true;
        }
    }
}
