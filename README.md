# CommandMethodReflectionProvider

The CommandMethodReflectionProvider is designed to make it easier to call methods from XAML in WPF MVVM pattern.
The method will be searched by the method name passed to the CommandMethodReflectionProvider by CommandParameter.
It's also possible to call methods using parameters.
For Example "MethodName,int=1,flo=2.4,,pro=Property" to call MethodName(1,2.4f,null,this.Property);.
To enable or disable calling of a method use a method CanMethodname and return a bool value. 

Usage in ViewModel:

...
private CommandMethodReflectionProvider CommandMethodReflectionProvider { get; }

public void ViewModel()

{

this.CommandMethodReflectionProvider = new CommandMethodReflectionProvider(this);

...

}


Usage in XAML:

...

\<Button Content="Button"

Command="{Binding CommandMethodReflectionProvider.Command}"

CommandParameter="Method"/\>...
