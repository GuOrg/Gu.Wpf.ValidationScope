namespace Gu.Wpf.ValidationScope.Demo
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class NotifyDataErrorInfoVm : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private int intValue1;
        private int intValue2;

        private string error1;

        private string error2;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => !(string.IsNullOrEmpty(this.Error1) && string.IsNullOrEmpty(this.Error2));

        public int IntValue1
        {
            get
            {
                return this.intValue1;
            }

            set
            {
                if (value == this.intValue1)
                {
                    return;
                }

                this.intValue1 = value;
                this.OnPropertyChanged();
            }
        }

        public string Error1
        {
            get
            {
                return this.error1;
            }

            set
            {
                if (value == this.error1)
                {
                    return;
                }

                this.error1 = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.HasErrors));
                this.OnErrorsChanged(nameof(this.IntValue1));
            }
        }

        public int IntValue2
        {
            get
            {
                return this.intValue2;
            }

            set
            {
                if (value == this.intValue2)
                {
                    return;
                }

                this.intValue2 = value;
                this.OnPropertyChanged();
            }
        }

        public string Error2
        {
            get
            {
                return this.error2;
            }

            set
            {
                if (value == this.error2)
                {
                    return;
                }

                this.error2 = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.HasErrors));
                this.OnErrorsChanged(nameof(this.IntValue2));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == nameof(this.IntValue1))
            {
                return string.IsNullOrEmpty(this.error1)
                           ? null
                           : new[] { this.Error1 };
            }

            if (propertyName == nameof(this.IntValue2))
            {
                return string.IsNullOrEmpty(this.error2)
                           ? null
                           : new[] { this.Error2 };
            }

            return Enumerable.Empty<object>();
        }

        protected virtual void OnErrorsChanged(string propertyName = null)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}