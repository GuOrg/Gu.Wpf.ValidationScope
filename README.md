# Gu.Wpf.ValidationScope

[![Gitter](https://badges.gitter.im/JohanLarsson/Gu.Wpf.ValidationScope.svg)](https://gitter.im/JohanLarsson/Gu.Wpf.ValidationScope?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[nuget](https://www.nuget.org/packages/Gu.Wpf.ValidationScope)

## Sample:
```
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="Auto" />
        <RowDefinition Height="*" />
    </Grid.RowDefinitions>
    <Border BorderBrush="{Binding Path=(validationScope:Scope.HasErrors),
                                  Converter={local:BoolToBrushConverter},
                                  ElementName=Form}"
            BorderThickness="1">
        <Grid x:Name="Form"
                validationScope:Scope.ForInputTypes="{x:Static validationScope:InputTypeCollection.Default}">
                <!-- this is where we define our scope, we do so by telling the scope what types of control sto track -->
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto" />
                <RowDefinition Height="Auto" />
                <RowDefinition />
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="Auto" />
                <ColumnDefinition />
            </Grid.ColumnDefinitions>
            <TextBlock Grid.Row="0"
                       Grid.Column="0"
                       Text="SomeIntValue" />
            <TextBox Grid.Row="0"
                     Grid.Column="1"
                     Text="{Binding IntValue,
                                    UpdateSourceTrigger=PropertyChanged}" />
            <TextBlock Grid.Row="1"
                       Grid.Column="0"
                       Text="SameIntValue" />
            <TextBox Grid.Row="1"
                     Grid.Column="1"
                     Text="{Binding IntValue,
                     UpdateSourceTrigger=PropertyChanged}" />
        </Grid>
    </Border>

    <ItemsControl Grid.Row="1"
                  ItemsSource="{Binding Path=(validationScope:Scope.Errors),
                                        ElementName=Form}">
        <ItemsControl.ItemTemplate>
            <DataTemplate DataType="{x:Type ValidationError}">
                <TextBlock Foreground="Red"
                           Text="{Binding ErrorContent}" />
            </DataTemplate>
        </ItemsControl.ItemTemplate>
    </ItemsControl>
</Grid>
```
More samples in the demo project

Renders:

![ItemsSource2D render](http://i.imgur.com/VSFYqBD.gif)
