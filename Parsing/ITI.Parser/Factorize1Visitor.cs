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
            return n;
        }

        public override Node Visit(UnaryNode n)
        {
            VisitNode(n.Right);
            _currentValue = -_currentValue;
            return new UnaryNode(n.OperatorType,_currentNode);

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
            VisitNode(n.Condition);
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
