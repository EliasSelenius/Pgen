using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    public class Token {
        public readonly LexRule Rule;
        public readonly string Value;

        internal Token(LexRule rule, string value) {
            Rule = rule; Value = value;
        }
    }
}
