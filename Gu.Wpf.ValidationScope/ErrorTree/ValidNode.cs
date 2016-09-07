namespace Gu.Wpf.ValidationScope
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    public class ValidNode : Node
    {
        public static readonly ValidNode Default = new ValidNode();

        private ValidNode()
        {
        }

        public override bool HasError => false;

        public override ReadOnlyObservableCollection<ValidationError> Errors { get; } = ErrorCollection.EmptyValidationErrors;

        public override ReadOnlyObservableCollection<ErrorNode> Children { get; } = ChildCollection.Empty;
    }
}