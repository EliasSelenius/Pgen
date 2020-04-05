using System;
using System.IO;
using Pgen;

namespace Pgen.Demo {
    class Program {
        static void Main(string[] args) {

            var p = new testJsonParser();
            p.Parse(File.ReadAllText("data/test1.txt"));

            Console.WriteLine("Hello World!");
        }
    }

    public class testJsonParser : Parser {

        [Rule("\\s+"),Skip] Lexrule whitespace;
        [Rule("\\d+")] Lexrule number;
        [Rule("true|false")] Lexrule boolean;
        [Rule("[a-zA-Z]+")] Lexrule name;
        [Rule(":")] Lexrule colon;
        [Rule(",")] Lexrule comma;
        [Rule("\\[")] Lexrule openSqbr;
        [Rule("\\]")] Lexrule closeSqbr;
        [Rule("{")] Lexrule openCurl;
        [Rule("}")] Lexrule closeCurl;

        [Rule("number | boolean | list | compund")] Parserule value;

        // lists:
        [Rule("value comma")] Parserule list_item;
        [Rule("openSqbr list_item* value closeSqbr | openSqbr closeSqbr")] Parserule list;
        
        // object:
        [Rule("name colon value")] Parserule keyvaluepair;        
        [Rule("keyvaluepair comma")] Parserule object_item;
        [Rule("openCurl object_item* keyvaluepair closeCurl | openCurl closeCurl")] Parserule compund;
        
    }
}
