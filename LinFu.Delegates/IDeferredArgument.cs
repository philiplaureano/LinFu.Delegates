using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinFu.Delegates
{
    public interface IDeferredArgument
    {
        object Evaluate();
    }
}
