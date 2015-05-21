using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public abstract class NodeVisitor
    {
        public void VisitNode( Node n )
        {
            n.Accept( this );
        }


        public virtual void Visit( BinaryNode n )
        {
            VisitNode( n.Left );
            VisitNode( n.Right );
        }

        public virtual void Visit( ConstantNode n )
        {
        }

        public virtual void Visit( ErrorNode n )
        {
        }

        public virtual void Visit( IfNode n )
        {
            VisitNode( n.Condition );
            VisitNode( n.WhenTrue );
            VisitNode( n.WhenFalse );
        }

        public virtual void Visit( UnaryNode n )
        {
            VisitNode( n.Right );
        }


    }
}


namespace gaby.Parser
{
    public abstract class NodeVisitor
    {
        public Node VisitNode(Node n)
        {
            return n.Accept(this);
        }


        public virtual Node Visit(BinaryNode n)
        {
            return new BinaryNode(n.OperatorType,VisitNode(n.Left),VisitNode(n.Right));
        }

        public virtual Node Visit(ConstantNode n)
        {
            return n;
        }

        public virtual Node Visit(ErrorNode n)
        {
            return n;
        }

        public virtual Node Visit(IfNode n)
        {
            return new IfNode(VisitNode(n.Condition),VisitNode(n.WhenTrue),VisitNode(n.WhenFalse));
        }

        public virtual Node Visit(UnaryNode n)
        {
            return new UnaryNode(n.OperatorType,VisitNode(n.Right));
        }


        public virtual Node Visit(VariableNode n)
        {
            return n;
        }
    }
}
