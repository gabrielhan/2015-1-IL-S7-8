using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Bottle.Tests
{
    [TestFixture]
    public class ITIDictionaryTests
    {

        [Test]
        public void dictionary_usage()
        {
            var d = new ITIDictionary<string,object>();
            d.Add( "key", 16.2 );
            d.Add( "key2", null );
            Assert.That( d.Count == 2 );

            Assert.Throws<KeyNotFoundException>( () => Console.Write( d[ "keyKExistePas" ] ) );
            // Adds or updates a key-value pair.
            d["key"] = 15.789;
            bool exist = d.ContainsKey( "key" );
            Assert.That( exist );

            d.Remove( "key" );
            Assert.That( d.Count == 1 );
            d.Clear();
            Assert.That( d.Count == 0 );

            Assert.That( d.ContainsKey( "k" ), Is.False );
            
            object val;
            if( d.TryGetValue( "k", out val ) )
            {
                Assert.Fail( "Key k does not exist." );
            }

            foreach( KeyValuePair<string,object> e in d )
            {
                Console.WriteLine( "{0} -> {1}", e.Key, e.Value );
            }
        }

    }
}
