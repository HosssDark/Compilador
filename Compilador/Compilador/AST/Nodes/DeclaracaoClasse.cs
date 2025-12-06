using System.Collections.Generic;

namespace Compilador
{
    public abstract class DeclaracaoClasse : NoAST
    {
        public Identificador NomeClasse { get; }
        public List<DeclaracaoVariavel> Variaveis { get; }
        public List<DeclaracaoMetodo> Metodos { get; }

        protected DeclaracaoClasse(Identificador nomeClasse, List<DeclaracaoVariavel> variaveis, List<DeclaracaoMetodo> metodos, int linha, int coluna) : base(linha, coluna)
        {
            NomeClasse = nomeClasse;
            Variaveis = variaveis ?? new List<DeclaracaoVariavel>();
            Metodos = metodos ?? new List<DeclaracaoMetodo>();
        }
    }

    public class DeclaracaoClasseSimples : DeclaracaoClasse
    {
        public DeclaracaoClasseSimples(Identificador nomeClasse, List<DeclaracaoVariavel> variaveis, List<DeclaracaoMetodo> metodos, int linha, int coluna) : base(nomeClasse, variaveis, metodos, linha, coluna)
        {
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class DeclaracaoClasseEstende : DeclaracaoClasse
    {
        public Identificador NomeClasseSuper { get; }

        public DeclaracaoClasseEstende(Identificador className, Identificador nomeClasseSuper, List<DeclaracaoVariavel> variaveis, List<DeclaracaoMetodo> metodos, int linha, int coluna)
            : base(className, variaveis, metodos, linha, coluna)
        {
            NomeClasseSuper = nomeClasseSuper;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }
}