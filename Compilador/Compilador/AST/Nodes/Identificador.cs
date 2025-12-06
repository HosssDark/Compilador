namespace Compilador
{
    public class Identificador : NoAST
    {
        public string Nome { get; set; }

        public Identificador(string nome, int linha, int coluna) : base(linha, coluna)
        {
            Nome = nome;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }
}