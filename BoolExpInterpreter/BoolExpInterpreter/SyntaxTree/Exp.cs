using System;

namespace BoolExpInterpreter.Parsing.Visitor
{
    public abstract class Exp
    {
        public abstract Boolean Accept(IVisitor visitor);

        
    }
}
