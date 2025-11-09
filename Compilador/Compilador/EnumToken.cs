namespace Compilador
{
    public static class EnumToken
    {
        public enum TipoToken
        {
            //Palavras chave
            CLASS, PUBLIC, STATIC, VOID, MAIN, STRING,
            EXTENDS, RETURN, INT, BOOLEAN, IF, ELSE,
            WHILE, PRINT, LENGTH, TRUE, FALSE, THIS, NEW,

            //Identificadores e literais
            ID, INTEGER_LITERAL,

            //Operadores
            AND, LESS_THAN, PLUS, MINUS, TIMES,
            NOT, EQUALS, DOT,

            //Delimitadores
            LPAREN, RPAREN, LBRACE, RBRACE,
            LBRACKET, RBRACKET, SEMICOLON, COMMA,

            //Especiais
            EOF, ERROR
        }
    }
}