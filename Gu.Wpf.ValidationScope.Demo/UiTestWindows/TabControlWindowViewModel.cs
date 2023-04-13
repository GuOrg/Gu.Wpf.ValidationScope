namespace Gu.Wpf.ValidationScope.Demo;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public class TabControlWindowViewModel : INotifyPropertyChanged
{
    private int intValue1;
    private int intValue2;
    private int intValue3;
    private int intValue4;

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

    public int IntValue3
    {
        get => this.intValue3;
        set
        {
            if (value == this.intValue3)
            {
                return;
            }

            this.intValue3 = value;
            this.OnPropertyChanged();
        }
    }

    public int IntValue4
    {
        get => this.intValue4;
        set
        {
            if (value == this.intValue4)
            {
                return;
            }

            this.intValue4 = value;
            this.OnPropertyChanged();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
