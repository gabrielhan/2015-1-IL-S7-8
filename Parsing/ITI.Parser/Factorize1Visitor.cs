using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    class Factorize1Visitor
    {
    }
}


namespace gaby.Parser
{
    class Factorize1Visitor : NodeVisitor
    {
        double _currentValue;
        bool _isConst;
        string _useAsConst;

        private int Operate(TokenType op, int leftval, int rightval)
        {
            switch (op)
            {
                case TokenType.Minus: return leftval - rightval;
                case TokenType.Plus: return leftval + rightval;
                case TokenType.Div: return leftval / rightval;
                case TokenType.Mult: return leftval * rightval;
                default: return 0;
            }
        }

        public override Node Visit(BinaryNode n)
        {
            Node leftNode = VisitNode(n.Left);
            bool leftTest = _isConst;
            var leftValue = _currentValue;
            Node rightNode = VisitNode(n.Right);
            
            if(leftTest == true && _isConst == true)

            
            return new BinaryNode(n.OperatorType,VisitNode(n.Left),VisitNode(n.Right));
        }

        public override Node Visit(UnaryNode n)
        {
            Node node = new UnaryNode(n.OperatorType, VisitNode(n.Right));
            if (n.OperatorType == TokenType.Minus)
            {
                _currentValue = -_currentValue;
            }
            return node;

        }

        public override Node Visit(ConstantNode n)
        {
            _currentValue = n.Value;
            _isConst = true;
            return n;
        }

        public override Node Visit(VariableNode n)
        {
            if (_useAsConst == n.Value)
            {
                _currentValue = 1;
                _isConst = true;
            }
            else
            {
                _isConst = false;
            }
            return n;
        }

        public override Node Visit(IfNode n)
        {
            // evaluer la condition si const
            Node node = VisitNode(n.Condition);

            // si impossible, lancer un evaluateur sur les deux branches
            if (_currentValue >= 0)
            {
                VisitNode(n.WhenTrue);
            }
            else
            {
                VisitNode(n.WhenFalse);
            }
            return n;
        }
    }
}
