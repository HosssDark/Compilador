using System.Collections.Generic;

namespace Compilador
{
    public class NoPrograma : NoAST
    {
        public ClassePrincipal ClassePrincipal { get; }
        public List<DeclaracaoClasse> DeclaracaoClasses { get; }

        public NoPrograma(ClassePrincipal classePrincipal, List<DeclaracaoClasse> declaracaoClasses, int linha, int coluna) : base(linha, coluna)
        {
            ClassePrincipal = classePrincipal;
            DeclaracaoClasses = declaracaoClasses ?? new List<DeclaracaoClasse>();
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }
}