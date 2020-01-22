namespace Gu.Wpf.ValidationScope.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ViewModel : INotifyPropertyChanged
    {
        private int intValue1;
        private string stringValue;

        private double doubleValue;

        private int intValue2;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int IntValue1
        {
            get => this.intValue1;

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
            get => this.intValue2;

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
            get => this.doubleValue;

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
            get => this.stringValue;

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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
