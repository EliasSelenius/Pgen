using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    public class LexerException : Exception {
        public LexerException(string message) : base(message) {
        }
    }

    public class ParserException : Exception {
        public ParserException(string message) : base(message) {
        }
    }
}
