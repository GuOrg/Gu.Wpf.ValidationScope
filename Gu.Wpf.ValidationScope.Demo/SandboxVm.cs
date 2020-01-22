namespace Gu.Wpf.ValidationScope.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class SandboxVm : INotifyPropertyChanged
    {
        private int value1;
        private int value2;

        private int value3;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Value1
        {
            get => this.value1;

            set
            {
                if (value == this.value1)
                {
                    return;
                }

                this.value1 = value;
                this.OnPropertyChanged();
            }
        }

        public int Value2
        {
            get => this.value2;

            set
            {
                if (value == this.value2)
                {
                    return;
                }

                this.value2 = value;
                this.OnPropertyChanged();
            }
        }

        public int Value3
        {
            get => this.value3;

            set
            {
                if (value == this.value3)
                {
                    return;
                }

                this.value3 = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
