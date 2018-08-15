using System;
using System.Runtime.InteropServices;

namespace ChakraCore.API
{
  /// <summary>
  ///     A script context.
  /// </summary>
  /// <remarks>
  ///     <para>
  ///     Each script context contains its own global object, distinct from the global object in
  ///     other script contexts.
  ///     </para>
  ///     <para>
  ///     Many Chakra hosting APIs require an "active" script context, which can be set using
  ///     Current. Chakra hosting APIs that require a current context to be set will note
  ///     that explicitly in their documentation.
  ///     </para>
  /// </remarks>
  public struct JavaScriptContext
  {
    /// <summary>
    ///     The reference.
    /// </summary>
    private readonly IntPtr reference;

    public static bool operator ==(JavaScriptContext lhs, JavaScriptContext rhs)
    {
      return lhs.reference == rhs.reference;
    }

    public static bool operator !=(JavaScriptContext lhs, JavaScriptContext rhs)
    {
      return lhs.reference != rhs.reference;
    }
    public override bool Equals(object obj)
    {
      if (obj is JavaScriptContext)
      {
        return (this == (JavaScriptContext)obj);
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return reference.GetHashCode();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="JavaScriptContext"/> struct.
    /// </summary>
    /// <param name="reference">The reference.</param>
    internal JavaScriptContext(IntPtr reference)
    {
      this.reference = reference;
    }

    /// <summary>
    ///     Gets an invalid context.
    /// </summary>
    public static JavaScriptContext Invalid
    {
      get { return new JavaScriptContext(IntPtr.Zero); }
    }

    /// <summary>
    ///     Gets a value indicating whether the context is a valid context or not.
    /// </summary>
    public bool IsValid
    {
      get { return reference != IntPtr.Zero; }
    }


    /// <summary>
    ///     Adds a reference to a garbage collected object.
    /// </summary>
    /// <remarks>
    ///     This only needs to be called on <c>JsRef</c> handles that are not going to be stored
    ///     somewhere on the stack. Calling <c>JsAddRef</c> ensures that the object the <c>JsRef</c>
    ///     refers to will not be freed until <c>JsRelease</c> is called.
    /// </remarks>
    /// <returns>
    ///     The object's new reference count.
    /// </returns>
    public uint AddRef()
    {
      Native.ThrowIfError(Native.JsAddRef(this, out uint count));
      return count;
    }

    /// <summary>
    ///     Releases a reference to a garbage collected object.
    /// </summary>
    /// <remarks>
    ///     Removes a reference to a <c>JsRef</c> handle that was created by <c>JsAddRef</c>.
    /// </remarks>
    /// <returns>
    ///     The object's new reference count.
    /// </returns>
    public uint Release()
    {
      Native.ThrowIfError(Native.JsRelease(this, out uint count));
      return count;
    }

    /// <summary>
    ///     Creates a script context for running scripts.
    /// </summary>
    /// <remarks>
    ///     Each script context has its own global object that is isolated from all other script
    ///     contexts.
    /// </remarks>
    /// <param name="runtime">The runtime the script context is being created in.</param>
    /// <returns>
    ///     The created script context.
    /// </returns>
    public static JavaScriptContext CreateContext(JavaScriptRuntime runtime)
    {
      Native.ThrowIfError(Native.JsCreateContext(runtime, out JavaScriptContext newContext));
      return newContext;
    }

    /// <summary>
    ///     Gets or sets the current script context on the thread.
    /// </summary>
    /// <param name="context">The script context to make current.</param>
    /// <returns>
    ///     The current script context on the thread, null if there is no current script context.
    /// </returns>
    public static JavaScriptContext Current
    {
      get
      {
        Native.ThrowIfError(Native.JsGetCurrentContext(out JavaScriptContext reference));
        return reference;
      }

      set
      {
        Native.ThrowIfError(Native.JsSetCurrentContext(value));
      }
    }


    /// <summary>
    ///     Gets the script context that the object belongs to.
    /// </summary>
    /// <param name="obj">The object to get the context from.</param>
    /// <returns>
    ///     The context the object belongs to.
    /// </returns>
    public static JavaScriptContext GetContextOfObject(JavaScriptValue obj)
    {
      Native.ThrowIfError(Native.JsGetContextOfObject(obj, out JavaScriptContext context));
      return context;
    }

    /// <summary>
    ///     Gets the internal data set on JsrtContext.
    /// </summary>
    /// <returns>
    ///     The pointer to the data where data will be returned.
    /// </returns>
    public IntPtr GetContextData()
    {
      Native.ThrowIfError(Native.JsGetContextData(this, out IntPtr data));
      return data;
    }

    /// <summary>
    ///     Sets the internal data of JsrtContext.
    /// </summary>
    /// <param name="data">The pointer to the data to be set.</param>
    public void SetContextData(IntPtr data)
    {
      Native.ThrowIfError(Native.JsSetContextData(this, data));
    }

    /// <summary>
    ///     Gets the runtime that the context belongs to.
    /// </summary>
    /// <returns>
    ///     The runtime the context belongs to.
    /// </returns>
    public JavaScriptRuntime GetRuntime()
    {
      Native.ThrowIfError(Native.JsGetRuntime(this, out JavaScriptRuntime runtime));
      return runtime;
    }


    // Debug Functions

    /// <summary>
    ///     TTD API -- may change in future versions:
    ///     Creates a script context that takes the TTD mode from the log or explicitly is not in TTD mode (regular takes mode from currently active script).
    /// </summary>
    /// <param name="runtime">The runtime the script context is being created in.</param>
    /// <param name="useRuntimeTTDMode">Set to true to use runtime TTD mode false to explicitly be non-TTD context.</param>
    /// <param name="newContext">The created script context.</param>
    /// <returns>
    ///     The code <c>JsNoError</c> if the operation succeeded, a failure code otherwise.
    /// </returns>
    public static JavaScriptContext TTDCreateContext(
      JavaScriptRuntime runtimeHandle,
      bool useRuntimeTTDMode
    )
    {
      Native.ThrowIfError(Native.JsTTDCreateContext(runtimeHandle, useRuntimeTTDMode, out JavaScriptContext newContext));
      return newContext;
    }

    /// <summary>
    ///     TTD API -- may change in future versions:
    ///     Notify the time-travel system that a context has been identified as dead by the gc (and is being de-allocated).
    /// </summary>
    /// <param name="context">The script context that is now dead.</param>
    /// <returns>
    ///     The code <c>JsNoError</c> if the operation succeeded, a failure code otherwise.
    /// </returns>
    public void TTDNotifyContextDestroy()
    {
      Native.ThrowIfError(Native.JsTTDNotifyContextDestroy(this));
    }


    //

    public static string GetLastError()
    {
      if (Native.JsGetAndClearException(out JavaScriptValue exception) != JavaScriptErrorCode.NoError)
        return "failed to get and clear exception";

      if (
        Native.JsGetPropertyIdFromName("message", out JavaScriptPropertyId messageName) !=
        JavaScriptErrorCode.NoError
      )
        return "failed to get error message id";

      if (Native.JsGetProperty(exception, messageName, out JavaScriptValue messageValue)
          != JavaScriptErrorCode.NoError)
        return "failed to get error message";

      if (Native.JsStringToPointer(messageValue, out IntPtr message, out UIntPtr length) != JavaScriptErrorCode.NoError)
        return "failed to convert error message";

      return Marshal.PtrToStringUni(message);
    }


    /// <summary>
    ///     Determines whether the runtime of the current context is in an exception state.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     If a call into the runtime results in an exception (either as the result of running a
    ///     script or due to something like a conversion failure), the runtime is placed into an
    ///     "exception state." All calls into any context created by the runtime (except for the
    ///     exception APIs) will fail with <c>JsErrorInExceptionState</c> until the exception is
    ///     cleared.
    ///     </para>
    ///     <para>
    ///     If the runtime of the current context is in the exception state when a callback returns
    ///     into the engine, the engine will automatically rethrow the exception.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <returns>
    ///     Whether the runtime of the current context is in the exception state.
    /// </returns>
    public static bool HasException
    {
      get
      {
        Native.ThrowIfError(Native.JsHasException(out bool hasException));
        return hasException;
      }
    }

    /// <summary>
    ///     Returns the exception that caused the runtime of the current context to be in the
    ///     exception state and resets the exception state for that runtime.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     If the runtime of the current context is not in an exception state, this API will return
    ///     <c>JsErrorInvalidArgument</c>. If the runtime is disabled, this will return an exception
    ///     indicating that the script was terminated, but it will not clear the exception (the
    ///     exception will be cleared if the runtime is re-enabled using
    ///     <c>JsEnableRuntimeExecution</c>).
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <returns>
    ///     The exception for the runtime of the current context.
    /// </returns>
    public static JavaScriptValue GetAndClearException()
    {
      Native.ThrowIfError(Native.JsGetAndClearException(out JavaScriptValue exception));
      return exception;
    }

    /// <summary>
    ///     Sets the runtime of the current context to an exception state.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     If the runtime of the current context is already in an exception state, this API will
    ///     return <c>JsErrorInExceptionState</c>.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <param name="exception">
    ///     The JavaScript exception to set for the runtime of the current context.
    /// </param>
    public static void SetException(JavaScriptValue exception)
    {
      Native.ThrowIfError(Native.JsSetException(exception));
    }



    // the rest

    /// <summary>
    ///     Tells the runtime to do any idle processing it need to do.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     If idle processing has been enabled for the current runtime, calling <c>Idle</c> will
    ///     inform the current runtime that the host is idle and that the runtime can perform
    ///     memory cleanup tasks.
    ///     </para>
    ///     <para>
    ///     <c>Idle</c> will also return the number of system ticks until there will be more idle work
    ///     for the runtime to do. Calling <c>Idle</c> before this number of ticks has passed will do
    ///     no work.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <returns>
    ///     The next system tick when there will be more idle work to do. Returns the
    ///     maximum number of ticks if there no upcoming idle work to do.
    /// </returns>
    public static uint Idle()
    {
      Native.ThrowIfError(Native.JsIdle(out uint ticks));
      return ticks;
    }

    /// <summary>
    ///     Parses a script and returns a <c>Function</c> representing the script.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="script">The script to parse.</param>
    /// <param name="sourceContext">
    ///     A cookie identifying the script that can be used by script contexts that have debugging enabled.
    /// </param>
    /// <param name="sourceName">The location the script came from.</param>
    /// <returns>A <c>Function</c> representing the script code.</returns>
    public static JavaScriptValue ParseScript(string script, JavaScriptSourceContext sourceContext, string sourceName)
    {
      Native.ThrowIfError(Native.JsParseScript(script, sourceContext, sourceName, out JavaScriptValue result));
      return result;
    }

    /// <summary>
    ///     Parses a serialized script and returns a <c>Function</c> representing the script.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="script">The script to parse.</param>
    /// <param name="buffer">The serialized script.</param>
    /// <param name="sourceContext">
    ///     A cookie identifying the script that can be used by script contexts that have debugging enabled.
    /// </param>
    /// <param name="sourceName">The location the script came from.</param>
    /// <returns>A <c>Function</c> representing the script code.</returns>
    public static JavaScriptValue ParseScript(string script, byte[] buffer, JavaScriptSourceContext sourceContext, string sourceName)
    {
      Native.ThrowIfError(Native.JsParseSerializedScript(script, buffer, sourceContext, sourceName, out JavaScriptValue result));
      return result;
    }

    /// <summary>
    ///     Parses a script and returns a <c>Function</c> representing the script.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="script">The script to parse.</param>
    /// <returns>A <c>Function</c> representing the script code.</returns>
    public static JavaScriptValue ParseScript(string script)
    {
      return ParseScript(script, JavaScriptSourceContext.None, string.Empty);
    }

    /// <summary>
    ///     Parses a serialized script and returns a <c>Function</c> representing the script.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="script">The script to parse.</param>
    /// <param name="buffer">The serialized script.</param>
    /// <returns>A <c>Function</c> representing the script code.</returns>
    public static JavaScriptValue ParseScript(string script, byte[] buffer)
    {
      return ParseScript(script, buffer, JavaScriptSourceContext.None, string.Empty);
    }

    /// <summary>
    ///     Executes a script.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="script">The script to run.</param>
    /// <param name="sourceContext">
    ///     A cookie identifying the script that can be used by script contexts that have debugging enabled.
    /// </param>
    /// <param name="sourceName">The location the script came from.</param>
    /// <returns>The result of the script, if any.</returns>
    public static JavaScriptValue RunScript(string script, JavaScriptSourceContext sourceContext, string sourceName)
    {
      JavaScriptValue scriptValue = JavaScriptValue.FromString(script);
      JavaScriptValue name = JavaScriptValue.FromString(sourceName);
      Native.ThrowIfError(Native.JsRun(scriptValue, sourceContext, name, JavaScriptParseScriptAttributes.JsParseScriptAttributeNone, out JavaScriptValue result));
      return result;
    }

    /// <summary>
    ///     Runs a serialized script.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="script">The source code of the serialized script.</param>
    /// <param name="buffer">The serialized script.</param>
    /// <param name="sourceContext">
    ///     A cookie identifying the script that can be used by script contexts that have debugging enabled.
    /// </param>
    /// <param name="sourceName">The location the script came from.</param>
    /// <returns>The result of the script, if any.</returns>
    public static JavaScriptValue RunScript(string script, byte[] buffer, JavaScriptSourceContext sourceContext, string sourceName)
    {
      Native.ThrowIfError(Native.JsRunSerializedScript(script, buffer, sourceContext, sourceName, out JavaScriptValue result));
      return result;
    }

    /// <summary>
    ///     Executes a script.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="script">The script to run.</param>
    /// <returns>The result of the script, if any.</returns>
    public static JavaScriptValue RunScript(string script, JavaScriptSourceContext sourceContext)
    {
      return RunScript(script, sourceContext, string.Empty);
    }

    /// <summary>
    ///     Runs a serialized script.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="script">The source code of the serialized script.</param>
    /// <param name="buffer">The serialized script.</param>
    /// <returns>The result of the script, if any.</returns>
    public static JavaScriptValue RunScript(string script, byte[] buffer, JavaScriptSourceContext sourceContext)
    {
      return RunScript(script, buffer, sourceContext, string.Empty);
    }

    /// <summary>
    ///     Serializes a parsed script to a buffer than can be reused.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     SerializeScript parses a script and then stores the parsed form of the script in a
    ///     runtime-independent format. The serialized script then can be deserialized in any
    ///     runtime without requiring the script to be re-parsed.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <param name="script">The script to serialize.</param>
    /// <param name="buffer">The buffer to put the serialized script into. Can be null.</param>
    /// <returns>
    ///     The size of the buffer, in bytes, required to hold the serialized script.
    /// </returns>
    public static ulong SerializeScript(string script, byte[] buffer)
    {
      var bufferSize = (ulong)buffer.Length;
      Native.ThrowIfError(Native.JsSerializeScript(script, buffer, ref bufferSize));
      return bufferSize;
    }

    /// <summary>
    ///     A scope automatically sets a context to current and resets the original context
    ///     when disposed.
    /// </summary>
    public struct Scope : IDisposable
    {
      /// <summary>
      ///     The previous context.
      /// </summary>
      private readonly JavaScriptContext previousContext;

      /// <summary>
      ///     Whether the structure has been disposed.
      /// </summary>
      private bool disposed;

      /// <summary>
      ///     Initializes a new instance of the <see cref="Scope"/> struct.
      /// </summary>
      /// <param name="context">The context to create the scope for.</param>
      public Scope(JavaScriptContext context)
      {
        disposed = false;
        previousContext = Current;
        Current = context;
      }

      /// <summary>
      ///     Disposes the scope and sets the previous context to current.
      /// </summary>
      public void Dispose()
      {
        if (disposed)
        {
          return;
        }

        Current = previousContext;
        disposed = true;
      }
    }
  }
}