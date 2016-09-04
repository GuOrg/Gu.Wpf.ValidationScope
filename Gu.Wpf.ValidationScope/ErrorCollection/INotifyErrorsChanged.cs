namespace Gu.Wpf.ValidationScope
{
    using System;

    internal interface INotifyErrorsChanged
    {
        event EventHandler<ErrorsChangedEventArgs> ErrorsChanged;
    }
}