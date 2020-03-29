using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    public class Parserule : IRule {
        public readonly string name;

        private readonly Pattern[] patterns;

        public Parserule(string name, params Pattern[] ps) {
            this.name = name; patterns = ps;
        }

        public bool ParseMatch(TokenReader tr) {
            for (int i = 0; i < patterns.Length; i++) {
                if (patterns[i].Match(tr)) return true;
            }
            return false;
        }

    }
}
