using System;
using System.Reflection;
using System.Windows.Input;

namespace CMK.Services
{
    public class CommandMethodReflectionProvider
    {
        public ICommand Command => this.command ?? (this.command = new CommandHandlerWithParameter<string>(this.Execute));

        private ICommand command;
        private readonly object methodIncludedObject;
        Action buttonNotFoundAction;

        public CommandMethodReflectionProvider(object o)
        {
            this.methodIncludedObject = o;
        }

        public CommandMethodReflectionProvider(object o, Action buttonNotFoundAction)
        {
            this.methodIncludedObject = o;
            this.buttonNotFoundAction = buttonNotFoundAction;
        }

        public void Execute(string parameterString)
        {
            var privateMethod = false;
            if (parameterString!=null&&parameterString.Length>2&&parameterString[0]=='*')
            {
                privateMethod = true;
            }
            var methodName = getMethodName(parameterString);
            var parameters = parseParameters(parameterString);
            MethodInfo canCall = this.methodIncludedObject.GetType().GetMethod("Can" + methodName);
            MethodInfo methodToCall;
            if (!privateMethod)
            {
                methodToCall = this.methodIncludedObject.GetType().GetMethod(methodName);
            }
            else
            {
                methodToCall = this.methodIncludedObject.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            }
            
            if (methodToCall == null && buttonNotFoundAction != null)
            {
                this.buttonNotFoundAction.Invoke();
            }
            else if (canCall == null || canCall.Invoke(this.methodIncludedObject, null) as bool? == true)
            {
                methodToCall?.Invoke(this.methodIncludedObject, parameters);
            }
        }

        private object[] parseParameters(string parameterString)
        {
            var i = parameterString.IndexOf(',') + 1;
            if (i <= 0)
                return null;
            var parameterCount = 1;
            for (var j = i; j < parameterString.Length; j++)
            {
                var c = parameterString[j];
                if (c == ',')
                    parameterCount++;
            }
            var parameters = new object[parameterCount];
            var k = i;
            for (var j = 0; j < parameterCount; j++)
            {
                string str = "";
                for (var l = k; l < parameterString.Length; l++)
                {
                    if (parameterString[l] == ',' && parameterString[l - 1] != '.')
                    {
                        k = l + 1;
                        break;
                    }
                    else
                        str += parameterString[l];
                }
                parameters[j] = createParameter(str);
            }
            return parameters;
        }

        private object createParameter(string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
                return null;
            if (parameter.Length < 5)
                return null;
            var parameterType = parameter.Remove(4, parameter.Length - 4);
            var parameterString = parameter.Remove(0, 4);
            if (parameterType == "int=")
            {
                return int.Parse(parameterString);
            }
            else if (parameterType == "flo=")
            {
                return float.Parse(parameterString);
            }
            else if (parameterType == "cha=")
            {
                return parameterString[0];
            }
            else if (parameterType == "dou=")
            {
                return double.Parse(parameterString);
            }
            else if (parameterType == "str=")
            {
                return parameterString;
            }
            else if (parameterType == "pro=")
            {
                return methodIncludedObject.GetType().GetProperty(parameterString)?.GetValue(methodIncludedObject, null);
            }
            else if (parameterType == "var=")
            {
                return methodIncludedObject.GetType().GetField(parameterString).GetValue(methodIncludedObject);
            }
            return null;
        }

        private string getMethodName(string parameterString)
        {
            var i = parameterString.IndexOf(',');

            string str = parameterString;
            if (parameterString != null && parameterString.Length > 2 && parameterString[0] == '*')
            {
                str = parameterString.Remove(0, 1);
                i--;
            }
            if (i <= -1)
                return str;
            return str.Remove(i, str.Length - i);
        }
    }
}