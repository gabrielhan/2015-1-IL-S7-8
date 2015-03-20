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
    public class Enumeration
    {
        interface IEnumerator : IDisposable
        {
            object Current { get; }

            bool MoveNext();
        }

        interface IEnumerator<T> : IEnumerator
        {
            T Current { get; }

            bool MoveNext();
        }

        interface IEnumerable
        {
            IEnumerator GetEnumerator();
        }
        
        interface IEnumerable<T> : IEnumerable
        {
            IEnumerator<T> GetEnumerator();
        }


        class Bag : IEnumerable<int>
        {

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<int> GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void simple_home_made_enumeration()
        {
            Bag b = new Bag();

            // La totale...
            IEnumerator e = null;
            try
            {
                e = b.GetEnumerator();
                while( e.MoveNext() )
                {
                    int o = (int)e.Current;
                    Console.WriteLine( o );
                }
            }
            finally
            {
                if( e != null ) e.Dispose();
            }

            // Intermediate: using the using keyword...
            using( IEnumerator e2 = b.GetEnumerator() )
            {
                while( e2.MoveNext() )
                {
                    int o = (int)e2.Current;
                    Console.WriteLine( o );
                }
            }

            // Much more easy...
            foreach( int o in b )
            {
                Console.WriteLine( o );
            }
        }






    }
}
