using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pgen {
    public class Lexrule : IRule {
        public readonly string name;

        private readonly Regex regex;

        public Lexrule(string name, string regx, RegexOptions options) {
            this.name = name; regex = new Regex($"^(?:{regx})", options); 
        }

        public Lexrule(string name, string regx) : this(name, regx, RegexOptions.None) { }

        public Token Match(string source) {
            var m = regex.Match(source);
            return m.Success ? new Token(this, m.Value) : null;
        }

        public bool ParseMatch(TokenReader tr) {
            if (tr.Peek().type == this) {
                tr.Next();
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
