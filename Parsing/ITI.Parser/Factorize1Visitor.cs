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
        Node _currentNode;
        bool _isConst;
        string _useAsConst;

        public Node Result { get { return _currentNode; } }

        public override void Visit(BinaryNode n)
        {
           
        }

        public override void Visit(UnaryNode n)
        {
            VisitNode(n.Right);
            _currentValue = -_currentValue;
            _currentNode = new UnaryNode(n.OperatorType,_currentNode);
        }

        public override void Visit(ConstantNode n)
        {
            _currentValue = n.Value;
            _currentNode = n;
            _isConst = true;
        }

        public override void Visit(VariableNode n)
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
            _currentNode = n;
        }

        public override void Visit(IfNode n)
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
        }
    }
}
