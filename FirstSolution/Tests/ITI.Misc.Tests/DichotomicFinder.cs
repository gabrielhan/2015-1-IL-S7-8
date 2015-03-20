using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Misc.Tests
{
    [TestFixture]
    public class DichotomicFinder
    {
        [Test]
        public void Dichoto()
        {
            var integers = new[] { 15, 587, 98797, -54, 21125, 687, 545, 218, 645987 };

            Array.Sort( integers );
            int idx = Array.BinarySearch( integers, 687 );
            Assert.That( idx, Is.EqualTo( 5 ) );

            int idxNotFound = Array.BinarySearch( integers, 688 );
            if( idxNotFound < 0 )
            {
                int toInsertAt = ~idxNotFound;

            }

            Assert.That( idxNotFound, Is.LessThan( 0 ) );

            int idxNotFound2 = Array.BinarySearch( integers, -10000 );
            Assert.That( idxNotFound2, Is.EqualTo( -1 ) );


        }


    }
}
