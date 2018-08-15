using System;

namespace ChakraCore.API
{
  class Program
  {
    static void Main(string[] args)
    {
      var runtime = JavaScriptRuntime.Create();
      var context = runtime.CreateContext();
      Console.WriteLine("runtime: " + runtime);
      Console.WriteLine("context: " + context);

      JavaScriptContext.Current = context;

      JavaScriptValue.GlobalObject.SetProperty(
        JavaScriptPropertyId.FromString("log"),
        JavaScriptValue.CreateFunction("log", log)
      );

      JavaScriptContext.RunScript(
        @"
          log('hello!');
        ",
        JavaScriptSourceContext.None,
        "a script"
      );

      JavaScriptContext.Current = JavaScriptContext.Invalid;
    }

    private static JavaScriptValue log(
      JavaScriptValue callee,
      bool isConstructCall,
      JavaScriptValue[] arguments,
      ushort argumentCount,
      IntPtr callbackData
    )
    {
      Console.WriteLine("log: " + arguments[1].ToString());
      return JavaScriptValue.Undefined;
    }
  }
}
