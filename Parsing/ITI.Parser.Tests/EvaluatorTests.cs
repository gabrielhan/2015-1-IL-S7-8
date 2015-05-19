using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ITI.Parser.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {

        [TestCase( "3", 3 )]
        [TestCase( "3+8", 3+8 )]
        [TestCase( "3*7/2", 3*7/2 )]
        [TestCase( "3712/(45+98)*12*(58/12)", 3712 / (45 + 98) * 12*( 58 / 12 ) )]
        [TestCase( "37*(12+4)/(45+98/(4+5+68-8-7+10))*1+(41/9*7+6-72)+2*(5+8/1-2)", 37 * (12 + 4) / (45 + 98 / (4 + 5 + 68 - 8 - 7 + 10)) * 1 + (41 / 9 * 7 + 6 - 72) + 2 * (5 + 8 / 1 - 2) )]
        public void test_evaluation( string text, double expectedResult )
        {
            Node n = new Analyser().Analyse( new StringTokenizer( text ) );

            EvalVisitor visitor = new EvalVisitor();
            visitor.VisitNode( n );
            double result = visitor.Result;
            Assert.That( result, Is.EqualTo( expectedResult ) );
        }

    }
}
