using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pgen {
    public class Parserule : IRule {

        public string name { get; }

        public bool createNode { get; set; }

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
                patterns[i] = new Pattern(this, ps[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => parser.GetRuleWithModifier(x)).ToArray());
            }
        }

        public bool ParseMatch(TokenReader tr, SyntaxTree.Node node) {

            var n = createNode ? new SyntaxTree.Node(this, "") : node;

            for (int i = 0; i < patterns.Length; i++) {
                if (patterns[i].Match(tr, n)) {
                    if (createNode) node.AddChild(n);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// A sequenze of rules (Lexrule or Parserule).
        /// </summary>
        public class Pattern {

            private readonly Parserule rule;
            private readonly IRule[] rules;

            private int nodeCount {
                get {
                    int i = 0;
                    for (int x = 0; x < rules.Length; x++) {
                        if (rules[x].createNode) i++;
                    }
                    return i;
                }
            }

            public Pattern(Parserule rule, IRule[] rules) {
                this.rule = rule;
                this.rules = rules;
            }

            public bool Match(TokenReader tr, SyntaxTree.Node node) {
                int startIndex = tr.index;
                int startChildCount = node.childCount;
                for (int i = 0; i < rules.Length; i++) {
                    if (!rules[i].ParseMatch(tr, node)) {
                        // fail. reset tokenreader index and syntaxtree nodes
                        tr.index = startIndex;
                        while (node.childCount > startChildCount) node.RemoveChild(node.childCount - 1);

                        return false;
                    }
                }
                return true;
            }
        }

    }
}
