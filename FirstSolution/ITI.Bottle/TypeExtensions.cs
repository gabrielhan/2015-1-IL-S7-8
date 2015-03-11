using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Bottle
{
    public static class TypeExtensions
    {
        public static bool IsAssignableTo( this Type @this, Type y )
        {
            return y.IsAssignableFrom( @this );
        }
    }
}
