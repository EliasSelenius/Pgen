using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {

    /// <summary>
    /// A sequenze of rules (Lexrule or Parserule).
    /// </summary>
    public class Pattern {

        public IRule[] rules;

        public Pattern(IRule[] rules) {
            this.rules = rules;
        }

        public bool Match(TokenReader tr) {
            int startIndex = tr.index;
            for (int i = 0; i < rules.Length; i++) {
                if (!rules[i].ParseMatch(tr)) {
                    tr.index = startIndex;
                    return false;
                }
            }
            return true;
        }
    }
}
