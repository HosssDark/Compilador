namespace Compilador
{
    public interface IConcatenar
    {
        void Concatenar(NoPrograma node);

        void Concatenar(ClassePrincipal node);
        void Concatenar(DeclaracaoClasseSimples node);
        void Concatenar(DeclaracaoClasseEstende node);

        void Concatenar(DeclaracaoVariavel node);
        void Concatenar(DeclaracaoMetodo node);
        void Concatenar(Formal node);

        void Concatenar(IntArrayType node);
        void Concatenar(BooleanType node);
        void Concatenar(IntegerType node);
        void Concatenar(IdentifierType node);

        void Concatenar(Block node);
        void Concatenar(If node);
        void Concatenar(While node);
        void Concatenar(Print node);
        void Concatenar(Assign node);
        void Concatenar(ArrayAssign node);

        void Concatenar(And node);
        void Concatenar(LessThan node);
        void Concatenar(Plus node);
        void Concatenar(Minus node);
        void Concatenar(Times node);
        void Concatenar(ArrayLookup node);
        void Concatenar(ArrayLength node);
        void Concatenar(Call node);
        void Concatenar(IntegerLiteral node);
        void Concatenar(True node);
        void Concatenar(False node);
        void Concatenar(IdentifierExp node);
        void Concatenar(This node);
        void Concatenar(NewArray node);
        void Concatenar(NewObject node);
        void Concatenar(Not node);

        void Concatenar(Identificador node);
    }
}