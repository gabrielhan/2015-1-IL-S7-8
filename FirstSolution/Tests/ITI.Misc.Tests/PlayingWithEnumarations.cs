using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Misc.Tests
{
    [TestFixture]
    public class PlayingWithEnumarations
    {

        [Test]
        public void infinite_enumeration()
        {
            foreach( var n in FibonacciNumbers() )
            {
                Console.WriteLine( n );
            }
        }

        public IEnumerable<long> FibonacciNumbers()
        {
            checked
            {
                long n1, n2;
                // Stupid block (assigments, without cast, can not overflow).
                // Just to show the unchecked keyword....
                unchecked
                {
                    n1 = 0;
                    n2 = 1;
                }
                for( ; ; )
                {
                    long n3 = n1 + n2;
                    n1 = n2;
                    n2 = n3;
                    yield return n3;
                }
            }
        }

    }
}
