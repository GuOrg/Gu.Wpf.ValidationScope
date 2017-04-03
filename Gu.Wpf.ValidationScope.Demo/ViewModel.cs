namespace Gu.Wpf.ValidationScope.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private int intValue1;
        private string stringValue;

        private double doubleValue;

        private int intValue2;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public double DoubleValue
        {
            get
            {
                return this.doubleValue;
            }

            set
            {
                if (value.Equals(this.doubleValue))
                {
                    return;
                }

                this.doubleValue = value;
                this.OnPropertyChanged();
            }
        }

        public string StringValue
        {
            get
            {
                return this.stringValue;
            }

            set
            {
                if (value == this.stringValue)
                {
                    return;
                }

                this.stringValue = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
