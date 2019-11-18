using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pgen {
    public class Lexer {

        private readonly List<LexRule> rules = new List<LexRule>();

        public Lexer(params LexRule[] lexRules) {
            rules.AddRange(lexRules);
        }

        public LexRule GetRule(int i) => rules[i];
        public LexRule GetRule(string name) => (from r in rules
                                                where r.Name == name
                                                select r).FirstOrDefault();

        public List<Token> Lex(string source) {
            var res = new List<Token>();

            while (source != string.Empty) {
                for (int i = 0; i < rules.Count; i++) {
                    if (i == rules.Count) {
                        throw new Exception($"Unexpected Token:\nHERE=>{source}");
                    }
                    var rule = rules[i];
                    var t = rule.Match(source);
                    if (t != null) {
                        source = source.Remove(0, t.Value.Length);
                        res.Add(t);
                        break;
                    }
                }
            }

            return res;
        }


    }
}
