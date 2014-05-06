using System;

namespace BoolExpInterpreter.Parsing.Visitor
{
    public  class ExpNot:Exp
    {
        public Exp exp;

        public ExpNot(Exp e)
        {
            exp = e;
        }

        public override Boolean Accept(IVisitor v)
        {
            return v.visit(this);
        }
    }
}
