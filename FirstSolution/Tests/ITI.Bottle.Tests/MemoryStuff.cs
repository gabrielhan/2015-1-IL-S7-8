using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Bottle.Tests
{
    [TestFixture]
    public class MemoryStuff
    {
        [Test]
        public void simple_string_concatenation()
        {
            Assert.That( CreateString( 'a', 10 ), Is.EqualTo( "aaaaaaaaaa" ) );
        }

        const int maxLoop = 100 * 1000;

        [Test]
        [Explicit]
        public void simple_string_concatenation_performance()
        {
            Stopwatch w = new Stopwatch();

            for( int len = 1000; len < maxLoop; len += 10000 )
            {
                w.Restart();
                CreateString( 'a', len );
                w.Stop();
                Console.WriteLine( "{0:## ### ### ###} - {1}", len, w.ElapsedTicks );
            }
        }

        [Test]
        [Explicit]
        public void better_simple_string_concatenation_performance()
        {
            Stopwatch w = new Stopwatch();

            for( int len = 1000; len < maxLoop; len += 10000 )
            {
                w.Restart();
                CreateStringBetter( 'a', len );
                w.Stop();
                Console.WriteLine( "{0:## ### ### ###} - {1}", len, w.ElapsedTicks );
            }
        }

        static private string CreateString( char c, int n )
        {
            string s = String.Empty;
            for( int i = 0; i < n; i++ )
            {
                s = s + c;
            }
            return s;
        }

        static private string CreateStringBetter( char c, int n )
        {
            StringBuilder b = new StringBuilder();
            for( int i = 0; i < n; i++ )
            {
                b.Append( c );
            }
            return b.ToString();
        }
        
        static private string CreateStringUltimate( char c, int n )
        {
            return new String( c, n );
        }
    }
}
