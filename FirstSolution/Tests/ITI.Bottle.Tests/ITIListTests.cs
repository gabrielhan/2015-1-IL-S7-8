using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Bottle.Tests
{
    [TestFixture]
    public class ITIListTests
    {
        [Test]
        public void enumeration_support()
        {
            ITIList<int> list = new ITIList<int>();
            list.Add( new[] { 1, 2, 3712, 87, 74 } );
            list.Add( 1, 2, 3712, 87, 74 );
            list.Add( 2 );
        }

    }
}
