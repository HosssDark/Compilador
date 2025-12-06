namespace Compilador
{
    public abstract class NoTipo : NoAST
    {
        protected NoTipo(int linha, int coluna) : base(linha, coluna)
        {
        }
    }

    public class IntArrayType : NoTipo
    {
        public IntArrayType(int linha, int coluna) : base(linha, coluna)
        {
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class BooleanType : NoTipo
    {
        public BooleanType(int linha, int coluna) : base(linha, coluna)
        {
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class IntegerType : NoTipo
    {
        public IntegerType(int linha, int coluna) : base(linha, coluna)
        {
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }

    public class IdentifierType : NoTipo
    {
        public string NomeTipo { get; set; }

        public IdentifierType(string momeTipo, int linha, int coluna) : base(linha, coluna)
        {
            NomeTipo = momeTipo;
        }

        public override void Aceitar(IConcatenar concatenar)
        {
            concatenar.Concatenar(this);
        }
    }
}