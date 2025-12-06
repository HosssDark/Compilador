using System.Collections.Generic;

namespace Compilador
{
    public class DeclaracaoMetodo : NoAST
    {
        public NoTipo RetornoTipo { get; set; }
        public Identificador NomeMetodo { get; set; }
        public List<Formal> Parametros { get; set; }
        public List<DeclaracaoVariavel> VariavelLocais { get; set; }
        public List<Declaracao> Declaracao { get; set; }
        public Expressao RetornoExpressao { get; set; }

        public DeclaracaoMetodo(NoTipo retornoTipo, Identificador nomeMetodo, List<Formal> parametros, List<DeclaracaoVariavel> variavelLocais, List<Declaracao> declaracao, Expressao retornoExpressao, int line, int column) : base(line, column)
        {
            RetornoTipo = retornoTipo;
            NomeMetodo = nomeMetodo;
            Parametros = parametros ?? new List<Formal>();
            VariavelLocais = variavelLocais ?? new List<DeclaracaoVariavel>();
            Declaracao = declaracao ?? new List<Declaracao>();
            RetornoExpressao = retornoExpressao;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }
}