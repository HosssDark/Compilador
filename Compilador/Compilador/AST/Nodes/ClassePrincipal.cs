namespace Compilador
{
    public class ClassePrincipal : NoAST
    {
        public Identificador ClasseNome { get; set; }
        public Identificador ArgumentoNome { get; set; }
        public Declaracao Declaracao { get; set; }

        public ClassePrincipal(Identificador classeNome, Identificador argumentoNome, Declaracao declaracao, int linha, int coluna) : base(linha, coluna)
        {
            ClasseNome = classeNome;
            ArgumentoNome = argumentoNome;
            Declaracao = declaracao;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }
}