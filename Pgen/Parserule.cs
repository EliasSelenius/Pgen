using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pgen {
    public class Parserule : IRule {
        public readonly string name;

        private readonly string pattern;
        private Pattern[] patterns;

        public Parserule(string name, string pattern) {
            this.name = name;
            this.pattern = pattern;
        }

        internal void Initialize(Parser parser) {

            var ps = pattern.Split('|');
            patterns = new Pattern[ps.Length];

            for (int i = 0; i < ps.Length; i++) {
                patterns[i] = new Pattern(ps[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => parser.GetRuleWithModifier(x)).ToArray());
            }

        }

        public bool ParseMatch(TokenReader tr) {
            for (int i = 0; i < patterns.Length; i++) {
                if (patterns[i].Match(tr)) return true;
            }
            return false;
        }

    }
}
