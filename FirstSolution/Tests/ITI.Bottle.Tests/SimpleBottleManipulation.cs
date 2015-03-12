using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Bottle.Tests
{
    [TestFixture]
    public class SimpleBottleManipulation
    {
        [Test]
        public void array_are_easy()
        {
            SimpleBottle[] a = new SimpleBottle[50];
            Assert.That( a.Length, Is.EqualTo( 50 ) );
            Assert.That( a[0], Is.Null );

            //Jagged array
            SimpleBottle[][] a2 = new SimpleBottle[50][];
            for( int i = 0; i < a2.Length; ++i )
            {
                Assert.That( a2[i], Is.Null );
                a2[i] = new SimpleBottle[100];
            }
            Assert.That( a2[0][0], Is.Null );

            // Multidimensional arrays.
            SimpleBottle[,] m2 = new SimpleBottle[50, 100];
            SimpleBottle[,,] m3 = new SimpleBottle[50, 100, 200];

            Assert.That( m2[0, 0], Is.Null );
            Assert.That( m2.GetLength( 0 ), Is.EqualTo( 50 ) );
            Assert.That( m2.GetLength( 1 ), Is.EqualTo( 100 ) );

            Assert.That( m3[10, 20, 58], Is.Null );
        }

        [Test]
        public void our_list()
        {
            var b1 = new SimpleBottle();
            var b2 = new SimpleBottle();
            var b3 = new SimpleBottle();
            ListSimpleBottle list = new ListSimpleBottle();
            Assert.That( list.Count, Is.EqualTo( 0 ) );
            list.Add( b1 );
            Assert.That( list[0] == b1 );
            Assert.That( list[0], Is.SameAs( b1 ) );
            list[0] = b2;
            list[0] = null;

            Assert.Throws<IndexOutOfRangeException>( () => { var x = list[1]; } );
            Assert.Throws<IndexOutOfRangeException>( () => list[1] = null );
        }

        [Test]
        public void optional_parameters()
        {
            ListSimpleBottle l = new ListSimpleBottle();
            Assert.That( l.IndexOf( null ), Is.LessThan( 0 ) );
            Assert.Throws<IndexOutOfRangeException>( () => l.IndexOf( item: null, count: 25 ) );
            Assert.Throws<IndexOutOfRangeException>( () => l.IndexOf( count: 25, item: null ) );
        }

    }
}
