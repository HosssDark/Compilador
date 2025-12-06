using System;
using System.Collections.Generic;

namespace Compilador
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Compilador MiniC# - Análise Léxica e Sintática ===\n");

            string sampleProgram = @"class Factorial {
                                        public static void main(String[] a) {
                                            System.out.println(new Fac().ComputeFac(10));
                                        }
                                    }

                                    class Fac {
                                        public int ComputeFac(int num) {
                                            int num_aux;
                                            if (num < 1)
                                                num_aux = 1;
                                            else
                                                num_aux = num * (this.ComputeFac(num-1));
                                            return num_aux;
                                        }
                                    }";

            Console.WriteLine("========== CÓDIGO DE ENTRADA ==========");
            Console.WriteLine(sampleProgram);
            Console.WriteLine("\n" + new string('=', 70) + "\n");

            try
            {
                Console.WriteLine("========== ANÁLISE LÉXICA ==========\n");
                Lexer lexer = new Lexer(sampleProgram);
                List<Token> tokens = lexer.Tokenizar();

                Console.WriteLine($"Tokens reconhecidos: {tokens.Count - 1}\n");

                foreach (Token token in tokens)
                {
                    if (token.Tipo != EnumToken.TipoToken.EOF)
                        Console.WriteLine($"  {token}");
                }

                Console.WriteLine("\n" + new string('=', 70) + "\n");

                Console.WriteLine("========== ANÁLISE SINTÁTICA ==========\n");
                Parser parser = new Parser(tokens);
                NoPrograma programa = parser.Analisar();

                Console.WriteLine("Estrutura da AST gerada:\n");

                ConcatenarImpressaoSaida imprimir = new ConcatenarImpressaoSaida();
                programa.Aceitar(imprimir);
                Console.WriteLine(imprimir.ObterSaida());

                Console.WriteLine(new string('=', 70));
            }
            catch (ParserException ex)
            {
                Console.WriteLine($"\n ERRO DE SINTAXE: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n ERRO: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            Console.WriteLine("\n\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}