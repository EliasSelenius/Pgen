using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pgen {
    public abstract class Parser {

        public Lexer lexer { get; private set; }

        private readonly List<Parserule> parserules = new List<Parserule>();
        
        public Parser() {
            initialize();
        }

        private void initialize() {
            var thisType = this.GetType();

            var fields = thisType.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Where(x => x.IsDefined(typeof(RuleAttribute), false));


            // init lexer:
            var lexrules = new List<Lexrule>();
            var lexrulesProps = fields.Where(x => x.FieldType == typeof(Lexrule));
            foreach (var item in lexrulesProps) {
                var at = item.GetCustomAttributes(typeof(RuleAttribute), false)[0] as RuleAttribute;
                var skipat = item.GetCustomAttributes(typeof(SkipAttribute), false).FirstOrDefault();
                var lr = new Lexrule(item.Name, skipat != null, at.Pattern);
                lexrules.Add(lr);
                item.SetValue(this, lr);
            }
            lexer = new Lexer(lexrules);


            // init parserules:
            var parserulesProps = fields.Where(x => x.FieldType == typeof(Parserule));
            foreach (var item in parserulesProps) {
                var at = item.GetCustomAttributes(typeof(RuleAttribute), false)[0] as RuleAttribute;
                var rule = new Parserule(item.Name, at.Pattern);
                parserules.Add(rule);
                item.SetValue(this, rule);
            }

            foreach (var item in parserules) {
                item.Initialize(this);
            }

        }

        public IRule GetRule(string name) {
            // search lex rules:
            IRule r = lexer.GetRule(name);
            if (r != null) return r;
            // search parse rules:
            return parserules.Where(x => x.name.Equals(name)).FirstOrDefault();
        }

        internal IRule GetRuleWithModifier(string name) {
            /*
                    rule?
                    rule*
                    rule+
                    'rule'
             */

            
            return ModifiedRule.CreateModifiedRule(GetRule(name.TrimEnd('?', '*', '+')), name.Last());
        }

        public void Parse(string input) {
            var reader = new TokenReader(lexer.Lex(input));

            var mainRule = parserules[0]; // hard-code to first rule for now

            if (!mainRule.ParseMatch(reader))
                throw new ParserException("");

        }

    
    }

    [System.AttributeUsage(AttributeTargets.Field)]
    public sealed class RuleAttribute : Attribute {
        public readonly string Pattern;
        public RuleAttribute(string pattern) {
            Pattern = pattern;
        }
    }

    [System.AttributeUsage(AttributeTargets.Field)]
    public sealed class SkipAttribute : Attribute { }
}
