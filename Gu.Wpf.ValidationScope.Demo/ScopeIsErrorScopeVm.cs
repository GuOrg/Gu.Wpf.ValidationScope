namespace Gu.Wpf.ValidationScope.Demo
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using JetBrains.Annotations;

    public class ScopeIsErrorScopeVm : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private int intValue;
        private bool hasErrors;
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors
        {
            get { return this.hasErrors; }
            set
            {
                if (value == this.hasErrors) return;
                this.hasErrors = value;
                this.OnPropertyChanged();
                this.OnErrorsChanged();
            }
        }

        public int IntValue
        {
            get { return this.intValue; }
            set
            {
                if (value == this.intValue) return;
                this.intValue = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return this.hasErrors && propertyName == nameof(this.HasErrors)
                ? new[] { "INotifyDataErrorInfo error" }
                : null;
        }

        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}