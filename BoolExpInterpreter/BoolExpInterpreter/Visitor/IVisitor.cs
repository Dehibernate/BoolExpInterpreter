using System;

namespace BoolExpInterpreter.Parsing.Visitor
{
    public interface IVisitor
    {
        Boolean visit(ExpOp element);
        Boolean visit(ExpTrue element);
        Boolean visit(ExpFalse element);
        Boolean visit(ExpNot element);
    }
}
