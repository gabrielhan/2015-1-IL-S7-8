using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MyLinq;

namespace ITI.Misc.Tests
{
    [TestFixture]
    public class MyLinqTests
    {
        object[] _objects;
        
        public MyLinqTests()
        {
            _objects = new object[]
            {
                21, 578, 87, 78, "A", "B", "C", "A", 
                "D", "U" , new ListSimpleBottle(), 
                new ListSimpleBottle(), new ListSimpleBottle(), new ListSimpleBottle(), new ListSimpleBottle(), 
                typeof(string), new Exception(),
                new OutOfMemoryException(), this
            };
        }

        public void how_a_list_can_be_filtered()
        {
            IEnumerable<object> a = _objects;
            {
                IEnumerable<string> s = ToObjectString( a );
                IEnumerable<Type> types = ToObjectTypes( a );
            }
            {
                IEnumerable<string> s = EnumerableExtension.Select( _objects, o => o.ToString() );
                IEnumerable<Type> types = EnumerableExtension.Select( _objects, o => o.GetType() );

                IEnumerable<string> s2 = _objects.Select( o => o.ToString() );
                IEnumerable<Type> types2 = _objects.Select( o => o.GetType() );
            }
            // Filtering for types
            IEnumerable<int> integers = EnumerableExtension.OfType<int>( _objects );
            int sum = EnumerableExtension.Sum( integers );

            IEnumerable<int> bigIntegers = EnumerableExtension.Where( integers, i => i > 2 );
        }

        public void filtering_is_easy()
        {
            Console.WriteLine( "Sum = {0}", _objects.OfType<int>().Sum() );
            foreach( var i in _objects.OfType<int>() )
            {
                //...
            }
            int sumOfEven = _objects
                                .OfType<int>()
                                .Where( i => i % 2 == 0 )
                                .Sum();
            int sumOfOdd = _objects
                                .OfType<int>()
                                .Where( i => i % 2 == 1 )
                                .Sum();
            Console.WriteLine( "even = {0}, odd = {1}", sumOfEven, sumOfOdd );
        }

        private IEnumerable<string> ToObjectString( IEnumerable<object> a )
        {
            foreach( object o in a )
                yield return o.ToString();
        }

        private IEnumerable<Type> ToObjectTypes( IEnumerable<object> a )
        {
            foreach( object o in a )
                yield return o.GetType();
        }

    }
}
