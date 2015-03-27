using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Misc.Tests
{
    [TestFixture]
    public class ThreadingBasics
    {
        int _counter;
        object _lock = new object();

        [Test]
        public void Counter_with_lock()
        {
            Thread[] threads = new Thread[8];
            for( int i = 0; i < threads.Length; i++ )
            {
                threads[i] = new Thread( DoSomethingComplicatedWithLock );
            }
            _counter = 0;
            for( int i = 0; i < threads.Length; i++ )
            {
                threads[i].Start();
            }
            for( int i = 0; i < threads.Length; i++ )
            {
                threads[i].Join();
            }
            Console.WriteLine( "It IS over!" );
            Assert.That( _counter, Is.EqualTo( 8 * 10000 ) );
        }


        public void DoSomethingComplicatedWithLock( object o )
        {
            for( int i = 0; i < 10000; i++ )
            {
                // Do...
                lock( _lock )
                {
                    _counter++;
                }
            }
            Console.WriteLine( "Thread : {0} is over.", Thread.CurrentThread.Name );
        }


        [Test]
        public void How_to_shoot_yourself_in_the_foot_with_threads()
        {
            Thread[] threads = new Thread[8];
            for( int i = 0; i < threads.Length; i++ )
            {
                threads[i] = new Thread( DoSomethingCompliated );
            }
            _counter = 0;
            for( int i = 0; i < threads.Length; i++ )
            {
                threads[i].Start();
            }
            for( int i = 0; i < threads.Length; i++ )
            {
                threads[i].Join();
            }
            Console.WriteLine( "It IS over!" );
            Assert.That( _counter, Is.EqualTo( 8 * 10000 ) );

        }

        [Test]
        [Explicit]
        public void How_to_shoot_yourself_in_the_foot()
        {
            _counter = 0;
            ThreadPool.QueueUserWorkItem( DoSomethingCompliated );
            ThreadPool.QueueUserWorkItem( DoSomethingCompliated );
            ThreadPool.QueueUserWorkItem( DoSomethingCompliated );
            ThreadPool.QueueUserWorkItem( DoSomethingCompliated );
            ThreadPool.QueueUserWorkItem( DoSomethingCompliated );
            ThreadPool.QueueUserWorkItem( DoSomethingCompliated );
            ThreadPool.QueueUserWorkItem( DoSomethingCompliated );
            ThreadPool.QueueUserWorkItem( DoSomethingCompliated );
            DoSomethingCompliated( null );

            Thread.Sleep( 5000 );
            Console.WriteLine( "Should be over!" );
            Assert.That( _counter, Is.EqualTo( 9 * 10000 ) );
        }

        void DoSomethingCompliated( object o )
        {
            for( int i = 0; i < 10000; i++ )
            {
                // Do...
                _counter++;
            }
            Console.WriteLine( "Thread : {0} is over.", Thread.CurrentThread.Name );
        }


        [Test]
        public void How_to_do_something_elsewhere()
        {
            ThreadPool.QueueUserWorkItem( DoSomething, 2000 );
            Console.WriteLine( "Starting Test" );
            Thread.Sleep( 2500 );
            Console.WriteLine( "Stopping Test" );
        }

        static void DoSomething( object o )
        {
            Console.WriteLine( "Start" );
            int sleepTime = (int)o;
            Thread.Sleep( sleepTime );
            Console.WriteLine( "Stop" );
        }

    }
}
