namespace Compilador
{
    public static class EnumToken
    {
        public enum TipoToken
        {
            //Palavras reservadas
            CLASS, PUBLIC, STATIC, VOID, MAIN, STRING,
            EXTENDS, RETURN, INT, BOOLEAN, IF, ELSE,
            WHILE, LENGTH, TRUE, FALSE, THIS, NEW,
            PRINTLN, SYSTEM, OUT, 

            //Identificadores e literais
            IDENTIFIER, INTEGER_LITERAL,

            //Operadores
            AND, LESS_THAN, PLUS, MINUS, TIMES,
            NOT, EQUALS, DOT,
            //Delimitadores
            LBRACKET, RBRACKET, SEMICOLON, COMMA,
            RKEY, LKEY, RPARENTHESE, LPARENTHESE,

            //Especiais
            EOF, ERROR, 
        }
    }
}