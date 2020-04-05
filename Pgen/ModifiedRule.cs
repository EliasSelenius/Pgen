using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    abstract class ModifiedRule : IRule {
        private IRule inner;

        public virtual bool ParseMatch(TokenReader tr) => inner.ParseMatch(tr);

        internal static IRule CreateModifiedRule(IRule inner, char type) {
            ModifiedRule rule = type switch {
                '?' => new Optional(),
                '*' => new ZeroOrMore(),
                '+' => new OneOrMore(),
                _ => null
            };
            if (rule == null) return inner;

            rule.inner = inner;
            return rule;
        }

        public class Optional : ModifiedRule {
            public override bool ParseMatch(TokenReader tr) {
                base.ParseMatch(tr); return true;
            }
        }

        public class ZeroOrMore : ModifiedRule {
            public override bool ParseMatch(TokenReader tr) {
                var con = true;
                while (con) {
                    con = base.ParseMatch(tr);
                }
                return true;
            }
        }

        public class OneOrMore : ModifiedRule {
            public override bool ParseMatch(TokenReader tr) {
                if (!base.ParseMatch(tr)) return false;
                var con = true;
                while (con) {
                    con = base.ParseMatch(tr);
                }
                return true;
            }
        }
    }
}
