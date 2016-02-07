namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public class ValidationScopeTypes : List<Type>
    {
        public bool IsScopeFor(DependencyObject dependencyObject)
        {
            return this.Any(x => x.IsInstanceOfType(dependencyObject));
        }
    }
}