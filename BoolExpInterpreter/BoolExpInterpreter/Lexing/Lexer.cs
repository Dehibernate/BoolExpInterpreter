using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpInterpreter.Lexing
{
    public class Lexer
    {
        private List<Matcher> matchers;
        private Tokenizer tokenizer;

        public Lexer(String stream)
        {
            tokenizer = new Tokenizer(stream);

        }

        public List<Token> Lex()
        {
            List<Token> Tokens = new List<Token>();

            matchers = InitializeMatchers();

            //Gets next token
            var current = Next();

           

            if (current == null) { Console.WriteLine("Syntax Error. Unacceptable token at col:1"); return null; }
            

            while ( current.type != TokenType.EOF)
            {
                if (current.type != TokenType.WHITESPACE)
                {
                    //yield return current;
                    Tokens.Add(current);
                }
                current = Next();
                
                
                if (current == null) { Console.WriteLine("Syntax Error. Unacceptable token after '" + Tokens[Tokens.Count - 1].image + "' at col:" + Tokens[Tokens.Count - 1].col); return null; }
                
            }
            return Tokens;
        }

        private Token Next()
        {
            if (tokenizer.End())
            {
                return new Token(TokenType.EOF, tokenizer.index);
            }

            return (from match in matchers
                    let token = match.isMatch(tokenizer)
                    where token != null
                    select token).FirstOrDefault();
        }

        private List<Matcher> InitializeMatchers()
        {
            List<Matcher> matchers = new List<Matcher>();

            List<BoolValue> boolMatchers = new List<BoolValue>
                {
                    new BoolValue(TokenType.TRUE,"True"),
                    new BoolValue(TokenType.FALSE,"False"),
                };

            List<Operand> operandMatchers = new List<Operand>
                {
                    new Operand(TokenType.AND,"&&"),
                    new Operand(TokenType.OR,"||"),
                    new Operand(TokenType.NOT,"!"),
                    new Operand(TokenType.EQUALS,"==")
                };

            List<Bracket> bracketMatchers = new List<Bracket>
                {
                    new Bracket(TokenType.LPAREN,"("),
                    new Bracket(TokenType.RPAREN,")"),

                };

            List<Matcher> SpecialCharacters = new List<Matcher>();
            SpecialCharacters.AddRange(operandMatchers);
            SpecialCharacters.AddRange(bracketMatchers);

            boolMatchers.ForEach(BMatcher =>
                BMatcher.SpecialCharacters = SpecialCharacters.Select(i => i as Matcher).ToList());

            matchers.AddRange(operandMatchers);
            matchers.AddRange(bracketMatchers);
            matchers.AddRange(boolMatchers);

            matchers.Add(new WhiteSpace());

            return matchers;
        }

        private class Tokenizer : TokenStream<String>
        {
            public Tokenizer(String source)
                : base(source.ToCharArray().Select(i => i.ToString()).ToList())
            { }
        }

        private class TokenStream<T> where T : class
        {

            private List<T> stream;
            public int index;
            private Stack<int> snapshots;

            public TokenStream(List<T> stream)
            {
                this.stream = stream;
                index = 0;
                snapshots = new Stack<int>();
            }

            public Boolean End()
            {
                return EOF(0);
            }

            public Boolean EOF(int lookahead)
            {
                return (lookahead + index >= stream.Count);
            }

            public virtual T Current
            {
                get
                {
                    if (EOF(0))
                    {
                        return null;
                    }
                    return stream[index];
                }
            }

            public T Peek(int lookahead)
            {
                if (EOF(lookahead))
                {
                    return null;
                }
                return stream[index + lookahead];
            }

            public void Eat()
            {
                index++;
            }

            public void takeSnapshot()
            {
                snapshots.Push(index);
            }

            public void rollbackSnapshot()
            {
                index = snapshots.Pop();
            }

            public void commitSnapshot()
            {
                snapshots.Pop();
            }
        }

        private abstract class Matcher
        {
            public String word;
            public Token isMatch(Tokenizer t)
            {
                if (t.End()) { return new Token(TokenType.EOF,t.index); }

                t.takeSnapshot();

                Token match = isMatchDriver(t);
                if (match == null)
                {
                    t.rollbackSnapshot();
                }
                else
                {
                    t.commitSnapshot();
                }

                return match;
            }
            protected abstract Token isMatchDriver(Tokenizer t);
        }

        private class BoolValue : Matcher
        {

            private TokenType type;
            public List<Matcher> SpecialCharacters;

            public BoolValue(TokenType type, String word)
            {
                this.type = type;
                this.word = word;
            }

            protected override Token isMatchDriver(Tokenizer t)
            {
                var current = t.Current;

                foreach (var character in word)
                {
                    if (character.ToString() == current)
                    {
                        t.Eat();
                        current = t.Current;
                    }
                    else
                    {
                        return null;
                    }
                }

                var next = t.Current;
                if (String.IsNullOrWhiteSpace(next) || SpecialCharacters.Any(c => c.word.First().ToString() == next))
                {
                    return new Token(type, word,t.index);
                }

                return null;
            }
        }

        private class Operand : Matcher
        {

            private TokenType type;

            public Operand(TokenType type, String word)
            {
                this.type = type;
                this.word = word;
            }

            protected override Token isMatchDriver(Tokenizer t)
            {
                var current = t.Current;

                foreach (var character in word)
                {
                    if (character.ToString() == current)
                    {
                        t.Eat();
                        current = t.Current;
                    }
                    else
                    {
                        return null;
                    }
                }

                //var next = t.Current;
                //if (String.IsNullOrWhiteSpace(next))
                //{
                return new Token(type, word,t.index);
                //}

            }
        }


        private class Bracket : Matcher
        {

            private TokenType type;

            public Bracket(TokenType type, String word)
            {
                this.type = type;
                this.word = word;
            }

            protected override Token isMatchDriver(Tokenizer t)
            {
                var current = t.Current;

                foreach (var character in word)
                {
                    if (character.ToString() == current)
                    {
                        t.Eat();
                        current = t.Current;
                    }
                    else
                    {
                        return null;
                    }
                }

                //var next = t.Current;
                //if (String.IsNullOrWhiteSpace(next))
                //{
                return new Token(type, word,t.index);
                //}

                return null;
            }
        }

        private class WhiteSpace : Matcher
        {
            protected override Token isMatchDriver(Tokenizer t)
            {
                Boolean match = false;
                while (!t.End() && String.IsNullOrWhiteSpace(t.Current))
                {
                    t.Eat();
                    match = true;
                }

                if (match) { return new Token(TokenType.WHITESPACE,t.index); }
                return null;
            }
        }
    }
}
