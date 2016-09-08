# Gu.Wpf.ValidationScope

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
[![Gitter](https://badges.gitter.im/JohanLarsson/Gu.Wpf.ValidationScope.svg)](https://gitter.im/JohanLarsson/Gu.Wpf.ValidationScope?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![NuGet](https://img.shields.io/nuget/v/Gu.Wpf.ValidationScope.svg)](https://www.nuget.org/packages/Gu.Wpf.ValidationScope/)
[![Build status](https://ci.appveyor.com/api/projects/status/omv9baijykp70dfr?svg=true)](https://ci.appveyor.com/project/JohanLarsson/gu-wpf-validationscope)

Library that provides functionalitdy for form validation in WPF.
It works by adding bindings to Validation.Errors for elements in a validation scope.
As bindings are somewhat expensive no bindings are added by default. The types to track errors for are specified using the `Scope.ForInputTypes`

The samples assumes an xml namespace alias `xmlns:validation="https://github.com/JohanLarsson/Gu.Wpf.ValidationScope"` is defined.

- [1. Sample:](#1-sample)
- [2. Scope](#2-scope)
  - [2.1. ForInputTypes](#21-forinputtypes)
    - [2.1.1. Sample: Defining a scope for textboxes.](#211-sample--defining-a-scope-for-textboxes)
    - [2.1.2. Sample: Defining a partial scope for textboxes.](#212-sample--defining-a-partial-scope-for-textboxes)
    - [2.1.3 Valid values](#213-valid-values)
      - [2.1.3.1 Typenames](#2131-typenames)
      - [2.1.3.2 Fully qualified Typenames](#2132-fully-qualified-typenames)
      - [2.1.3.3 IEnumebarble<T>](#2133-ienumebarble-t)
      - [2.1.3.4 InputTypeCollection.Default](#2134-inputtypecollectiondefault)
      - [2.1.3.5 InputTypes markupextension](#2135-inputtypes-markupextension)
  - [2.2. HasError](#22-haserror)
  - [2.3. Errors](#23-errors)
  - [2.4. Node](#24-node)
  - [2.5. ErrorEvent](#25-errorevent)
  - [2.6. ErrorsChangedEvent](#26-errorschangedevent)
  - [2.7. OneWayToSourceBindings](#27-onewaytosourcebindings)
- [3. InputTypeCollection](#3-inputtypecollection)
  - [3.1. Default](#31-default)
- [4. InputTypes markupextension](#4-inputtypes-markupextension)
- [5. Node](#5-node)
  - [5.1. HasError](#51-haserror)
  - [5.2. Errors](#52-errors)
  - [5.3. Children](#53-children)
  - [5.4 Node types](#54-node-types)
    - [5.4.1 InputNode](#541-inputnode)
    - [5.4.2 ScopeNode](#542-scopenode)
    - [5.4.3 ValidNode](#543-validnode)

# 1. Sample:
*BoolToBrushConverter is not included in the nuget. Check the demo project if interested.

```xaml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="Auto" />
        <RowDefinition Height="*" />
    </Grid.RowDefinitions>
    <Border BorderBrush="{Binding Path=(validation:Scope.HasError),
                                  Converter={local:BoolToBrushConverter WhenTrue=Red, WhenFalse=Black},
                                  ElementName=Form}"
            BorderThickness="1">
        <Grid x:Name="Form"
                validation:Scope.ForInputTypes="TextBox">
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
                       Text="IntValue1" />
            <TextBox Grid.Row="0"
                     Grid.Column="1"
                     Text="{Binding IntValue1,
                                    UpdateSourceTrigger=PropertyChanged}" />
            <TextBlock Grid.Row="1"
                       Grid.Column="0"
                       Text="IntValue2" />
            <TextBox Grid.Row="1"
                     Grid.Column="1"
                     Text="{Binding IntValue2,
                     UpdateSourceTrigger=PropertyChanged}" />
        </Grid>
    </Border>

    <ItemsControl Grid.Row="1"
                  ItemsSource="{Binding Path=(validation:Scope.Errors),
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

Renders:

![ItemsSource2D render](http://i.imgur.com/EkuWA9c.gif)

More samples in the demo project

# 2. Scope
## 2.1. ForInputTypes
By setting `ForInputTypes` we specify what type of elements to track in the validation scope. Most of the times only TextBoxes will be relevant.
The `ForInputTypes` inherits so setting it on one element sets it to all chidlren unless explicitly set to something else for a child.
Setting `validation:Scope.ForInputTypes="{x:Null}"` means that errors are not tracked for nodes below this element.
Setting `validation:Scope.ForInputTypes="Scope"` means that only errors from subscopes are tracked.
The default value is null meaning no scope is defined.

### 2.1.1. Sample: Defining a scope for textboxes.
```xaml
<Border validation:Scope.ForInputTypes="TextBox">
    <StackPanel>
        <!--The stackpanel will inherit the scope-->
        <TextBox Text="{Binding Value1}" />
        <TextBox Text="{Binding Value2}" />
        <ComboBox ItemsSource="{Binding Values}" Text="{Binding Value3}" />
        <!-- this combobox will not be tracked because the scope is only for textboxes and sliders--> 
    </StackPanel>
</Border>
```

### 2.1.2. Sample: Defining a partial scope for textboxes.
```xaml
<Border validation:Scope.ForInputTypes="Scope">
    <StackPanel validation:Scope.ForInputTypes="TextBox">
        <TextBox Text="{Binding Value1}" />
        <TextBox Text="{Binding Value2}" />
    </StackPanel>
	<StackPanel validation:Scope.ForInputTypes="{x:null}">
		<!-- No tracking of errors for these textboxes, due to ForInputTypes="{x:null}". -->
        <TextBox Text="{Binding Value1}" />
        <TextBox Text="{Binding Value2}" />
    </StackPanel>
</Border>
```

### 2.1.3 Valid values
Types must be elements deriving from `UIElement`
#### 2.1.3.1 Typenames
`<Border validation:Scope.ForInputTypes="TextBox, ComboBox" ...>`

#### 2.1.3.2 Fully qualified Typenames
`<Border validation:Scope.ForInputTypes="System.Windows.Controls.TextBox, ComboBox" ...>`

#### 2.1.3.3 IEnumebarble<T>
`<Border validation:Scope.ForInputTypes="{Binding ScopeTypes}" ...>`

#### 2.1.3.4 InputTypeCollection.Default
`<Border validation:Scope.ForInputTypes="{x:Static validation:InputTypeCollection.Default}" ...>`

#### 2.1.3.5 InputTypes markupextension
`<Border validation:Scope.ForInputTypes="{validation:InputTypes {x:Type TextBox}, {x:Type ComboBox}}" ...>`

## 2.2. HasError
A bool indicating if there is a validation error in the scope. Similar to `System.Controls.Validation.HasError`

```xaml
<Border BorderBrush="{Binding Path=(validation:Scope.HasError), 
                              Converter={local:BoolToBrushConverter WhenTrue=Red, WhenFalse=Transparent}, 
							  ElementName=Form}" 
        BorderThickness="1">
    <StackPanel x:Name="Form" validation:Scope.ForInputTypes="TextBox">
        <TextBox Text="{Binding Value1}" />
        <TextBox Text="{Binding Value2}" />
    </StackPanel>
</Border>
```

## 2.3. Errors
A `ReadOnlyObservableCollection<ValidationError>` with the errors in the scope. Similar to `System.Controls.Validation.Errors`

```xaml
<StackPanel>
    <StackPanel x:Name="Form" validation:Scope.ForInputTypes="TextBox">
        <TextBox Text="{Binding Value1}" />
        <TextBox Text="{Binding Value2}" />
    </StackPanel>
	<TextBlock Foreground="Red" Text="{Binding (validation:Scope.Errors).Count, ElementName=Form, StringFormat='Errors: {0}'}" />
</StackPanel>
```

## 2.4. Node
A `Node` in the tree that is the validation scope.

```xaml
<StackPanel>
    <StackPanel x:Name="Form" validation:Scope.ForInputTypes="TextBox">
        <TextBox Text="{Binding Value1}" />
        <TextBox Text="{Binding Value2}" />
    </StackPanel>
	<TextBlock Foreground="Red" Text="{Binding (validation:Scope.Node).Children.Count, ElementName=Form, StringFormat='Children: {0}'}" />
</StackPanel>
```

## 2.5. ErrorEvent
An event that notifies when errors are added and removed. Similar to `System.Windows.Controls.Validation.ErrorEvent`
Does not require bindings to have `NotifyOnValidationError=True`

```xaml
<StackPanel>
    <StackPanel validation:Scope.ForInputTypes="TextBox"
				validation:Scope.Error="OnValidationError">
        <TextBox Text="{Binding Value1}" />
        <TextBox Text="{Binding Value2}" />
    </StackPanel>
</StackPanel>
```

## 2.6. ErrorsChangedEvent
An event that notifies when errors are added and removed. The `ErrorEvent` notifies about each add and remove while `ErrorsChangedEvent`notifies once with a all added and removed events.
Does not require bindings to have `NotifyOnValidationError=True`

```xaml
<StackPanel>
    <StackPanel validation:Scope.ForInputTypes="TextBox"
				validation:Scope.Error="OnValidationError">
        <TextBox Text="{Binding Value1}" />
        <TextBox Text="{Binding Value2}" />
    </StackPanel>
</StackPanel>
```

## 2.7. OneWayToSourceBindings
WPF does not allow binding readonly dependency properties even with `Mode=OneWayToSource`.
As a workaround for this OneWayToSourceBindings can be used like this:

```xaml
<StackPanel x:Name="Form" validation:Scope.ForInputTypes="TextBox">
    <validation:Scope.OneWayToSourceBindings>
        <validation:OneWayToSourceBindings Errors="{Binding Errors}"
                                           HasError="{Binding HasError}"
                                           Node="{Binding Node}" />
    </validation:Scope.OneWayToSourceBindings>
    <TextBox Text="{Binding Value1}" />
    <TextBox Text="{Binding Value2}" />
</StackPanel>
```

# 3. InputTypeCollection
## 3.1. Default
Contains the following types `{ typeof(Scope), typeof(TextBoxBase), typeof(Selector), typeof(ToggleButton), typeof(Slider) }`
And should be enough for most scenarios when you don't have third party controls for example a third party textbox that does not derive from `TextBoxBase`

# 4. InputTypes markupextension
Exposed for convenience to create list of types in xaml.

`<Border validation:Scope.ForInputTypes="{validation:InputTypes {x:Type TextBox}, {x:Type ComboBox}}">`

# 5. Node
The validation scope is a tree of nodes.
## 5.1. HasError
A bool indicating if there is a validation error in the scope. Similar to `System.Controls.Validation.HasError`

## 5.2. Errors
A `ReadOnlyObservableCollection<ValidationError>` with the errors in the scope. Similar to `System.Controls.Validation.Errors`

## 5.3. Children
A `ReadOnlyObservableCollection<ErrorNode>` with the child nodes which have errors in the scope.

## 5.4 Node types

### 5.4.1 InputNode
This node type is used for elements for which we track errors.

### 5.4.2 ScopeNode
This node type is used for elements which has subnodes with errors.
This node type does not listen to validation errors for its source element.

### 5.4.3 ValidNode
This node type is used for elements which has no errors or are not in a scope.
This is immutable and a single instance is used for all.

