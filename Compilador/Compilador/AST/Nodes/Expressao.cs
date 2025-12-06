using System.Collections.Generic;

namespace Compilador
{
    public abstract class Expressao : NoAST
    {
        protected Expressao(int linha, int coluna) : base(linha, coluna)
        {
        }
    }

    public class And : Expressao
    {
        public Expressao Esquerda { get; set; }
        public Expressao Direita { get; set; }

        public And(Expressao esquerda, Expressao direita, int linha, int coluna) : base(linha, coluna)
        {
            Esquerda = esquerda;
            Direita = direita;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class LessThan : Expressao
    {
        public Expressao Esquerda { get; set; }
        public Expressao Direita { get; set; }

        public LessThan(Expressao esquerda, Expressao direita, int linha, int coluna) : base(linha, coluna)
        {
            Esquerda = esquerda;
            Direita = direita;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class Plus : Expressao
    {
        public Expressao Esquerda { get; set; }
        public Expressao Direita { get; set; }

        public Plus(Expressao esquerda, Expressao direita, int linha, int coluna) : base(linha, coluna)
        {
            Esquerda = esquerda;
            Direita = direita;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class Minus : Expressao
    {
        public Expressao Esquerda { get; set; }
        public Expressao Direita { get; set; }

        public Minus(Expressao esquerda, Expressao direita, int linha, int coluna) : base(linha, coluna)
        {
            Esquerda = esquerda;
            Direita = direita;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class Times : Expressao
    {
        public Expressao Esquerda { get; set; }
        public Expressao Direita { get; set; }

        public Times(Expressao esquerda, Expressao direita, int linha, int coluna) : base(linha, coluna)
        {
            Esquerda = esquerda;
            Direita = direita;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class ArrayLookup : Expressao
    {
        public Expressao Array { get; set; }
        public Expressao Index { get; set; }

        public ArrayLookup(Expressao array, Expressao index, int linha, int coluna) : base(linha, coluna)
        {
            Array = array;
            Index = index;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class ArrayLength : Expressao
    {
        public Expressao Array { get; set; }

        public ArrayLength(Expressao array, int linha, int coluna) : base(linha, coluna)
        {
            Array = array;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class Call : Expressao
    {
        public Expressao Objeto { get; set; }
        public Identificador NomeMetodo { get; set; }
        public List<Expressao> Argumentos { get; set; }

        public Call(Expressao objeto, Identificador nomeMetodo, List<Expressao> argumentos, int linha, int coluna) : base(linha, coluna)
        {
            Objeto = objeto;
            NomeMetodo = nomeMetodo;
            Argumentos = argumentos ?? new List<Expressao>();
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class IntegerLiteral : Expressao
    {
        public int Valor { get; set; }

        public IntegerLiteral(int valor, int linha, int coluna) : base(linha, coluna)
        {
            Valor = valor;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class True : Expressao
    {
        public True(int linha, int coluna) : base(linha, coluna)
        {
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class False : Expressao
    {
        public False(int linha, int coluna) : base(linha, coluna)
        {
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class IdentifierExp : Expressao
    {
        public string Name { get; set; }

        public IdentifierExp(string name, int linha, int coluna) : base(linha, coluna)
        {
            Name = name;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class This : Expressao
    {
        public This(int linha, int coluna) : base(linha, coluna)
        {
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class NewArray : Expressao
    {
        public Expressao Tamanho { get; set; }

        public NewArray(Expressao tamanho, int linha, int coluna) : base(linha, coluna)
        {
            Tamanho = tamanho;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class NewObject : Expressao
    {
        public Identificador NomeClasse { get; set; }

        public NewObject(Identificador nomeClasse, int linha, int coluna) : base(linha, coluna)
        {
            NomeClasse = nomeClasse;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class Not : Expressao
    {
        public Expressao Expressao { get; set; }

        public Not(Expressao expressao, int linha, int coluna) : base(linha, coluna)
        {
            Expressao = expressao;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }
}