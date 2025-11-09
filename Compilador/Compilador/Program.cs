using System;
using System.Collections.Generic;
using static Compilador.EnumToken;

namespace Compilador
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Analisador Léxico MiniC# ===\n");

            string sampleProgram = @"
                                    class Factorial {
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

            Console.WriteLine("Código de entrada:");
            Console.WriteLine(sampleProgram);
            Console.WriteLine("\n" + new string('=', 60) + "\n");

            Lexer lexer = new Lexer(sampleProgram);

            List<Token> tokens = lexer.Tokenizar();

            Console.WriteLine("Tokens reconhecidos:\n");

            foreach (Token token in tokens)
            {
                if (token.Tipo != TipoToken.EOF)
                    Console.WriteLine(token);
            }

            Console.WriteLine($"\n\nTotal de tokens: {tokens.Count - 1}");
            Console.WriteLine("\n\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}