namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;

    public interface IErrorNode
    {
        IReadOnlyList<IErrorNode> Children { get; }

        bool HasErrors { get;  }
    }
}