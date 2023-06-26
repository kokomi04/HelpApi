using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Applications
{
    public static class AplicationAssembly
    {
        public static Assembly Assembly => typeof(AplicationAssembly).Assembly;
    }
}
