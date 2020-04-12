using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    abstract class ModifiedRule : IRule {
        private IRule inner;

        public virtual string name => "Modified_" + inner.name;
        public bool createNode => inner.createNode;

        public virtual bool ParseMatch(TokenReader tr, SyntaxTree.Node node) => inner.ParseMatch(tr, node);

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
            public override string name => "Optional(" + inner.name + ")";
            public override bool ParseMatch(TokenReader tr, SyntaxTree.Node node) {
                base.ParseMatch(tr, node);
                return true;
            }

        }

        public class ZeroOrMore : ModifiedRule {
            public override string name => "ZeroOrMore(" + inner.name + ")";
            public override bool ParseMatch(TokenReader tr, SyntaxTree.Node node) {
                /*var con = true;
                while (con) {
                    con = base.ParseMatch(tr, node);
                }*/

                while (base.ParseMatch(tr, node)) { }

                return true;
            }
        }

        public class OneOrMore : ModifiedRule {
            public override string name => "OneOrMore(" + inner.name + ")";

            public override bool ParseMatch(TokenReader tr, SyntaxTree.Node node) {
                
                if (!base.ParseMatch(tr, node)) return false;
                
                /*var con = true;
                while (con) {
                    con = base.ParseMatch(tr, node);
                }*/

                while (base.ParseMatch(tr, node)) { }

                return true;
            }
        }
    }
}
