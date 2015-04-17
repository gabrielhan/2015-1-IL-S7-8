using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Misc
{
    [TestFixture]
    public class StreamManipulation
    {
        const string fileToRead = @"E:\Intech\2015-1\S7-8\Dev\2015-1-IL-S7-8\FirstSolution\ITI.Misc\StreamManipulation.cs"; 

        [Test]
        public void reading_a_file()
        {
            using( var s = new FileStream( fileToRead, FileMode.Open, FileAccess.Read, FileShare.None ) )
            {
                Assert.Th
            }

        }

    }
}
