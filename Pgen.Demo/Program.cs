using System;
using System.IO;
using Pgen;

namespace Pgen.Demo {
    class Program {
        static void Main(string[] args) {

            var p = new testParser();

            Console.WriteLine("Hello World!");
        }
    }

    public class testParser : Parser {

        [Rule("\\s+")] 
        public readonly Lexrule whitespace;
        [Rule("\\d+")] 
        public readonly Lexrule number;
        [Rule("true|false")]
        public readonly Lexrule boolean;


    }
}
