using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class BinaryNode : Node
    {

        public BinaryNode( TokenType operatorType, Node left, Node right )
        {
            OperatorType = operatorType;
            Left = left;
            Right = right;
        }

        public TokenType OperatorType { get; private set; }
        public Node Left { get; private set; }
        public Node Right { get; private set; }

    }
}
