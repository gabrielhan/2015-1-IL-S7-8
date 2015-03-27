using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Misc.Tests
{
    [TestFixture]
    public class MyLinqTests
    {
        object[] _objects;
        
        public MyLinqTests()
        {
            _objects = new object[]
            {
                21, 578, 87, 78, "A", "B", "C", "A", 
                "D", "U" , new ListSimpleBottle(), typeof(string), new Exception(),
                new OutOfMemoryException(), this
            };
        }

        public void how_a_list_can_be_filtered()
        {

        }

    }
}
