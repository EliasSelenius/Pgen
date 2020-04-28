using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pgen {
    public class Lexrule : IRule {

        public string name { get; }
        public bool createNode { get; set; }


        public readonly bool skipable;
        private readonly Regex regex;


        public Lexrule(string name, bool skip, string regx, RegexOptions options) {
            this.name = name;
            this.skipable = skip;
            regex = new Regex($"^(?:{regx})", options); 
        }

        public Lexrule(string name, bool skip, string regx) : this(name, skip, regx, RegexOptions.None) { }
        public Lexrule(string name, string regx) : this(name, false, regx, RegexOptions.None) { }

        public Token Match(string source) {
            var m = regex.Match(source);
            return m.Success ? new Token(this, m.Value) : null;
        }

        public bool ParseMatch(TokenReader tr, SyntaxTree.Node node) {
            if (tr.Peek()?.type == this) {
                tr.Next();
                if (createNode) node.AddChild(new SyntaxTree.Node(this, tr.Current.value));
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Collection of commonly used token types
    /// </summary>
    public static class Lexrules {
        public static readonly Lexrule Whitespace = new Lexrule("whitespace", "\\s+");
        public static readonly Lexrule String = new Lexrule("string", "(?:\"\")|(?:\".*?[^\\\\]\")");
        public static readonly Lexrule Boolean = new Lexrule("bool", "true|false");
        public static readonly Lexrule Number = new Lexrule("number", "-?\\d+(\\.\\d+)?");

    }

}
