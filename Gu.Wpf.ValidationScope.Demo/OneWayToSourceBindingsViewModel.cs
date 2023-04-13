namespace Gu.Wpf.ValidationScope.Demo;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

public class OneWayToSourceBindingsViewModel : INotifyPropertyChanged
{
    private int intValue;
    private bool hasError;
    private Node? node;
    private ReadOnlyObservableCollection<ValidationError>? errors;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int IntValue
    {
        get => this.intValue;

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

    public bool HasError
    {
        get => this.hasError;

        set
        {
            if (value == this.hasError)
            {
                return;
            }

            this.hasError = value;
            this.OnPropertyChanged();
        }
    }

    public ReadOnlyObservableCollection<ValidationError>? Errors
    {
        get => this.errors;

        set
        {
            if (Equals(value, this.errors))
            {
                return;
            }

            this.errors = value;
            this.OnPropertyChanged();
        }
    }

    public Node? Node
    {
        get => this.node;

        set
        {
            if (Equals(value, this.node))
            {
                return;
            }

            this.node = value;
            this.OnPropertyChanged();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
