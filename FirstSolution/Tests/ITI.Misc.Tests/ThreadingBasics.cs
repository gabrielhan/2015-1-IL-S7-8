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

        int[] _theArray;
        object _lockArray = new object();

        public void AddAnIntegerToArray( int i )
        {
            lock( _lockArray )
            {
                if( _theArray == null ) _theArray = new int[] { i };
                else
                {
                    var newArray = new int[_theArray.Length + 1];
                    Array.Copy( _theArray, newArray, _theArray.Length );
                    newArray[_theArray.Length] = i;
                    _theArray = newArray;
                }
            }
        }

        public void AddAnIntegerToArrayLockFree( int i )
        {
            int[] originalValue;
            int[] newValue = null;
            do
            {
                originalValue = _theArray;
                if( originalValue == null ) newValue = new int[] { i };
                else
                {
                    newValue = new int[originalValue.Length + 1];
                    Array.Copy( originalValue, newValue, originalValue.Length );
                    newValue[originalValue.Length] = i;
                }
            }
            while( Interlocked.CompareExchange( ref _theArray, newValue, originalValue ) != originalValue );
        }

        [Test]
        public void Counter_with_lock()
        {
            ParallelExecute( DoSomethingComplicatedWithLock, 500*1000 );
        }

        [Test]
        public void Counter_with_interlocked_increment()
        {
            ParallelExecute( DoSomethingComplicatedWithIncrement, 500*1000 );
        }

        void ParallelExecute( ParameterizedThreadStart start, object startParam )
        {
            Thread[] threads = new Thread[8];
            for( int i = 0; i < threads.Length; i++ )
            {
                threads[i] = new Thread( start );
            }
            _counter = 0;
            for( int i = 0; i < threads.Length; i++ )
            {
                threads[i].Start( startParam );
            }
            for( int i = 0; i < threads.Length; i++ )
            {
                threads[i].Join();
            }
            Console.WriteLine( "It IS over!" );
            Assert.That( _counter, Is.EqualTo( 8 * (int)startParam ) );
        }

        public void DoSomethingComplicatedWithIncrement( object o )
        {
            for( int i = 0; i < (int)o; i++ )
            {
                Interlocked.Increment( ref _counter );
            }
            Console.WriteLine( "Thread : {0} is over.", Thread.CurrentThread.Name );
        }

        public void DoSomethingComplicatedWithLock( object o )
        {
            for( int i = 0; i < (int)o; i++ )
            {
                // Do...
                lock( _lock )
                {
                    _counter++;
                }
            }
            Console.WriteLine( "Thread : {0} is over.", Thread.CurrentThread.Name );
        }

        public void DoSomethingComplicatedWithMonitor( object o )
        {
            int missed = 1;
            for( int i = 0; i < 10000; i++ )
            {
                if( Monitor.TryEnter( _lock, 50 ) )
                {
                    try
                    {
                        _counter += missed;
                        missed = 1;
                    }
                    finally
                    {
                        Monitor.Exit( _lock );
                    }
                }
                else missed++;
            }
            if( missed > 1 )
            {
                Monitor.Enter( _lock );
                try
                {
                    _counter += missed;
                }
                finally
                {
                    Monitor.Exit( _lock );
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
                threads[i] = new Thread( DoSomethingComplicated );
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
            ThreadPool.QueueUserWorkItem( DoSomethingComplicated );
            ThreadPool.QueueUserWorkItem( DoSomethingComplicated );
            ThreadPool.QueueUserWorkItem( DoSomethingComplicated );
            ThreadPool.QueueUserWorkItem( DoSomethingComplicated );
            ThreadPool.QueueUserWorkItem( DoSomethingComplicated );
            ThreadPool.QueueUserWorkItem( DoSomethingComplicated );
            ThreadPool.QueueUserWorkItem( DoSomethingComplicated );
            ThreadPool.QueueUserWorkItem( DoSomethingComplicated );
            DoSomethingComplicated( null );

            Thread.Sleep( 5000 );
            Console.WriteLine( "Should be over!" );
            Assert.That( _counter, Is.EqualTo( 9 * 10000 ) );
        }

        void DoSomethingComplicated( object o )
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
