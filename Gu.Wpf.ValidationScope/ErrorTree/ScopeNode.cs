using System.Collections.Generic;

namespace Gu.Wpf.ValidationScope
{
    public class ScopeNode : IErrorNode
    {
        private readonly ImmutableList<IErrorNode> children;

        public ScopeNode(IErrorNode childError)
        {
            this.children = ImmutableList<IErrorNode>.Empty.Add(childError);
        }

        public IReadOnlyList<IErrorNode> Children => this.children;
    }
}