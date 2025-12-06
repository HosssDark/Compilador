using static Compilador.EnumToken;
using System.Collections.Generic;
using System;

namespace Compilador
{
    public class Parser
    {
        private List<Token> tokens;
        private int posicaoAtual;
        private Token tokenAtual;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.posicaoAtual = 0;
            this.tokenAtual = tokens.Count > 0 ? tokens[0] : null;
        }

        private void Avancar()
        {
            if (posicaoAtual < tokens.Count - 1)
            {
                posicaoAtual++;
                tokenAtual = tokens[posicaoAtual];
            }
        }

        private void VerificarTipoToken(TipoToken expectedType)
        {
            if (tokenAtual == null)
            {
                throw new ParserException($"Fim inesperado do arquivo. Esperado: {expectedType}");
            }

            if (tokenAtual.Tipo != expectedType)
            {
                throw new ParserException(
                    $"Token inesperado na linha {tokenAtual.Linha}, coluna {tokenAtual.Coluna}. " +
                    $"Esperado: {expectedType}, Encontrado: {tokenAtual.Tipo} ('{tokenAtual.Lexema}')");
            }

            Avancar();
        }

        public NoPrograma Analisar()
        {
            int linha = tokenAtual?.Linha ?? 1;
            int coluna = tokenAtual?.Coluna ?? 1;

            ClassePrincipal classePrincipal = AnalisarClassePrincipal();
            List<DeclaracaoClasse> lstDeclaracaoClasse = new List<DeclaracaoClasse>();

            while (tokenAtual != null && tokenAtual.Tipo == TipoToken.CLASS)
                lstDeclaracaoClasse.Add(AnalisarDeclaracaoClasses());

            return new NoPrograma(classePrincipal, lstDeclaracaoClasse, linha, coluna);
        }

