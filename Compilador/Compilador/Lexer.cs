using System.Collections.Generic;
using System.Text;

namespace Compilador
{
    public class Lexer
    {
        private readonly string codigoFonte;
        private int posicao;
        private int linha;
        private int coluna;
        private char caractereAtual;

        /// <summary>
        /// Construtor do analisador léxico
        /// </summary>
        /// <param name="codigoFonte">Código fonte a ser analisado</param>
        public Lexer(string codigoFonte)
        {
            this.codigoFonte = codigoFonte;
            posicao = 0;
            linha = 1;
            coluna = 1;

            // Inicializa com o primeiro caractere ou '\0' se vazio
            caractereAtual = codigoFonte.Length > 0 ? codigoFonte[0] : '\0';
        }

        /// <summary>
        /// Avança para o próximo caractere no código fonte
        /// Atualiza linha e coluna conforme necessário
        /// </summary>
        private void Avancar()
        {
            // Se encontrou quebra de linha, incrementa linha e reseta coluna
            if (caractereAtual == '\n')
            {
                linha++;
                coluna = 1;
            }
            else
            {
                coluna++;
            }

            posicao++;
            // Atualiza caractere atual ou '\0' se chegou ao fim
            caractereAtual = posicao < codigoFonte.Length ? codigoFonte[posicao] : '\0';
        }

        /// <summary>
        /// Olha à frente no código fonte sem consumir o caractere
        /// Útil para decisões de análise (ex: verificar se // é comentário)
        /// </summary>
        /// <param name="deslocamento">Quantos caracteres à frente olhar (padrão: 1)</param>
        /// <returns>Caractere na posição especificada ou '\0'</returns>
        private char Espiar(int deslocamento = 1)
        {
            int pos = posicao + deslocamento;
            return pos < codigoFonte.Length ? codigoFonte[pos] : '\0';
        }

        /// <summary>
        /// Pula todos os espaços em branco, tabs e quebras de linha
        /// Esses caracteres não geram tokens, são apenas separadores
        /// </summary>
        private void PularEspacosEmBranco()
        {
            while (char.IsWhiteSpace(caractereAtual))
            {
                Avancar();
            }
        }

        /// <summary>
        /// Pula comentários no código fonte
        /// Suporta dois tipos:
        /// - Comentário de linha: // até o fim da linha
        /// - Comentário de bloco: /* até encontrar */
        /// </summary>
        private void PularComentario()
        {
            // Comentário de linha: //
            if (caractereAtual == '/' && Espiar() == '/')
            {
                // Avança até encontrar quebra de linha ou fim do arquivo
                while (caractereAtual != '\n' && caractereAtual != '\0')
                    Avancar();
            }
            // Comentário de bloco: /* */
            else if (caractereAtual == '/' && Espiar() == '*')
            {
                Avancar(); // Pula '/'
                Avancar(); // Pula '*'

                // Continua até encontrar */ ou fim do arquivo
                while (!(caractereAtual == '*' && Espiar() == '/') && caractereAtual != '\0')
                {
                    Avancar();
                }

                // Se encontrou o fechamento do comentário
                if (caractereAtual == '*')
                {
                    Avancar(); // Pula '*'
                    Avancar(); // Pula '/'
                }
            }
        }

        /// <summary>
        /// Lê um número inteiro do código fonte
        /// Continua lendo enquanto encontrar dígitos
        /// </summary>
        /// <returns>Token do tipo INTEGER_LITERAL com o número lido</returns>
        private Token LerNumero()
        {
            var sb = new StringBuilder();
            int linhaInicio = linha;
            int colunaInicio = coluna;

            // Continua lendo enquanto for dígito (0-9)
            while (char.IsDigit(caractereAtual))
            {
                sb.Append(caractereAtual);
                Avancar();
            }

            return new Token(EnumToken.TipoToken.INTEGER_LITERAL, sb.ToString(), linhaInicio, colunaInicio);
        }

        /// <summary>
        /// Lê um identificador ou palavra reservada
        /// Identificadores: começam com letra ou _, seguidos de letras, dígitos ou _
        /// Exemplos: variavel, _temp, contador123
        /// </summary>
        /// <returns>Token do tipo ID ou palavra reservada</returns>
        private Token LerIdentificadorOuPalavraReservada()
        {
            var sb = new StringBuilder();
            int linhaInicio = linha;
            int colunaInicio = coluna;

            // Continua lendo enquanto for letra, dígito ou underscore
            while (char.IsLetterOrDigit(caractereAtual) || caractereAtual == '_')
            {
                sb.Append(caractereAtual);
                Avancar();
            }

            string texto = sb.ToString();

            // Verifica se é uma palavra reservada (keyword)
            if (palavrasReservadas.ContainsKey(texto))
                return new Token(palavrasReservadas[texto], texto, linhaInicio, colunaInicio);

            // Se não for palavra reservada, é um identificador
            return new Token(EnumToken.TipoToken.ID, texto, linhaInicio, colunaInicio);
        }

