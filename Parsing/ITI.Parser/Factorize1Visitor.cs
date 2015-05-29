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

        public override Node Visit(BinaryNode n)
        {
            Node node = VisitNode(n.Left);
            bool leftTest = _isConst;


            switch(n.OperatorType)
            {
                case TokenType.Minus: break;
                case TokenType.Plus: break;
                case TokenType.Div: break;
                case TokenType.Mult: break;
            }
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
