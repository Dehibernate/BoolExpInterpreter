using System;

namespace BoolExpInterpreter.Parsing.Visitor
{
    public  class ExpOp:Exp
    {
        public enum  Op
        {
            AND, 
            OR,
            NOT,
            EQUALS,
            NONE
        }

        public Exp e1, e2;
        public Op op;

        public ExpOp(Exp ae1, Op op, Exp ae2)
        {
            e1 = ae1;
            this.op = op;
            e2 = ae2;
        }

        public override Boolean Accept(IVisitor v)
        {
           return  v.visit(this);
        }

    }
}
