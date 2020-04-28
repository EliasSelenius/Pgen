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

            public readonly string content;

            private List<Node> children;

            public int childCount => children?.Count ?? 0;

            public void AddChild(Node node) => (children ??= new List<Node>()).Add(node);
            public Node GetChild(int index) => children?[index];
            public void RemoveChild(Node node) => children?.Remove(node);
            public void RemoveChild(int index) => children?.RemoveAt(index);

            internal Node(IRule rule, string content) {
                this.rule = rule;
                this.content = content;
            }
            internal string AsText(string prepend) {
                var res = prepend;
                res += rule.name + (!string.IsNullOrEmpty(content) ? ": " + content.Replace("\n", "") : "") + "\n";
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
