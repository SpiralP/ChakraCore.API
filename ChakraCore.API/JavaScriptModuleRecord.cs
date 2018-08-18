using System;
using System.Text;

namespace ChakraCore.API {
  /// <summary>
  ///     A reference to an ES module.
  /// </summary>
  /// <remarks>
  ///     A module record represents an ES module.
  /// </remarks>
  public struct JavaScriptModuleRecord {
    /// <summary>
    /// The reference.
    /// </summary>
    private readonly IntPtr reference;

    /// <summary>
    ///     Initializes a new instance of the <see cref="JavaScriptModuleRecord"/> struct.
    /// </summary>
    /// <param name="reference">The reference.</param>
    private JavaScriptModuleRecord(IntPtr reference) {
      this.reference = reference;
    }

    /// <summary>
    ///     Gets an invalid ID.
    /// </summary>
    public static JavaScriptModuleRecord Invalid {
      get { return new JavaScriptModuleRecord(IntPtr.Zero); }
    }


    public static JavaScriptModuleRecord Create(JavaScriptModuleRecord? parent, string name) {
      if (string.IsNullOrEmpty(name)) {
        name = Guid.NewGuid().ToString(); //root module has no name, give it a unique name
      }
      JavaScriptValue moduleName = JavaScriptValue.FromString(name);
      JavaScriptModuleRecord result;
      if (parent.HasValue) {
        Native.ThrowIfError(Native.JsInitializeModuleRecord(parent.Value, moduleName, out result));
      } else {
        Native.ThrowIfError(Native.JsInitializeModuleRecord(JavaScriptModuleRecord.Invalid, moduleName, out result));
      }

      return result;
    }

    public static void ParseScript(JavaScriptModuleRecord module, string script, JavaScriptSourceContext sourceContext) {
      var buffer = Encoding.UTF8.GetBytes(script);
      uint length = (uint) buffer.Length;
      Native.ThrowIfError(Native.JsParseModuleSource(module, sourceContext, buffer, length, JavaScriptParseModuleSourceFlags.JsParseModuleSourceFlags_DataIsUTF8, out JavaScriptValue parseException));
      if (parseException.IsValid) {
        string ex = parseException.ToString();
        throw new InvalidOperationException($"Parse script failed with error={ex}");
      }
    }

    public static JavaScriptValue RunModule(JavaScriptModuleRecord module) {
      Native.ThrowIfError(Native.JsModuleEvaluation(module, out JavaScriptValue result));
      return result;
    }

    public static void SetHostUrl(JavaScriptModuleRecord module, string url) {
      var value = JavaScriptValue.FromString(url);
      Native.ThrowIfError(Native.JsSetModuleHostInfo(module, JavascriptModuleHostInfoKind.JsModuleHostInfo_Url, value));
    }

    /// <summary>
    /// Set callback from chakraCore when the module resolution is finished, either successfuly or unsuccessfully.
    /// </summary>
    /// <param name="module"></param>
    /// <param name="callback"></param>
    public static void SetNotifyReady(JavaScriptModuleRecord module, NotifyModuleReadyCallback callback) {
      Native.ThrowIfError(Native.JsSetModuleNotifyModuleReadyCallback(module, JavascriptModuleHostInfoKind.JsModuleHostInfo_NotifyModuleReadyCallback, callback));
    }

    /// <summary>
    /// Set callback from chakracore to fetch dependent module.
    /// While this call will come back directly from ParseModuleSource, the additional
    /// task are treated as Promise that will be executed later.
    /// </summary>
    /// <param name="module"></param>
    /// <param name="callback"></param>
    public static void SetFetchModuleCallback(JavaScriptModuleRecord module, FetchImportedModuleCallBack callback) {
      Native.ThrowIfError(Native.JsFetchImportedModuleCallBack(module, JavascriptModuleHostInfoKind.JsModuleHostInfo_FetchImportedModuleCallback, callback));
    }

