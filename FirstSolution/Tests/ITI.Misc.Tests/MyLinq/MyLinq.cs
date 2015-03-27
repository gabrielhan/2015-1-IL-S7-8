using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLinq
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> Where<T>( this IEnumerable<T> objects, Func<T, bool> filter )
        {
            foreach( T o in objects )
            {
                if( filter( o ) ) yield return o;
            }
        }

        public static int Sum( this IEnumerable<int> integers )
        {
            int total = 0;
            foreach( int i in integers ) total += i;
            return total;
        }

        public static IEnumerable<T> OfType<T>( this IEnumerable objects )
        {
            foreach( object o in objects )
            {
                if( o is T ) yield return (T)o;
            }
        }

        public static IEnumerable<TOutput> Select<TInput, TOutput>( this IEnumerable<TInput> a, Func<TInput, TOutput> f )
        {
            foreach( TInput i in a )
                yield return f( i );
        }
    }
}
