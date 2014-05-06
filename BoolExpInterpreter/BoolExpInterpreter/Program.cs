using BoolExpInterpreter.Lexing;
using BoolExpInterpreter.Parsing;
using BoolExpInterpreter.Parsing.Visitor;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BoolExpInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            String welcome = "BOOLEAN INTERPRETER";
            Console.CursorLeft = Console.BufferWidth / 2 - welcome.Length / 2;
            Console.Write(welcome);
            Console.CursorLeft = Console.BufferWidth - 3;
            Console.WriteLine("AI");
            for (int i = 0; i < Console.BufferWidth; i++) { Console.Write("="); }
            Console.WriteLine("\nPlease write a boolean expression to be evaluated or write ':quit' to exit:\n");
            String expression = Console.ReadLine();
            Stopwatch sw = Stopwatch.StartNew();
            while (expression != ":quit")
            {
                if (expression.Length > 0)
                {

                    //Console.WriteLine("\nParsing...\n");
                    List<Token> tokens = new Lexer(expression).Lex();

                    if (tokens != null)
                    {

                        Parser parser = new Parser(tokens);
                        Exp Expression = parser.MainExpression();

                        if (Expression != null)
                        {

                            //Console.WriteLine("Parsing completed.");

                            //Console.WriteLine("Tokens Identified:\n=================");
                            //foreach (var token in tokens)
                            //{
                            //    Console.WriteLine(token.type + " - " + token.image);
                            //}

                            Interpreter interpreter = new Interpreter(Expression);
                            Boolean result = interpreter.interpret();
                            Console.WriteLine("\nResult: " + result);
                        }
                    }
                    sw.Stop();
                    Console.WriteLine("Done in " +( (sw.ElapsedMilliseconds == 0) ? "<" + sw.ElapsedMilliseconds : ""+sw.ElapsedMilliseconds )+ " ms.");
                    sw.Reset();
                    Console.WriteLine("\n==============\n");
                }    
                    expression = Console.ReadLine();
                
            }
        }
    }
}
