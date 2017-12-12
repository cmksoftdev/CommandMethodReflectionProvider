using System.Reflection;
using System.Windows.Input;

namespace TeaTimer.Services
{
    public class CommandMethodReflectionProvider<T>
    {
        public ICommand Command => this.command ?? (this.command = new CommandHandlerWithParameter<string>(this.Execute));

        private ICommand command;
        private readonly T methodIncludedObject;

        public CommandMethodReflectionProvider(T o)
        {
            this.methodIncludedObject = o;
        }

        public void Execute(string methodName)
        {
            MethodInfo canCall = this.methodIncludedObject.GetType().GetMethod("Can" + methodName);
            MethodInfo methodToCall = this.methodIncludedObject.GetType().GetMethod(methodName);
            if (canCall == null || canCall.Invoke(this.methodIncludedObject, null) as bool? == true)
            {
                methodToCall?.Invoke(this.methodIncludedObject, null);
            }
        }
    }
}