using System.Reflection;

namespace TicketManagement.Base.Helpers.Extensions;

public static class ReflectionExtensions
{
    public static void SetPrivatePropertyValue<T>(this object obj, string propName, T val)
    {
        // This method can be useful when you need to access and modify the value of a private property of an object
        // for which you don't have direct access to the property's setter.
        // It provides a way to bypass the accessibility restrictions and dynamically assign a new value to the private property.

        obj.GetType().GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(obj, val);

        #region How to use
        // var myObject = new MyClass("Initial Value");
        // myObject.PrintPrivateProperty(); // Output: Initial Value
        // myObject.SetPrivatePropertyValue("privateProperty", "New Value");
        // myObject.PrintPrivateProperty(); // Output: New Value
        #endregion
    }


    public static List<Type> GetTypesThatInheritsFromAnInterface(this Assembly assembly, Type interfaceType)
    {
        // This method can be helpful when you want to discover and retrieve all types within an assembly that inherit from a specific interface.
        // It allows you to dynamically explore the types at runtime, which can be useful for implementing features such as plugin systems,
        // dependency injection container registration, or discovering classes that implement specific behaviors or contracts.
        Type interfaceType2 = interfaceType;
        return (from t in assembly.GetTypes()
                where t.GetInterfaces()
                    .Any((i) => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType2)
                select t).ToList();
        #region  How to use
        // let's say we have several classes implementing this interface (IPlugin), and we want to retrieve all those classes from an assembly

        //Assembly currentAssembly = Assembly.GetExecutingAssembly();
        //Type interfaceType = typeof(IPlugin);

        //List<Type> pluginTypes = currentAssembly.GetTypesThatInheritsFromAnInterface(interfaceType);

        //foreach (var type in pluginTypes)
        //{
        //    var pluginInstance = Activator.CreateInstance(type) as IPlugin;
        //    pluginInstance.Execute();
        //}
        #endregion
    }
}