using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pgen {
    public class Lexer {

        private readonly List<Lexrule> rules;

        public Lexer(params Lexrule[] lexRules) {
            rules = lexRules.ToList();
        }
        public Lexer(List<Lexrule> lexRules) {
            rules = lexRules;
        }

        public Lexrule GetRule(int i) => rules[i];
        public Lexrule GetRule(string name) => (from r in rules
                                                where r.name == name
                                                select r).FirstOrDefault();



        public List<Token> Lex(string source) {
            var res = new List<Token>();

            while (source != string.Empty) {
                var t = nextToken(source);
                source = source.Remove(0, t.value.Length);
                res.Add(t);
            }

            return res;
        }

        private Token nextToken(string source) {
            for (int i = 0; i < rules.Count; i++) {
                var t = rules[i].Match(source);
                if (t != null) return t;
            }
            throw new LexerException($"Unrecognized Token:\nHERE=>{source}");
        }
    }
}
