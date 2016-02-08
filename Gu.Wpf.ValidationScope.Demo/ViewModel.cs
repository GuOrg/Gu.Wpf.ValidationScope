namespace Gu.Wpf.ValidationScope.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private int intValue;

        private string stringValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public int IntValue
        {
            get
            {
                return this.intValue;
            }
            set
            {
                if (value == this.intValue)
                {
                    return;
                }
                this.intValue = value;
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
