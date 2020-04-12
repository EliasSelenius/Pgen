using System;
using System.Collections.Generic;
using System.Text;

namespace Pgen {
    public class SyntaxTree {

        public readonly Node rootNode;

        public SyntaxTree(Node root) {
            rootNode = root;
        }

        public class Node {
            public readonly IRule rule;

            private List<Node> children;

            public int childCount => children?.Count ?? 0;

            public void AddChild(Node node) => (children ??= new List<Node>()).Add(node);
            public void RemoveChild(Node node) => children?.Remove(node);
            public void RemoveChild(int index) => children?.RemoveAt(index);

            internal Node(IRule rule) {
                this.rule = rule;
            }
            internal string AsText(string prepend) {
                var res = prepend;
                res += rule.name + "\n";
                if (children != null) {
                    foreach (var item in children) {
                        if (item == null) res += prepend + ".null\n";
                        else res += item.AsText(prepend + ".");
                    }
                }
                return res;
            }
        }

        public string AsText() => rootNode.AsText("");

    }
}
