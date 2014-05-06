using System;

namespace BoolExpInterpreter.Parsing.Visitor
{
    public  class ExpTrue:Exp
    {
        public override Boolean Accept(IVisitor v)
        {
            return v.visit(this);
        }
    }
}
