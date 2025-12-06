using System.Collections.Generic;

namespace Compilador
{
    public abstract class Declaracao : NoAST
    {
        protected Declaracao(int linha, int coluna) : base(linha, coluna)
        {
        }
    }

    public class Block : Declaracao
    {
        public List<Declaracao> Statements { get; set; }

        public Block(List<Declaracao> statements, int linha, int coluna) : base(linha, coluna)
        {
            Statements = statements ?? new List<Declaracao>();
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class If : Declaracao
    {
        public Expressao Condition { get; set; }
        public Declaracao ThenStatement { get; set; }
        public Declaracao ElseStatement { get; set; }

        public If(Expressao condition, Declaracao thenStatement, Declaracao elseStatement, int linha, int coluna) : base(linha, coluna)
        {
            Condition = condition;
            ThenStatement = thenStatement;
            ElseStatement = elseStatement;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class While : Declaracao
    {
        public Expressao Condition { get; set; }
        public Declaracao Body { get; set; }

        public While(Expressao condition, Declaracao body, int linha, int coluna) : base(linha, coluna)
        {
            Condition = condition;
            Body = body;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class Print : Declaracao
    {
        public Expressao Expression { get; set; }

        public Print(Expressao expression, int linha, int coluna) : base(linha, coluna)
        {
            Expression = expression;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class Assign : Declaracao
    {
        public Identificador Identificador { get; set; }
        public Expressao Expression { get; set; }

        public Assign(Identificador identificador, Expressao expression, int linha, int coluna) : base(linha, coluna)
        {
            Identificador = identificador;
            Expression = expression;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class ArrayAssign : Declaracao
    {
        public Identificador Identificador { get; set; }
        public Expressao Indice { get; set; }
        public Expressao Valor { get; set; }

        public ArrayAssign(Identificador identificador, Expressao indice, Expressao valor, int linha, int coluna) : base(linha, coluna)
        {
            Identificador = identificador;
            Indice = indice;
            Valor = valor;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }
}