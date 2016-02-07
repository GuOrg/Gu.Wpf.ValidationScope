namespace Gu.Wpf.ValidationScope
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Data;

    public class AggregateErrors
    {
        private readonly BindingExpression errorCountExpression;
        private readonly List<AggregateErrors> childErrors = new List<AggregateErrors>();

        internal AggregateErrors(BindingExpression errorCountExpression)
        {
            this.errorCountExpression = errorCountExpression;
        }

        public AggregateErrors(AggregateErrors childErrors)
        {
            this.childErrors.Add(childErrors);
        }

        public bool HasErrors
        {
            get
            {
                var hasError = System.Windows.Controls.Validation.GetHasError(this.Source);
                if (hasError)
                {
                    return true;
                }

                for (int i = this.childErrors.Count - 1; i >= 0; i--)
                {
                    var child = this.childErrors[i];
                    //if (child.Source == null)
                    //{
                    //    this.childErrors.RemoveAt(i);
                    //}

                    if (child.HasErrors)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private DependencyObject Source => (DependencyObject)this.errorCountExpression?.ParentBinding.Source;

        public void UpdateChildErrors(AggregateErrors errors, bool hasError)
        {
            if (hasError)
            {
                if (!this.childErrors.Contains(errors))
                {
                    this.childErrors.Add(errors);
                }
            }
            else
            {
                this.childErrors.Remove(errors);
            }
        }
    }
}