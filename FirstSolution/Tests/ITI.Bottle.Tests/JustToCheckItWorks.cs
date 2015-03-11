using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ITI.Bottle;
using System.IO;

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
                //var b = new SimpleBottle( -1 );
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

        interface IRunnable
        {
            void Run();
        }

        class TestLeTruc : IRunnable
        {
            public void Run()
            {
                var b = new SimpleBottle( -78 );
            }
        }

        [Test]
        public void runnable_throws_ArgumentException()
        {
            AssertThrowsArgumentException( new TestLeTruc() );
        }

        //[Test]
        //public void runnable_throws_ArgumrntException_JAVA()
        //{
        //    AssertThrowsArgumentException( new IRunnable
        //    {
        //        void Run() {
        //             var b = new SimpleBottle( -78 );
        //        }
        //    } );
        //}

        void AssertThrowsArgumentException( IRunnable test )
        {
            try
            {
                test.Run();
                Assert.Fail( "There MUST be an ArgumentException!" );
            }
            catch( ArgumentException )
            {
            }
            catch( Exception ex )
            {
                if( ex is AssertionException ) throw;
                Assert.Fail( "Expected {0} but got a {1}!", "ArgumentException", ex.GetType().FullName );
            }
        }

        static void Truc()
        {
            var b = new SimpleBottle( -1 );
        }

        [Test]
        public void runnable_throws_ArgumentException_better_but_with_a_named_function()
        {
            AssertThrowsArgumentException( Truc );
        }

        [Test]
        public void runnable_throws_ArgumentException_better_with_an_anonymous_function()
        {
            int i = 0;
            AssertThrowsArgumentException( delegate()
            {
                ++i;
                var b = new SimpleBottle( -1 );
                ++i;
            } );
            Assert.That( i, Is.EqualTo( 1 ) );
        }

        [Test]
        public void runnable_throws_ArgumentException_better_with_a_lambda_function()
        {
            AssertThrowsArgumentException( () =>
            {
                var b = new SimpleBottle( -1 );
            } );
        }

        private void AssertThrowsArgumentException( Action test )
        {
            try
            {
                test();
                Assert.Fail( "There MUST be an ArgumentException!" );
            }
            catch( Exception ex )
            {
                if( ex is ArgumentException ) return;
                if( ex is AssertionException ) throw;
                Assert.Fail( "Expected {0} but got a {1}!", "ArgumentException", ex.GetType().FullName );
            }
        }

        [Test]
        public void runnable_throws_ArgumentException_better_with_a_lambda_function_and_generic()
        {
            AssertThrows<ArgumentException>( () =>
            {
                var b = new SimpleBottle( -1 );
            } );
        }

        private void AssertThrows<T>( Action test ) where T : Exception
        {
            if( typeof( AssertionException ).IsAssignableFrom( typeof( T ) ) ) throw new ArgumentException( "T must NOT be an AssertionException!" );
            
            if( TypeExtensions.IsAssignableTo( typeof( T ), typeof( AssertionException ) ) ) throw new ArgumentException( "T must NOT be an AssertionException!" );
            
            if( typeof(T).IsAssignableTo( typeof(AssertionException) ) ) throw new ArgumentException( "T must NOT be an AssertionException!" );
            
            try
            {
                test();
                Assert.Fail( "There MUST be an ArgumentException!" );
            }
            catch( Exception ex )
            {
                if( ex is T ) return;
                if( ex is AssertionException ) throw;
                Assert.Fail( "Expected {0} but got a {1}!", typeof(T).Name, ex.GetType().FullName );
            }
        } 
        
        #region Topo on lambda functions
        delegate void ActionInt( int i );
        delegate void ActionString( string i );
        delegate void ActionStringInt( string s, int i );
        delegate void ActionSimpleBottle( SimpleBottle b );

        static void SampleFuncAndActions()
        {
            Action<SimpleBottle,int> a = delegate( SimpleBottle b, int i ) { Console.Write( b.MaxCapacity * i ); };
            Func<SimpleBottle> f = delegate() { return new SimpleBottle( 500 ); };
            Func<SimpleBottle,string,SimpleBottle> f2 = delegate( SimpleBottle b, string name ) { return new SimpleBottle( 500 ); };

            Action<SimpleBottle,int> la = ( b, i ) => Console.Write( b.MaxCapacity * i );
            Func<SimpleBottle> lf = () => new SimpleBottle( 500 );
            Func<SimpleBottle,string,SimpleBottle> lf2 = ( b, name ) => new SimpleBottle( 500 );
        }
        #endregion
        




    }
}
