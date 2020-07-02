namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    /// <summary>A valid node has no validation errors nor has any sub node any errors.</summary>
    public sealed class ValidNode : Node
    {
        /// <summary>Cached instance as it is immutable.</summary>
        public static readonly ValidNode Default = new ValidNode();

        private ValidNode()
        {
        }

        /// <inheritdoc/>
        public override bool HasError => false;

        /// <inheritdoc/>
        public override ReadOnlyObservableCollection<ValidationError> Errors { get; } = ErrorCollection.EmptyValidationErrors;

        /// <inheritdoc/>
        public override ReadOnlyObservableCollection<ErrorNode> Children { get; } = ChildCollection.Empty;
    }
}
