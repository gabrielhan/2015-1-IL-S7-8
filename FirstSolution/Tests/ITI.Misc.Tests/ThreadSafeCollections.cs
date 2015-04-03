using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Misc.Tests
{
    [TestFixture]
    public class ThreadSafeCollections
    {

        class Dog
        {
            public string Name;
            public Bone Nonosse;
        }

        class Bone
        {
            public int Size;
        }

        public void what_is_used()
        {
            var d1 = CreateDog( "Médor" );
            _lastCreated = d1;
        }

        static Dog _lastCreated = null;

        private static Dog CreateDog()
        {
            var d = new Dog();
            var b = new Bone();
            d.Nonosse = b;
            return d;
        }

        private static Dog CreateDog( string name )
        {
            var d1 = CreateDog();
            d1.Name = name;
            return d1;
        }

        public void using_a_concurent_Dictionnary()
        {
            ConcurrentDictionary<string,string> d = new ConcurrentDictionary<string, string>();
            if( d.TryAdd( "toto", "tata" ) )
            {
                //Assert.That( d.ContainsKey( "toto" ) );
            }
            d.AddOrUpdate( "kk", "newValue", ( o, n ) => o + n );
            d.AddOrUpdate( "kk", "v2", ( o, n ) => o + n );

            string shouldBeVal = d.GetOrAdd( "key", "val" );

            foreach( var k in d.Keys )
            {

            }
        }

    }
}
