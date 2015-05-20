using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace ITI.Parser
{
   
}



namespace gaby.Parser
{
    public class VariableNode : Node
    {

        public VariableNode(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }


        [DebuggerStepThrough]
        internal override void Accept(NodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Value;
        }

    }
}
