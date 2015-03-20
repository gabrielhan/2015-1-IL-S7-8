using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI.Bottle;
using NUnit.Framework;

namespace ITI.Bottle.Tests
{
    [TestFixture]
    public class JustToCheckItWorks
    {
        [Test]
        public void maxCapacity_is_correctly_initialized()
        {
            int mc;
            {
                var b = new SimpleBottle( 1500 );
                mc = b.MaxCapacity;

                Assert.That( typeof( SimpleBottle ).GetProperty( "MaxCapacity" ).GetSetMethod(), Is.Null, "No Setter Please!" );

                Assert.AreEqual( 1500, mc, "MaxCapacity MUST be correctly initialized." );
                Assert.That( mc, Is.EqualTo( 1500 ), "MaxCapacity MUST be correctly initialized." );
            }
            {
                int b = 1500;
                Assert.That( mc, Is.EqualTo( b ), "MaxCapacity MUST be correctly initialized." );
            }
        }

        [TestCase( -1 )]
        [TestCase( -2 )]
        [TestCase( -45451 )]
        [TestCase( -871 )]
        [ExpectedException( typeof( ArgumentException ) )]
        public void maxCapacity_can_not_be_negative( int faultyCapacity )
        {
            var b = new SimpleBottle( faultyCapacity );
        }

        [Test]
        public void maxCapacity_can_not_be_negative_home_made()
        {
            try
            {
                //throw new FileNotFoundException();
                var b = new SimpleBottle( -1 );
                Assert.Fail( "There MUST be an ArgumentException!" );
            }
            catch( ArgumentException )
            {
            }
            catch( Exception ex )
            {
                // Lyskov Substitution Principle violation:
                // if( ex.GetType() == typeof( AssertionException ) ) throw;
                if( ex is AssertionException ) throw;
                Assert.Fail( "Expected {0} but got a {1}!", "ArgumentException", ex.GetType().FullName );
            }
        }

    }
}
