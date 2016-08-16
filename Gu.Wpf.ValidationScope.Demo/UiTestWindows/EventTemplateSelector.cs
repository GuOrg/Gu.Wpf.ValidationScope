namespace Gu.Wpf.ValidationScope.Demo
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    public class EventTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }

        public DataTemplate ValidationErrorsCollectionTemplate { get; set; }

        public DataTemplate EmptyValidationErrorsCollectionTemplate { get; set; }

        public DataTemplate ValidationErrorEventArgsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is string)
            {
                return this.StringTemplate;
            }

            var collection = item as ReadOnlyObservableCollection<ValidationError>;
            if (collection != null)
            {
                if (collection.Count == 0)
                {
                    return this.EmptyValidationErrorsCollectionTemplate;
                }

                return this.ValidationErrorsCollectionTemplate;
            }

            var node = item as IErrorNode;
            if (node != null)
            {
                if (node.Count == 0)
                {
                    return this.EmptyValidationErrorsCollectionTemplate;
                }

                return this.ValidationErrorsCollectionTemplate;
            }

            if (item is ValidationErrorEventArgs)
            {
                return this.ValidationErrorEventArgsTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
