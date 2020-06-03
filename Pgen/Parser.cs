using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Pgen {
    public abstract class Parser {

        public Lexer lexer { get; private set; }

        private readonly List<Parserule> parserules = new List<Parserule>();
        
        public Parser() {
            initializeRules();
        }

        public Parser(string grammarCode) {
            // TODO: implement...
        }

        private void initializeRules() {
            var thisType = this.GetType();

            var fields = thisType.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Where(x => x.IsDefined(typeof(RuleAttribute), false));


            // init lexer:
            var lexrules = new List<Lexrule>();
            var lexrulesProps = fields.Where(x => x.FieldType == typeof(Lexrule));
            foreach (var item in lexrulesProps) {
                var at = item.GetCustomAttributes(typeof(RuleAttribute), false)[0] as RuleAttribute;
                var skipat = item.GetCustomAttributes(typeof(SkipAttribute), false).FirstOrDefault();
                var lr = new Lexrule(item.Name, skipat != null, at.pattern);
                lr.createNode = at.createNode;
                lexrules.Add(lr);
                item.SetValue(this, lr);
            }
            lexer = new Lexer(lexrules);


            // init parserules:
            var parserulesProps = fields.Where(x => x.FieldType == typeof(Parserule));
            foreach (var item in parserulesProps) {
                var at = item.GetCustomAttributes(typeof(RuleAttribute), false)[0] as RuleAttribute;
                var rule = new Parserule(item.Name, at.pattern);
                rule.createNode = at.createNode;
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
            return parserules.Where(x => x.name.Equals(name)).FirstOrDefault() ?? throw new Exception("'" + name + "' is not a defined rule");
        }

        private int inlineId = 0;

        internal IRule GetRuleWithModifier(string name) {
            /*
                    rule?
                    rule*
                    rule+
                    'rule'
             */
            bool sw(string c) => name.StartsWith(c);
            bool ew(string c) => name.EndsWith(c);
            if (ew("?") || ew("*") || ew("+"))
                return ModifiedRule.CreateModifiedRule(GetRule(name.TrimEnd('?', '*', '+')), name.Last());
            
            if (sw("'") && ew("'")) {
                var r = new Lexrule("inline" + inlineId++, name.Trim('\''));
                lexer.InsertRule(0, r);
                return r;
            }

            return GetRule(name);
        }

        public SyntaxTree Parse(string input) {
            var reader = new TokenReader(lexer.Lex(input));

            Console.WriteLine(reader.TokensAsText());

            var mainRule = parserules[0]; // hard-code to first rule for now

            var root = new SyntaxTree.Node(mainRule, "");
            if (!mainRule.ParseMatch(reader, root))
                throw new ParserException("");

            return new SyntaxTree(root);
        }

    
    }

    [System.AttributeUsage(AttributeTargets.Field)]
    public sealed class RuleAttribute : Attribute {
        public readonly string pattern;
        public readonly bool createNode;
        public RuleAttribute(string pattern, bool createNode = false) { 
            this.pattern = pattern;
            this.createNode = createNode;
        }
    }


    [System.AttributeUsage(AttributeTargets.Field)]
    public sealed class SkipAttribute : Attribute { }
}
