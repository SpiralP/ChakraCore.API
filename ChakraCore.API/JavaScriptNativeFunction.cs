using System;
using System.Runtime.InteropServices;

namespace ChakraCore.API
{
  /// <summary>
  ///     A structure containing information about a native function callback.
  /// </summary>
  public struct JavaScriptNativeFunctionInfo
  {
    JavaScriptValue thisArg;
    JavaScriptValue newTargetArg;
    bool isConstructCall;
  }

  /// <summary>
  ///     A function callback.
  /// </summary>
  /// <param name="callee">
  ///     A function object that represents the function being invoked.
  /// </param>
  /// <param name="arguments">The arguments to the call.</param>
  /// <param name="argumentCount">The number of arguments.</param>
  /// <param name="info">Additional information about this function call.</param>
  /// <param name="callbackState">
  ///     The state passed to <c>JsCreateFunction</c>.
  /// </param>
  /// <returns>The result of the call, if any.</returns>
  // typedef _Ret_maybenull_ JsValueRef(CHAKRA_CALLBACK* JsEnhancedNativeFunction)(_In_ JsValueRef callee, _In_ JsValueRef* arguments, _In_ unsigned short argumentCount, _In_ JsNativeFunctionInfo* info, _In_opt_ void* callbackState);
  public delegate JavaScriptValue JavaScriptEnhancedNativeFunction(JavaScriptValue callee, JavaScriptValue[] arguments, ushort argumentCount, JavaScriptNativeFunctionInfo info, IntPtr callbackState);

  
  /// <summary>
  ///     A function callback.
  /// </summary>
  /// <param name="callee">
  ///     A function object that represents the function being invoked.
  /// </param>
  /// <param name="isConstructCall">Indicates whether this is a regular call or a 'new' call.</param>
  /// <param name="arguments">The arguments to the call.</param>
  /// <param name="argumentCount">The number of arguments.</param>
  /// <param name="callbackState">
  ///     The state passed to <c>JsCreateFunction</c>.
  /// </param>
  /// <returns>The result of the call, if any.</returns>
  public delegate JavaScriptValue JavaScriptNativeFunction(JavaScriptValue callee, [MarshalAs(UnmanagedType.U1)] bool isConstructCall, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData);
}
