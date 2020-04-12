using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    public interface IRule {
        string name { get; }
        /// <summary>
        /// value indicating wheter or not this rule should create a node in the syntaxtree
        /// </summary>
        bool createNode { get; }
        bool ParseMatch(TokenReader tr, SyntaxTree.Node node);
    }
}
