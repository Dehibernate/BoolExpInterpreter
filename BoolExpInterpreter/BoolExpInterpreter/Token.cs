using System;

namespace BoolExpInterpreter
{
    public class Token
    {
        public TokenType type;
        public String image;
        public int col,endcol=1;

        public Token(TokenType type, String image,int col)
        {
            this.type = type;
            this.image = image;
            this.col = col+1;
            this.endcol = this.col + image.Length;
        }

        public Token(TokenType type,int col)
        {
            this.type = type;
            this.col = col+1;
            endcol = this.col;
        }
    }
}
