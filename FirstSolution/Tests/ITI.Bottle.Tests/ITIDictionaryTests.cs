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


        [Test]
        public void dictionary_enumerator_empty_works()
        {
            var d = new ITIDictionary<string, object>();
            Assert.That( d.Count, Is.EqualTo( 0 ) );
            foreach( var kv in d )
            {
                Assert.Fail( "Never here!!" );
            }
        }

        [Test]
        public void dictionary_enumerator_works()
        {
            var d = new ITIDictionary<int, int>();
            d.Add( 1, 10 );
            d.Add( 2, 20 );
            d.Add( 3, 30 );
            d.Add( 4, 40 );
            d.Add( 5, 50 );
            int sumKey = 0;
            int sumValue = 0;
            foreach( var kv in d )
            {
                sumKey += kv.Key;
                sumValue += kv.Value;
            }
            Assert.That( sumKey, Is.EqualTo( 1 + 2 + 3 + 4 + 5 ) );
            Assert.That( sumValue, Is.EqualTo( 10 + 20 + 30 + 40 + 50 ) );
        }

        public void playing_with_enumerations()
        {
            // A storage for a Query string:
            Dictionary<string,List<string>> qs = new Dictionary<string, List<string>>();
            qs.Add( "q", new List<string>() { "1", "2", "3" } );
            qs.Add( "name", new List<string>() { "John", "Paul", "Albert" } );
            qs.Add( "weight", new List<string>() { "12", "87" } );
            qs.Add( "plus", new List<string>() { "1" } );

            // Want to extract KeyValuePair<string,string> from it.
            foreach( KeyValuePair<string,string> parameters in qs.ExpandedParameters() )
            {
                // "plus" "1"
                // "q" "1"
                // "q" "2"
                // "q" "2"
                // "weight" "12"
                // "weight" "87"
                // "name" "John"
                // "name" "Paul"
                // "name" "Albert"
            }

        }

    }
}
