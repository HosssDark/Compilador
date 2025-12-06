namespace Compilador
{
    public class DeclaracaoVariavel : NoAST
    {
        public NoTipo Tipo { get; set; }
        public Identificador Nome { get; set; }

        public DeclaracaoVariavel(NoTipo tipo, Identificador nome, int linha, int coluna) : base(linha, coluna)
        {
            Tipo = tipo;
            Nome = nome;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }
}