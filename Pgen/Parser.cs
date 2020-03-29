using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pgen {
    public abstract class Parser {

        public Lexer lexer { get; private set; }

        private readonly List<Parserule> parserules = new List<Parserule>();
        
        public Parser() {
            initializeLexer();
        }

        private void initializeLexer() {
            var thisType = this.GetType();

            var fields = thisType.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Where(x => x.IsDefined(typeof(RuleAttribute), false));

            var lexrules = new List<Lexrule>();
            var lexrulesProps = fields.Where(x => x.FieldType == typeof(Lexrule));
            foreach (var item in lexrulesProps) {
                var at = item.GetCustomAttributes(typeof(RuleAttribute), false)[0] as RuleAttribute;
                var lr = new Lexrule(item.Name, at.Pattern);
                lexrules.Add(lr);
                item.SetValue(this, lr);
            }

            lexer = new Lexer(lexrules);

        }

        public IRule GetRule(string name) {
            // search lex rules:
            IRule r = lexer.GetRule(name);
            if (r != null) return r;
            // search parse rules:
            return parserules.Where(x => x.name.Equals(name)).FirstOrDefault();
        }

        public void Parse(string input) {
            var reader = new TokenReader(lexer.Lex(input));
            
        }

    
    }

    [System.AttributeUsage(AttributeTargets.Field)]
    public sealed class RuleAttribute : Attribute {
        public readonly string Pattern;
        public RuleAttribute(string pattern) {
            Pattern = pattern;
        }
    }
}
