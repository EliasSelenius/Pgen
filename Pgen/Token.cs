using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    public class Token {
        public readonly Lexrule type;
        public readonly string value;

        internal Token(Lexrule rule, string value) {
            type = rule; this.value = value;
        }
    }
}
