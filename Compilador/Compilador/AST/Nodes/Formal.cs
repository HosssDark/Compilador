namespace Compilador
{
    public class Formal : NoAST
    {
        public NoTipo Tipo { get; set; }
        public Identificador Nome { get; set; }

        public Formal(NoTipo tipo, Identificador nome, int linha, int coluna) : base(linha, coluna)
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