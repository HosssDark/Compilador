using System.Text;

namespace Compilador
{
    public class ConcatenarImpressaoSaida : IConcatenar
    {
        private StringBuilder saida;
        private int identificadorNivel;
        private const string identificadorString = "  ";

        public ConcatenarImpressaoSaida()
        {
            saida = new StringBuilder();
            identificadorNivel = 0;
        }

        public string ObterSaida()
        {
            return saida.ToString();
        }

        private void Identificador()
        {
            saida.Append(new string(' ', identificadorNivel * identificadorString.Length));
        }

        private void EscreverLinha(string text)
        {
            Identificador();
            saida.AppendLine(text);
        }

        public void Concatenar(NoPrograma node)
        {
            EscreverLinha("Program:");
            identificadorNivel++;
            node.ClassePrincipal.Aceitar(this);
            
            foreach (var classDecl in node.DeclaracaoClasses)
                classDecl.Aceitar(this);

            identificadorNivel--;
        }

        public void Concatenar(ClassePrincipal node)
        {
            EscreverLinha($"MainClass: {node.ClasseNome.Nome}");
            identificadorNivel++;
            EscreverLinha($"Args: {node.ClasseNome.Nome}");
            node.Declaracao.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(DeclaracaoClasseSimples node)
        {
            EscreverLinha($"Class: {node.NomeClasse.Nome}");
            identificadorNivel++;

            if (node.Variaveis.Count > 0)
            {
                EscreverLinha("Variables:");
                identificadorNivel++;

                foreach (var varDecl in node.Variaveis)
                    varDecl.Aceitar(this);

                identificadorNivel--;
            }

            if (node.Metodos.Count > 0)
            {
                EscreverLinha("Methods:");
                identificadorNivel++;
                
                foreach (var methodDecl in node.Metodos)
                    methodDecl.Aceitar(this);

                identificadorNivel--;
            }

            identificadorNivel--;
        }

        public void Concatenar(DeclaracaoClasseEstende node)
        {
            EscreverLinha($"Class: {node.NomeClasse.Nome} extends {node.NomeClasseSuper.Nome}");
            identificadorNivel++;

            if (node.Variaveis.Count > 0)
            {
                EscreverLinha("Variables:");
                identificadorNivel++;
                
                foreach (var varDecl in node.Variaveis)
                    varDecl.Aceitar(this);

                identificadorNivel--;
            }

            if (node.Metodos.Count > 0)
            {
                EscreverLinha("Methods:");
                identificadorNivel++;
                
                foreach (var methodDecl in node.Metodos)
                    methodDecl.Aceitar(this);

                identificadorNivel--;
            }

            identificadorNivel--;
        }

        public void Concatenar(DeclaracaoVariavel node)
        {
            Identificador();
            saida.Append("Var: ");
            node.Tipo.Aceitar(this);
            saida.AppendLine($" {node.Nome.Nome}");
        }

        public void Concatenar(DeclaracaoMetodo node)
        {
            Identificador();
            saida.Append("Method: ");
            node.RetornoTipo.Aceitar(this);
            saida.AppendLine($" {node.NomeMetodo.Nome}");

            identificadorNivel++;

            if (node.Parametros.Count > 0)
            {
                EscreverLinha("Parameters:");
                identificadorNivel++;

                foreach (var formal in node.Parametros)
                    formal.Aceitar(this);

                identificadorNivel--;
            }

            if (node.VariavelLocais.Count > 0)
            {
                EscreverLinha("Local Variables:");
                identificadorNivel++;
                foreach (var varDecl in node.VariavelLocais)
                    varDecl.Aceitar(this);

                identificadorNivel--;
            }

            if (node.Declaracao.Count > 0)
            {
                EscreverLinha("Statements:");
                identificadorNivel++;

                foreach (var stmt in node.Declaracao)
                    stmt.Aceitar(this);

                identificadorNivel--;
            }

            EscreverLinha("Return:");
            identificadorNivel++;
            node.RetornoExpressao.Aceitar(this);
            identificadorNivel--;

            identificadorNivel--;
        }

        public void Concatenar(Formal node)
        {
            Identificador();
            saida.Append("Param: ");
            node.Tipo.Aceitar(this);
            saida.AppendLine($" {node.Nome.Nome}");
        }

        public void Concatenar(IntArrayType node)
        {
            saida.Append("int[]");
        }

        public void Concatenar(BooleanType node)
        {
            saida.Append("boolean");
        }

        public void Concatenar(IntegerType node)
        {
            saida.Append("int");
        }

        public void Concatenar(IdentifierType node)
        {
            saida.Append(node.NomeTipo);
        }

        public void Concatenar(Block node)
        {
            EscreverLinha("Block:");
            identificadorNivel++;
            
            foreach (var stmt in node.Statements)
                stmt.Aceitar(this);

            identificadorNivel--;
        }

        public void Concatenar(If node)
        {
            EscreverLinha("If:");
            identificadorNivel++;
            EscreverLinha("Condition:");
            identificadorNivel++;
            node.Condition.Aceitar(this);
            identificadorNivel--;
            EscreverLinha("Then:");
            identificadorNivel++;
            node.ThenStatement.Aceitar(this);
            identificadorNivel--;
            EscreverLinha("Else:");
            identificadorNivel++;
            node.ElseStatement.Aceitar(this);
            identificadorNivel--;
            identificadorNivel--;
        }

        public void Concatenar(While node)
        {
            EscreverLinha("While:");
            identificadorNivel++;
            EscreverLinha("Condition:");
            identificadorNivel++;
            node.Condition.Aceitar(this);
            identificadorNivel--;
            EscreverLinha("Body:");
            identificadorNivel++;
            node.Body.Aceitar(this);
            identificadorNivel--;
            identificadorNivel--;
        }

        public void Concatenar(Print node)
        {
            EscreverLinha("Print:");
            identificadorNivel++;
            node.Expression.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(Assign node)
        {
            EscreverLinha($"Assign to {node.Identificador.Nome}:");
            identificadorNivel++;
            node.Expression.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(ArrayAssign node)
        {
            EscreverLinha($"ArrayAssign to {node.Identificador.Nome}:");
            identificadorNivel++;
            EscreverLinha("Index:");
            identificadorNivel++;
            node.Indice.Aceitar(this);
            identificadorNivel--;
            EscreverLinha("Value:");
            identificadorNivel++;
            node.Valor.Aceitar(this);
            identificadorNivel--;
            identificadorNivel--;
        }

        public void Concatenar(And node)
        {
            EscreverLinha("And:");
            identificadorNivel++;
            node.Esquerda.Aceitar(this);
            node.Direita.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(LessThan node)
        {
            EscreverLinha("LessThan:");
            identificadorNivel++;
            node.Esquerda.Aceitar(this);
            node.Direita.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(Plus node)
        {
            EscreverLinha("Plus:");
            identificadorNivel++;
            node.Esquerda.Aceitar(this);
            node.Direita.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(Minus node)
        {
            EscreverLinha("Minus:");
            identificadorNivel++;
            node.Esquerda.Aceitar(this);
            node.Direita.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(Times node)
        {
            EscreverLinha("Times:");
            identificadorNivel++;
            node.Esquerda.Aceitar(this);
            node.Direita.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(ArrayLookup node)
        {
            EscreverLinha("ArrayLookup:");
            identificadorNivel++;
            EscreverLinha("Array:");
            identificadorNivel++;
            node.Array.Aceitar(this);
            identificadorNivel--;
            EscreverLinha("Index:");
            identificadorNivel++;
            node.Index.Aceitar(this);
            identificadorNivel--;
            identificadorNivel--;
        }

        public void Concatenar(ArrayLength node)
        {
            EscreverLinha("ArrayLength:");
            identificadorNivel++;
            node.Array.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(Call node)
        {
            EscreverLinha($"Call method {node.NomeMetodo.Nome}:");
            identificadorNivel++;
            EscreverLinha("Object:");
            identificadorNivel++;
            node.Objeto.Aceitar(this);
            identificadorNivel--;

            if (node.Argumentos.Count > 0)
            {
                EscreverLinha("Arguments:");
                identificadorNivel++;
                
                foreach (var arg in node.Argumentos)
                    arg.Aceitar(this);

                identificadorNivel--;
            }

            identificadorNivel--;
        }

        public void Concatenar(IntegerLiteral node)
        {
            EscreverLinha($"IntegerLiteral: {node.Valor}");
        }

        public void Concatenar(True node)
        {
            EscreverLinha("True");
        }

        public void Concatenar(False node)
        {
            EscreverLinha("False");
        }

        public void Concatenar(IdentifierExp node)
        {
            EscreverLinha($"Identifier: {node.Name}");
        }

        public void Concatenar(This node)
        {
            EscreverLinha("This");
        }

        public void Concatenar(NewArray node)
        {
            EscreverLinha("NewArray:");
            identificadorNivel++;
            EscreverLinha("Size:");
            identificadorNivel++;
            node.Tamanho.Aceitar(this);
            identificadorNivel--;
            identificadorNivel--;
        }

        public void Concatenar(NewObject node)
        {
            EscreverLinha($"NewObject: {node.NomeClasse.Nome}");
        }

        public void Concatenar(Not node)
        {
            EscreverLinha("Not:");
            identificadorNivel++;
            node.Expressao.Aceitar(this);
            identificadorNivel--;
        }

        public void Concatenar(Identificador node)
        {
            EscreverLinha($"Identifier: {node.Nome}");
        }
    }
}