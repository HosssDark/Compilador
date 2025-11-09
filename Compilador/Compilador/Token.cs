namespace Compilador
{
    public class Token
    {
        public EnumToken.TipoToken Tipo { get; set; }
        public string Lexema { get; set; }
        public int Linha { get; set; }
        public int Coluna { get; set; }

        public Token(EnumToken.TipoToken tipo, string lexema, int linha, int coluna)
        {
            Tipo = tipo;
            Lexema = lexema;
            Linha = linha;
            Coluna = coluna;
        }

        //formatada com as informações do token
        public override string ToString()
        {
            return $"Token({Tipo}, '{Lexema}', Linha:{Linha}, Col:{Coluna})";
        }
    }
}