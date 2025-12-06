namespace Compilador
{
    public abstract class NoAST
    {
        public int Linha { get; set; }
        public int Coluna { get; set; }

        protected NoAST(int linha, int coluna)
        {
            Linha = linha;
            Coluna = coluna;
        }

        public abstract void Aceitar(IConcatenar concatenar);
    }
}