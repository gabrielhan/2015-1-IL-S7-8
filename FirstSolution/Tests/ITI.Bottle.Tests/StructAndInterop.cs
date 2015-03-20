using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Bottle.Tests
{
    [TestFixture]
    public class StructAndInterop
    {

        [StructLayout(LayoutKind.Explicit)]
        struct IPAddress
        {
            [FieldOffset(0)]
            public uint Value;

            [FieldOffset(0)]
            public byte B1;

            [FieldOffset(1)]
            public byte B2;

            [FieldOffset(2)]
            public byte B3;

            [FieldOffset( 3 )]
            public byte B4;
        }

        [Test]
        public void how_int_is_layered()
        {
            IPAddress a = new IPAddress();
            a.Value = 3712;
            Console.WriteLine( a.Value );
            Console.WriteLine( "{0}.{1}.{2}.{3}", a.B1, a.B2, a.B3, a.B4 );
            Console.WriteLine( "Little Endian!" );
        }
    }
}
