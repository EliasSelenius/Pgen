using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace Pgen {
    public class LexRule {
        public readonly string Name;

        private readonly Regex regex;

        public LexRule(string name, string regx, RegexOptions options) {
            Name = name; regex = new Regex($"^({regx})", RegexOptions.Compiled | options); 
        }

        public LexRule(string name, string regx) : this(name, regx, RegexOptions.None) { }

        public Token Match(string source) {
            var m = regex.Match(source);
            return m.Success ? new Token(this, m.Value) : null;
        }

    }
}
