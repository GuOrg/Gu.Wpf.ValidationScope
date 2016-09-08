namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    /// <summary>A valid node has no validation errors nor has any subnode any errors.</summary>
    public class ValidNode : Node
    {
        /// <summary>Cached instance as it is immutable.</summary>
        public static readonly ValidNode Default = new ValidNode();

        private ValidNode()
        {
        }

        public override bool HasError => false;

        public override ReadOnlyObservableCollection<ValidationError> Errors { get; } = ErrorCollection.EmptyValidationErrors;

        public override ReadOnlyObservableCollection<ErrorNode> Children { get; } = ChildCollection.Empty;
    }
}