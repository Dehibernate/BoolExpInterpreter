using System;
using BoolExpInterpreter.Parsing.Visitor;


namespace BoolExpInterpreter.Parsing
{
    public class Interpreter : IVisitor
    {

        private Exp MainExp;

        public Interpreter(Exp exp)
        {
            MainExp = exp;
        }

        public Boolean interpret()
        {
            return MainExp.Accept(this);
        }

        public Boolean visit(ExpOp exp)
        {
            Boolean e1 = exp.e1.Accept(this);
            
            if (exp.op == ExpOp.Op.AND && e1 == false) { return false; }

            Boolean e2 = exp.e2.Accept(this);
            switch(exp.op){
                case ExpOp.Op.AND:
                    return e1 && e2;
                case ExpOp.Op.OR:
                    return e1 || e2;
                case ExpOp.Op.EQUALS:
                    return e1 == e2;
            }
            throw new Exception("Unexpected error...");
        }

        public Boolean visit(ExpTrue exp)
        {
            return true;
        }

        public Boolean visit(ExpFalse exp)
        {
            return false;
        }

        public Boolean visit(ExpNot exp)
        {
            return !exp.exp.Accept(this);
        }
    }
}
