using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    public class TokenReader {

        private readonly List<Token> tokens;
        public int index = -1;


        public Token Current => tokens[index];

        public bool IsEnd => index == (tokens.Count - 1);
        public bool IsStart => index == -1;

        public TokenReader(List<Token> tokens) {
            this.tokens = tokens;
        }

        private Token getToken(int index) {
            return index >= 0 && index < tokens.Count ? tokens[index] : null;
        }

        /// <summary>
        /// Moves to the next token and returns it
        /// </summary>
        /// <returns></returns>
        public Token Next() => getToken(++index);
        /// <summary>
        /// returns the next token, but does not move to it
        /// </summary>
        /// <returns></returns>
        public Token Peek() => getToken(index + 1);

        /// <summary>
        /// throws a parser exception if the current tokens type is not the specified value
        /// </summary>
        /// <param name="lexrule"></param>
        public void Assert(Lexrule lexrule) {
            if (Current.type != lexrule) {
                throw new ParserException("TokneReader assertion failed");
            }
        }
        /// <summary>
        /// Moves to the next token and asserts that it is of the given type
        /// </summary>
        /// <param name="lexrule"></param>
        /// <returns></returns>
        public Token AssertNext(Lexrule lexrule) {
            var r = Next();
            Assert(lexrule);
            return r;
        }
        /// <summary>
        /// Moves to the next token if the current token is of the given type, othervise does nothing
        /// </summary>
        /// <param name="lexrule"></param>
        public void SkipIf(Lexrule lexrule) {
            if (Current.type == lexrule) Next();
        }

        public string TokensAsText() {
            string res = "";
            foreach (var item in tokens) {
                res += item.type.name + "(" + item.value + "), ";
            }
            return res;
        }

    }
}
