#### 0.2.1
* FEATURE: Sign

#### 0.2.0
* FEATURE: Scope.ErrorEvent
* FEATURE: Scope.ErrorsChangedEvent
* FEATURE: Scope.NodeProperty, was Scope.ErrorsProperty
* FEATURE: OneWayToSourceBindings
* BREAKING CHANGE: Remove IErrorNode, use Node or ErrorNode instead
* BREAKING CHANGE: Rename Scope.Errors is now ReadOnlyObservableCollection<ValidationError>
* BREAKING CHANGE: Node is no longer INotifyCollectionChanged, use Errors instead.
* BREAKING CHANGE: Rename HasErrorProperty
* BREAKING CHANGE: Setting properties and raising events in different order now. Symmetric with System.Controls.Validation.
* BREAKING CHANGE: Removed old mess for binding readonly dependency properties.
* BREAKING CHANGE: Change xaml namespace & url.