    /// <summary>
    /// Set callback from chakracore to fetch module dynamically during runtime.
    /// While this call will come back directly from runtime script or module code, the additional
    /// task can be scheduled asynchronously that executed later.
    /// </summary>
    /// <param name="module"></param>
    /// <param name="callback"></param>
    public static void SetFetchModuleScriptCallback(JavaScriptModuleRecord module, FetchImportedModuleFromScriptCallBack callback) {
      Native.ThrowIfError(Native.JsFetchImportedModuleFromScriptCallBack(module, JavascriptModuleHostInfoKind.JsModuleHostInfo_FetchImportedModuleFromScriptCallback, callback));
    }
  }


  /// <summary>
  ///     User implemented callback to fetch additional imported modules in ES modules.
  /// </summary>
  /// <remarks>
  ///     The callback is invoked on the current runtime execution thread, therefore execution is blocked until
  ///     the callback completes. Notify the host to fetch the dependent module. This is the "import" part
  ///     before HostResolveImportedModule in ES6 spec. This notifies the host that the referencing module has
  ///     the specified module dependency, and the host needs to retrieve the module back.
  ///
  ///     Callback should:
  ///     1. Check if the requested module has been requested before - if yes return the existing
  ///         module record
  ///     2. If no create and initialize a new module record with JsInitializeModuleRecord to
  ///         return and schedule a call to JsParseModuleSource for the new record.
  /// </remarks>
  /// <param name="referencingModule">The referencing module that is requesting the dependent module.</param>
  /// <param name="specifier">The specifier coming from the module source code.</param>
  /// <param name="dependentModuleRecord">The ModuleRecord of the dependent module. If the module was requested
  ///                                     before from other source, return the existing ModuleRecord, otherwise
  ///                                     return a newly created ModuleRecord.</param>
  /// <returns>
  ///     Returns a <c>JsNoError</c> if the operation succeeded an error code otherwise.
  /// </returns>
  public delegate JavaScriptErrorCode FetchImportedModuleCallBack(
    JavaScriptModuleRecord referencingModule,
    JavaScriptValue specifier,
    out JavaScriptModuleRecord dependentModuleRecord
  );

  /// <summary>
  ///     User implemented callback to fetch imported modules dynamically in scripts.
  /// </summary>
  /// <remarks>
  ///     The callback is invoked on the current runtime execution thread, therefore execution is blocked untill
  ///     the callback completes. Notify the host to fetch the dependent module. This is used for the dynamic
  ///     import() syntax.
  ///
  ///     Callback should:
  ///     1. Check if the requested module has been requested before - if yes return the existing module record
  ///     2. If no create and initialize a new module record with JsInitializeModuleRecord to return and
  ///         schedule a call to JsParseModuleSource for the new record.
  /// </remarks>
  /// <param name="sourceContext">The referencing script context that calls import()</param>
  /// <param name="specifier">The specifier provided to the import() call.</param>
  /// <param name="dependentModuleRecord">The ModuleRecord of the dependent module. If the module was requested
  ///                                     before from other source, return the existing ModuleRecord, otherwise
  ///                                     return a newly created ModuleRecord.</param>
  /// <returns>
  ///     Returns <c>JsNoError</c> if the operation succeeded or an error code otherwise.
  /// </returns>
  public delegate JavaScriptErrorCode FetchImportedModuleFromScriptCallBack(
    JavaScriptSourceContext sourceContext,
    JavaScriptValue specifier,
    out JavaScriptModuleRecord dependentModuleRecord
  );

  /// <summary>
  ///     User implemented callback to get notification when the module is ready.
  /// </summary>
  /// <remarks>
  ///     The callback is invoked on the current runtime execution thread, therefore execution is blocked until the
  ///     callback completes. This callback should schedule a call to JsEvaluateModule to run the module that has been loaded.
  /// </remarks>
  /// <param name="referencingModule">The referencing module that has finished running ModuleDeclarationInstantiation step.</param>
  /// <param name="exceptionVar">If nullptr, the module is successfully initialized and host should queue the execution job
  ///                            otherwise it's the exception object.</param>
  /// <returns>
  ///     Returns a JsErrorCode - note, the return value is ignored.
  /// </returns>
  public delegate JavaScriptErrorCode NotifyModuleReadyCallback(JavaScriptModuleRecord referencingModule, JavaScriptValue exceptionVar);
}