        private ClassePrincipal AnalisarClassePrincipal()
        {
            int linha = tokenAtual.Linha;
            int coluna = tokenAtual.Coluna;

            VerificarTipoToken(TipoToken.CLASS);
            Identificador NomeClasse = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);
            VerificarTipoToken(TipoToken.IDENTIFIER);
            VerificarTipoToken(TipoToken.LKEY);
            VerificarTipoToken(TipoToken.PUBLIC);
            VerificarTipoToken(TipoToken.STATIC);
            VerificarTipoToken(TipoToken.VOID);
            VerificarTipoToken(TipoToken.MAIN);
            VerificarTipoToken(TipoToken.LPARENTHESE);
            VerificarTipoToken(TipoToken.STRING);
            VerificarTipoToken(TipoToken.LBRACKET);
            VerificarTipoToken(TipoToken.RBRACKET);
            Identificador NomeArgumento = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);
            VerificarTipoToken(TipoToken.IDENTIFIER);
            VerificarTipoToken(TipoToken.RPARENTHESE);
            VerificarTipoToken(TipoToken.LKEY);
            Declaracao Declaracao = AnalisarDeclaracao();
            VerificarTipoToken(TipoToken.RKEY);
            VerificarTipoToken(TipoToken.RKEY);

            return new ClassePrincipal(NomeClasse, NomeArgumento, Declaracao, linha, coluna);
        }

        private DeclaracaoClasse AnalisarDeclaracaoClasses()
        {
            int linha = tokenAtual.Linha;
            int coluna = tokenAtual.Coluna;

            VerificarTipoToken(TipoToken.CLASS);
            Identificador nomeClasse = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);
            VerificarTipoToken(TipoToken.IDENTIFIER);

            Identificador nomeClasseSuper = null;

            if (tokenAtual.Tipo == TipoToken.EXTENDS)
            {
                VerificarTipoToken(TipoToken.EXTENDS);
                nomeClasseSuper = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);
                VerificarTipoToken(TipoToken.IDENTIFIER);
            }

            VerificarTipoToken(TipoToken.LKEY);

            List<DeclaracaoVariavel> lstDeclaracaoVariavel = new List<DeclaracaoVariavel>();
            List<DeclaracaoMetodo> lstDeclaracaoMetodo = new List<DeclaracaoMetodo>();

            while (EhDeclaracaoVariavel())
                lstDeclaracaoVariavel.Add(AnalisarDeclaracaoVariavel());

            while (tokenAtual.Tipo == TipoToken.PUBLIC)
                lstDeclaracaoMetodo.Add(AnalisarMetodoDeclaracao());

            VerificarTipoToken(TipoToken.RKEY);

            if (nomeClasseSuper != null)
                return new DeclaracaoClasseEstende(nomeClasse, nomeClasseSuper, lstDeclaracaoVariavel, lstDeclaracaoMetodo, linha, coluna);
            else
                return new DeclaracaoClasseSimples(nomeClasse, lstDeclaracaoVariavel, lstDeclaracaoMetodo, linha, coluna);
        }

        private bool EhDeclaracaoVariavel()
        {
            if (tokenAtual == null || tokenAtual.Tipo == TipoToken.RKEY)
                return false;

            if (tokenAtual.Tipo == TipoToken.PUBLIC)
                return false;

            return tokenAtual.Tipo == TipoToken.INT || tokenAtual.Tipo == TipoToken.BOOLEAN || tokenAtual.Tipo == TipoToken.IDENTIFIER;
        }

        private DeclaracaoVariavel AnalisarDeclaracaoVariavel()
        {
            int linha = tokenAtual.Linha;
            int coluna = tokenAtual.Coluna;

            NoTipo tipo = AnalisarTipo();
            Identificador nome = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);
            VerificarTipoToken(TipoToken.IDENTIFIER);
            VerificarTipoToken(TipoToken.SEMICOLON);

            return new DeclaracaoVariavel(tipo, nome, linha, coluna);
        }

        private DeclaracaoMetodo AnalisarMetodoDeclaracao()
        {
            int linha = tokenAtual.Linha;
            int coluna = tokenAtual.Coluna;

            VerificarTipoToken(TipoToken.PUBLIC);
            NoTipo retornoTipo = AnalisarTipo();
            Identificador nomeMetodo = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);
            VerificarTipoToken(TipoToken.IDENTIFIER);
            VerificarTipoToken(TipoToken.LPARENTHESE);

            List<Formal> lstFormal = new List<Formal>();

            if (tokenAtual.Tipo != TipoToken.RPARENTHESE)
            {
                lstFormal.Add(AnaliseFormal());
                
                while (tokenAtual.Tipo == TipoToken.COMMA)
                {
                    VerificarTipoToken(TipoToken.COMMA);
                    lstFormal.Add(AnaliseFormal());
                }
            }

            VerificarTipoToken(TipoToken.RPARENTHESE);
            VerificarTipoToken(TipoToken.LKEY);

            List<DeclaracaoVariavel> lstDeclaracaoVariavel = new List<DeclaracaoVariavel>();
            
            while (EhDeclaracaoVariavel())
                lstDeclaracaoVariavel.Add(AnalisarDeclaracaoVariavel());

            List<Declaracao> lstDeclaracao = new List<Declaracao>();
            
            while (tokenAtual.Tipo != TipoToken.RETURN)
                lstDeclaracao.Add(AnalisarDeclaracao());

            VerificarTipoToken(TipoToken.RETURN);
            Expressao returnExpression = AnalisarExpressao();
            VerificarTipoToken(TipoToken.SEMICOLON);
            VerificarTipoToken(TipoToken.RKEY);

            return new DeclaracaoMetodo(retornoTipo, nomeMetodo, lstFormal, lstDeclaracaoVariavel, lstDeclaracao, returnExpression, linha, coluna);
        }

        private Formal AnaliseFormal()
        {
            int linha = tokenAtual.Linha;
            int coluna = tokenAtual.Coluna;

            NoTipo tipo = AnalisarTipo();
            Identificador nome = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);

            VerificarTipoToken(TipoToken.IDENTIFIER);

            return new Formal(tipo, nome, linha, coluna);
        }

        private NoTipo AnalisarTipo()
        {
            int linha = tokenAtual.Linha;
            int coluna = tokenAtual.Coluna;

            if (tokenAtual.Tipo == TipoToken.INT)
            {
                VerificarTipoToken(TipoToken.INT);

                if (tokenAtual.Tipo == TipoToken.LBRACKET)
                {
                    VerificarTipoToken(TipoToken.LBRACKET);
                    VerificarTipoToken(TipoToken.RBRACKET);

                    return new IntArrayType(linha, coluna);
                }
                
                return new IntegerType(linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.BOOLEAN)
            {
                VerificarTipoToken(TipoToken.BOOLEAN);

                return new BooleanType(linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.IDENTIFIER)
            {
                string nome = tokenAtual.Lexema;

                VerificarTipoToken(TipoToken.IDENTIFIER);

                return new IdentifierType(nome, linha, coluna);
            }

            throw new ParserException($"Tipo esperado na linha {tokenAtual.Linha}, coluna {tokenAtual.Coluna}");
        }

        private Declaracao AnalisarDeclaracao()
        {
            int linha = tokenAtual.Linha;
            int coluna = tokenAtual.Coluna;

            if (tokenAtual.Tipo == TipoToken.LKEY)
            {
                VerificarTipoToken(TipoToken.LKEY);
                List<Declaracao> lstDeclaracao = new List<Declaracao>();

                while (tokenAtual.Tipo != TipoToken.RKEY)
                    lstDeclaracao.Add(AnalisarDeclaracao());

                VerificarTipoToken(TipoToken.RKEY);

                return new Block(lstDeclaracao, linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.IF)
            {
                VerificarTipoToken(TipoToken.IF);
                VerificarTipoToken(TipoToken.LPARENTHESE);
                
                Expressao condition = AnalisarExpressao();
                
                VerificarTipoToken(TipoToken.RPARENTHESE);
                
                Declaracao thenStatement = AnalisarDeclaracao();
                
                VerificarTipoToken(TipoToken.ELSE);
                
                Declaracao elseStatement = AnalisarDeclaracao();

                return new If(condition, thenStatement, elseStatement, linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.WHILE)
            {
                VerificarTipoToken(TipoToken.WHILE);
                VerificarTipoToken(TipoToken.LPARENTHESE);
                
                Expressao condicao = AnalisarExpressao();
                
                VerificarTipoToken(TipoToken.RPARENTHESE);
                
                Declaracao corpo = AnalisarDeclaracao();

                return new While(condicao, corpo, linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.SYSTEM)
            {
                VerificarTipoToken(TipoToken.SYSTEM);
                VerificarTipoToken(TipoToken.DOT);
                VerificarTipoToken(TipoToken.OUT);
                VerificarTipoToken(TipoToken.DOT);
                VerificarTipoToken(TipoToken.PRINTLN);
                VerificarTipoToken(TipoToken.LPARENTHESE);
                
                Expressao expression = AnalisarExpressao();

                VerificarTipoToken(TipoToken.RPARENTHESE);
                VerificarTipoToken(TipoToken.SEMICOLON);
                
                return new Print(expression, linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.IDENTIFIER)
            {
                Identificador identificador = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);
                VerificarTipoToken(TipoToken.IDENTIFIER);

                if (tokenAtual.Tipo == TipoToken.EQUALS)
                {
                    VerificarTipoToken(TipoToken.EQUALS);

                    Expressao expressao = AnalisarExpressao();
                    VerificarTipoToken(TipoToken.SEMICOLON);

                    return new Assign(identificador, expressao, linha, coluna);
                }
                else if (tokenAtual.Tipo == TipoToken.LBRACKET)
                {
                    VerificarTipoToken(TipoToken.LBRACKET);
                    
                    Expressao indice = AnalisarExpressao();
                    VerificarTipoToken(TipoToken.RBRACKET);
                    VerificarTipoToken(TipoToken.EQUALS);
                    
                    Expressao valor = AnalisarExpressao();
                    VerificarTipoToken(TipoToken.SEMICOLON);

                    return new ArrayAssign(identificador, indice, valor, linha, coluna);
                }
            }

            throw new ParserException($"Statement esperado na linha {tokenAtual.Linha}, coluna {tokenAtual.Coluna}");
        }

        private Expressao AnalisarExpressao()
        {
            return AnalisarExpressaoAnd();
        }

        private Expressao AnalisarExpressaoAnd()
        {
            Expressao esquerda = AnalisarExpressaoComparacao();

            while (tokenAtual != null && tokenAtual.Tipo == TipoToken.AND)
            {
                int linha = tokenAtual.Linha;
                int coluna = tokenAtual.Coluna;

                VerificarTipoToken(TipoToken.AND);
                
                Expressao direita = AnalisarExpressaoComparacao();

                esquerda = new And(esquerda, direita, linha, coluna);
            }

            return esquerda;
        }

        private Expressao AnalisarExpressaoComparacao()
        {
            Expressao esquerda = AnalisarExpressaoAditiva();

            if (tokenAtual != null && tokenAtual.Tipo == TipoToken.LESS_THAN)
            {
                int linha = tokenAtual.Linha;
                int coluna = tokenAtual.Coluna;

                VerificarTipoToken(TipoToken.LESS_THAN);
                
                Expressao direita = AnalisarExpressaoAditiva();
                
                return new LessThan(esquerda, direita, linha, coluna);
            }

            return esquerda;
        }

        private Expressao AnalisarExpressaoAditiva()
        {
            Expressao esquerda = AnalisarExpressaoMultiplicativa();

            while (tokenAtual != null && (tokenAtual.Tipo == TipoToken.PLUS || tokenAtual.Tipo == TipoToken.MINUS))
            {
                int linha = tokenAtual.Linha;
                int coluna = tokenAtual.Coluna;
                
                TipoToken operador = tokenAtual.Tipo;
                
                VerificarTipoToken(operador);

                Expressao direita = AnalisarExpressaoMultiplicativa();

                if (operador == TipoToken.PLUS)
                    esquerda = new Plus(esquerda, direita, linha, coluna);
                else
                    esquerda = new Minus(esquerda, direita, linha, coluna);
            }

            return esquerda;
        }

        private Expressao AnalisarExpressaoMultiplicativa()
        {
            Expressao esquerda = AnalisarExpressaoUnaria();

            while (tokenAtual != null && tokenAtual.Tipo == TipoToken.TIMES)
            {
                int linha = tokenAtual.Linha;
                int coluna = tokenAtual.Coluna;

                VerificarTipoToken(TipoToken.TIMES);
                
                Expressao direita = AnalisarExpressaoUnaria();
                
                esquerda = new Times(esquerda, direita, linha, coluna);
            }

            return esquerda;
        }

        private Expressao AnalisarExpressaoUnaria()
        {
            if (tokenAtual != null && tokenAtual.Tipo == TipoToken.NOT)
            {
                int linha = tokenAtual.Linha;
                int coluna = tokenAtual.Coluna;

                VerificarTipoToken(TipoToken.NOT);
                
                Expressao expressao = AnalisarExpressaoUnaria();
                
                return new Not(expressao, linha, coluna);
            }

            return AnalisarExpressaoPosFixada();
        }

        private Expressao AnalisarExpressaoPosFixada()
        {
            Expressao expressao = AnalisarExpressaoPrimaria();

            while (tokenAtual != null)
            {
                int linha = tokenAtual.Linha;
                int coluna = tokenAtual.Coluna;

                if (tokenAtual.Tipo == TipoToken.LBRACKET)
                {
                    VerificarTipoToken(TipoToken.LBRACKET);
                    
                    Expressao indice = AnalisarExpressao();
                    
                    VerificarTipoToken(TipoToken.RBRACKET);

                    expressao = new ArrayLookup(expressao, indice, linha, coluna);
                }
                else if (tokenAtual.Tipo == TipoToken.DOT)
                {
                    VerificarTipoToken(TipoToken.DOT);

                    if (tokenAtual.Tipo == TipoToken.LENGTH)
                    {
                        VerificarTipoToken(TipoToken.LENGTH);
                        expressao = new ArrayLength(expressao, linha, coluna);
                    }
                    else if (tokenAtual.Tipo == TipoToken.IDENTIFIER)
                    {
                        Identificador nomeMetodo = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);
                        VerificarTipoToken(TipoToken.IDENTIFIER);
                        VerificarTipoToken(TipoToken.LPARENTHESE);

                        List<Expressao> lstArgumento = new List<Expressao>();

                        if (tokenAtual.Tipo != TipoToken.RPARENTHESE)
                        {
                            lstArgumento.Add(AnalisarExpressao());

                            while (tokenAtual.Tipo == TipoToken.COMMA)
                            {
                                VerificarTipoToken(TipoToken.COMMA);
                                lstArgumento.Add(AnalisarExpressao());
                            }
                        }

                        VerificarTipoToken(TipoToken.RPARENTHESE);
                        expressao = new Call(expressao, nomeMetodo, lstArgumento, linha, coluna);
                    }
                }
                else
                    break;
            }

            return expressao;
        }
        
        private Expressao AnalisarExpressaoPrimaria()
        {
            int linha = tokenAtual.Linha;
            int coluna = tokenAtual.Coluna;

            if (tokenAtual.Tipo == TipoToken.INTEGER_LITERAL)
            {
                int value = int.Parse(tokenAtual.Lexema);

                VerificarTipoToken(TipoToken.INTEGER_LITERAL);

                return new IntegerLiteral(value, linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.TRUE)
            {
                VerificarTipoToken(TipoToken.TRUE);

                return new True(linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.FALSE)
            {
                VerificarTipoToken(TipoToken.FALSE);

                return new False(linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.IDENTIFIER)
            {
                string nome = tokenAtual.Lexema;

                VerificarTipoToken(TipoToken.IDENTIFIER);
                
                return new IdentifierExp(nome, linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.THIS)
            {
                VerificarTipoToken(TipoToken.THIS);

                return new This(linha, coluna);
            }
            else if (tokenAtual.Tipo == TipoToken.NEW)
            {
                VerificarTipoToken(TipoToken.NEW);

                if (tokenAtual.Tipo == TipoToken.INT)
                {
                    VerificarTipoToken(TipoToken.INT);
                    VerificarTipoToken(TipoToken.LBRACKET);

                    Expressao tamanho = AnalisarExpressao();
                    
                    VerificarTipoToken(TipoToken.RBRACKET);
                    
                    return new NewArray(tamanho, linha, coluna);
                }
                else if (tokenAtual.Tipo == TipoToken.IDENTIFIER)
                {
                    Identificador nomeClasse = new Identificador(tokenAtual.Lexema, tokenAtual.Linha, tokenAtual.Coluna);
                    VerificarTipoToken(TipoToken.IDENTIFIER);
                    VerificarTipoToken(TipoToken.LPARENTHESE);
                    VerificarTipoToken(TipoToken.RPARENTHESE);
                    return new NewObject(nomeClasse, linha, coluna);
                }
            }
            else if (tokenAtual.Tipo == TipoToken.LPARENTHESE)
            {
                VerificarTipoToken(TipoToken.LPARENTHESE);

                Expressao expressao = AnalisarExpressao();
                
                VerificarTipoToken(TipoToken.RPARENTHESE);
                
                return expressao;
            }

            throw new ParserException($"Expressão esperada na linha {tokenAtual.Linha}, coluna {tokenAtual.Coluna}");
        }
    }

    public class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }
    }
}