        /// <summary>
        /// Obtém o próximo token do código fonte
        /// Este é o método principal do analisador léxico
        /// </summary>
        /// <returns>Próximo token identificado</returns>
        public Token ProximoToken()
        {
            // Loop principal: continua até acabar o código
            while (caractereAtual != '\0')
            {
                // Pula espaços em branco (não geram tokens)
                if (char.IsWhiteSpace(caractereAtual))
                {
                    PularEspacosEmBranco();
                    continue;
                }

                // Pula comentários (não geram tokens)
                if (caractereAtual == '/' && (Espiar() == '/' || Espiar() == '*'))
                {
                    PularComentario();
                    continue;
                }

                // Salva posição inicial do token (para mensagens de erro)
                int linhaInicio = linha;
                int colunaInicio = coluna;

                // Reconhece números (começam com dígito)
                if (char.IsDigit(caractereAtual))
                    return LerNumero();

                // Reconhece identificadores e palavras reservadas (começam com letra ou _)
                if (char.IsLetter(caractereAtual) || caractereAtual == '_')
                    return LerIdentificadorOuPalavraReservada();

                // Reconhece operadores e delimitadores (símbolos)
                switch (caractereAtual)
                {
                    case '(':
                        Avancar();
                        return new Token(EnumToken.TipoToken.LPAREN, "(", linhaInicio, colunaInicio);

                    case ')':
                        Avancar();
                        return new Token(EnumToken.TipoToken.RPAREN, ")", linhaInicio, colunaInicio);

                    case '{':
                        Avancar();
                        return new Token(EnumToken.TipoToken.LBRACE, "{", linhaInicio, colunaInicio);

                    case '}':
                        Avancar();
                        return new Token(EnumToken.TipoToken.RBRACE, "}", linhaInicio, colunaInicio);

                    case '[':
                        Avancar();
                        return new Token(EnumToken.TipoToken.LBRACKET, "[", linhaInicio, colunaInicio);

                    case ']':
                        Avancar();
                        return new Token(EnumToken.TipoToken.RBRACKET, "]", linhaInicio, colunaInicio);

                    case ';':
                        Avancar();
                        return new Token(EnumToken.TipoToken.SEMICOLON, ";", linhaInicio, colunaInicio);

                    case ',':
                        Avancar();
                        return new Token(EnumToken.TipoToken.COMMA, ",", linhaInicio, colunaInicio);

                    case '.':
                        Avancar();
                        return new Token(EnumToken.TipoToken.DOT, ".", linhaInicio, colunaInicio);

                    case '+':
                        Avancar();
                        return new Token(EnumToken.TipoToken.PLUS, "+", linhaInicio, colunaInicio);

                    case '-':
                        Avancar();
                        return new Token(EnumToken.TipoToken.MINUS, "-", linhaInicio, colunaInicio);

                    case '*':
                        Avancar();
                        return new Token(EnumToken.TipoToken.TIMES, "*", linhaInicio, colunaInicio);

                    case '!':
                        Avancar();
                        return new Token(EnumToken.TipoToken.NOT, "!", linhaInicio, colunaInicio);

                    case '<':
                        Avancar();
                        return new Token(EnumToken.TipoToken.LESS_THAN, "<", linhaInicio, colunaInicio);

                    case '=':
                        Avancar();
                        return new Token(EnumToken.TipoToken.EQUALS, "=", linhaInicio, colunaInicio);

                    // Operador && (precisa de dois caracteres)
                    case '&':
                        if (Espiar() == '&')
                        {
                            Avancar(); // Pula primeiro '&'
                            Avancar(); // Pula segundo '&'
                            return new Token(EnumToken.TipoToken.AND, "&&", linhaInicio, colunaInicio);
                        }
                        break;
                }

                // Se chegou aqui, é um caractere não reconhecido (erro léxico)
                char caractereErro = caractereAtual;
                Avancar();

                return new Token(EnumToken.TipoToken.ERROR, caractereErro.ToString(), linhaInicio, colunaInicio);
            }

            // Fim do arquivo: retorna token EOF (End Of File)
            return new Token(EnumToken.TipoToken.EOF, "", linha, coluna);
        }

        /// <summary>
        /// Tokeniza todo o código fonte de uma vez
        /// Retorna uma lista com todos os tokens identificados
        /// </summary>
        /// <returns>Lista de tokens do código fonte</returns>
        public List<Token> Tokenizar()
        {
            var tokens = new List<Token>();
            Token token;

            // Continua obtendo tokens até encontrar EOF
            do
            {
                token = ProximoToken();
                tokens.Add(token);
            } while (token.Tipo != EnumToken.TipoToken.EOF);

            return tokens;
        }

        /// <summary>
        /// Dicionário de palavras reservadas da linguagem MiniJava
        /// Mapeia cada palavra reservada para seu tipo de token correspondente
        /// Estas palavras não podem ser usadas como identificadores
        /// </summary>
        private static readonly Dictionary<string, EnumToken.TipoToken> palavrasReservadas = new Dictionary<string, EnumToken.TipoToken>
        {
            { "class", EnumToken.TipoToken.CLASS },           
            { "public", EnumToken.TipoToken.PUBLIC },         
            { "static", EnumToken.TipoToken.STATIC },         
            { "void", EnumToken.TipoToken.VOID },             
            { "main", EnumToken.TipoToken.MAIN },             
            { "String", EnumToken.TipoToken.STRING },         
            { "extends", EnumToken.TipoToken.EXTENDS },       
            { "return", EnumToken.TipoToken.RETURN },         
            { "int", EnumToken.TipoToken.INT },               
            { "boolean", EnumToken.TipoToken.BOOLEAN },       
            { "if", EnumToken.TipoToken.IF },                 
            { "else", EnumToken.TipoToken.ELSE },             
            { "while", EnumToken.TipoToken.WHILE },           
            { "System.out.println", EnumToken.TipoToken.PRINT }, 
            { "length", EnumToken.TipoToken.LENGTH },         
            { "true", EnumToken.TipoToken.TRUE },             
            { "false", EnumToken.TipoToken.FALSE },           
            { "this", EnumToken.TipoToken.THIS },             
            { "new", EnumToken.TipoToken.NEW }                
        };
    }
}