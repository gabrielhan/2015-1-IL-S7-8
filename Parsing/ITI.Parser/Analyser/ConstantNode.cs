using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class ConstantNode : Node
    {

        public ConstantNode( double value )
        {
            Value = value;
        }

        public double Value { get; private set; }

    }
}
