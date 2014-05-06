using BoolExpInterpreter.Parsing.Visitor;
using System;
using System.Collections.Generic;

namespace BoolExpInterpreter.Parsing
{
    public class Parser
    {

        private List<Token> tokens;
        private int counter;


        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            counter = 0;
        }

        private void Eat(){
            counter++;
        }

        private Boolean End(int lookahead){
            return counter + lookahead >= tokens.Count;
        }

        private Token Current()
        {
            if(End(0)){
                return null;
            }
            return tokens[counter];
        }

        private Token Lookahead(int lookahead)
        {
            if (End(lookahead))
            {
                return null;
            }
            return tokens[counter + lookahead];
        }


        private Boolean EOF()
        {
            return End(0);
        }

        public Exp MainExpression()
        {
            Exp e = Exp();
            if(EOF()){
                return e;
            }
            else {
                Console.WriteLine("Expected EOF, but instead got '" + Current().image + "' at col:" + Current().col);
                return null;
            }
            //EOF
        }

        private Exp nt_ExpOp(Exp mainExp)
        {
                   
                    ExpOp.Op op = ExpOp.Op.NONE;

                    if (Lookahead(1)!=null){
                    TokenType next = Lookahead(1).type;
                        switch (next)
                        {
                            case TokenType.AND:
                                op = ExpOp.Op.AND;
                                break;
                            case TokenType.OR:
                                op = ExpOp.Op.OR;
                                break;
                            case TokenType.EQUALS:
                                op = ExpOp.Op.EQUALS;
                                break;
                        }
                    }

                    //Eat the main expression
                    Eat();
                    
                    if (op == ExpOp.Op.NONE)
                    {
                        return mainExp;
                    }
                    else
                    {
                        //Eat the operator
                        Eat();
                        Exp secExp =  Exp();
                        if (secExp != null)
                            return new ExpOp(mainExp, op, secExp);
                        else return null;
                    }
        }

        private Exp Exp()
        {
            Token current = Current();
            if(current!=null)
            switch(current.type){

                case TokenType.NOT:
                    Eat();
                    if (Current() != null && Current().type == TokenType.LPAREN)
                    {
                        return new ExpNot(Exp());
                    }
                    return Exp();

                case TokenType.TRUE:
                case TokenType.FALSE:

                    
                    Exp mainExp = (current.type==TokenType.TRUE)? (Exp) new ExpTrue(): (Exp) new ExpFalse();

                    if (counter>0 && Lookahead(-1).type == TokenType.NOT) { mainExp = new ExpNot(mainExp); }

                    return nt_ExpOp(mainExp);

                case TokenType.LPAREN:
                    Eat();
                    Exp e = Exp();
                    if (!End(0) && Current().type == TokenType.RPAREN)
                    {
                        //Eat();
                        return nt_ExpOp(e);
                    }
                    else
                    {
                        Console.WriteLine("Missing closing bracket at col:" + Lookahead(-1).col);
                        return null;
                    }

                default:
                    Console.WriteLine("Expected Exp, but got '" + current.image + "' at col:" + current.col);
                    return null;
            };
            String extra = (Lookahead(-1) != null) ? " at col:" + Lookahead(-1).col : "";
            Console.WriteLine("Expected Exp, but got 'EOF'"+extra);
            return null;
        }




    }

}
