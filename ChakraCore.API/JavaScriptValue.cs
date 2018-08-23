using System;
using System.Text;

namespace ChakraCore.API {
  /// <summary>
  ///     A JavaScript value.
  /// </summary>
  /// <remarks>
  ///     A JavaScript value is one of the following types of values: Undefined, Null, Boolean,
  ///     String, Number, or Object.
  /// </remarks>
  public struct JavaScriptValue {
    /// <summary>
    /// The reference.
    /// </summary>
    private readonly IntPtr reference;

    /// <summary>
    ///     Initializes a new instance of the <see cref="JavaScriptValue"/> struct.
    /// </summary>
    /// <param name="reference">The reference.</param>
    private JavaScriptValue(IntPtr reference) {
      this.reference = reference;
    }

    /// <summary>
    ///     Gets an invalid value.
    /// </summary>
    public static JavaScriptValue Invalid {
      get { return new JavaScriptValue(IntPtr.Zero); }
    }

    /// <summary>
    ///     Gets a value indicating whether the value is a valid or not.
    /// </summary>
    public bool IsValid {
      get { return reference != IntPtr.Zero; }
    }

    public static bool operator ==(JavaScriptValue lhs, JavaScriptValue rhs) {
      return lhs.reference == rhs.reference;
    }

    public static bool operator !=(JavaScriptValue lhs, JavaScriptValue rhs) {
      return lhs.reference != rhs.reference;
    }
    public override bool Equals(object obj) {
      if (obj is JavaScriptValue) {
        return (this == (JavaScriptValue) obj);
      } else {
        return false;
      }
    }

    public override int GetHashCode() {
      return reference.GetHashCode();
    }


    /// <summary>
    ///     Gets the value of <c>undefined</c> in the current script context.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public static JavaScriptValue Undefined {
      get {
        JavaScriptValue value;
        Native.ThrowIfError(Native.JsGetUndefinedValue(out value));
        return value;
      }
    }

    /// <summary>
    /// map internal memory to chakracore engine
    /// </summary>
    /// <param name="data">data block</param>
    /// <param name="byteLength">length of data</param>
    /// <param name="finalizeCallback">callback for object disposed in js engine</param>
    /// <param name="callbackState">reference, can be null</param>
    /// <returns></returns>
    public static JavaScriptValue CreateExternalArrayBuffer(IntPtr data, uint byteLength, JavaScriptFinalizeCallback finalizeCallback, IntPtr callbackState) {
      JavaScriptValue result;
      Native.ThrowIfError(Native.JsCreateExternalArrayBuffer(data, byteLength, finalizeCallback, callbackState, out result));
      return result;
    }


    public static JavaScriptValue CreateArrayBuffer(uint byteLength) {
      JavaScriptValue result;
      Native.ThrowIfError(Native.JsCreateArrayBuffer(byteLength, out result));
      return result;
    }


    public static JavaScriptValue CreateTypedArray(JavaScriptTypedArrayType arrayType, JavaScriptValue arrayBuffer, uint byteOffset, uint elementLength) {
      JavaScriptValue result;
      Native.ThrowIfError(Native.JsCreateTypedArray(arrayType, arrayBuffer, byteOffset, elementLength, out result));
      return result;
    }

    public static IntPtr GetArrayBufferStorage(JavaScriptValue value, out uint bufferSize) {
      IntPtr data;
      uint bufferLength;
      Native.ThrowIfError(Native.JsGetArrayBufferStorage(value, out data, out bufferLength));
      bufferSize = bufferLength;
      return data;
    }

    public static JavaScriptValue CreateDataView(JavaScriptValue arrayBuffer, uint byteOffset, uint byteOffsetLength) {
      JavaScriptValue result;
      Native.ThrowIfError(Native.JsCreateDataView(arrayBuffer, byteOffset, byteOffsetLength, out result));
      return result;
    }

    public static void GetDataViewStorage(JavaScriptValue dataView, out IntPtr data, out uint bufferLength) {
      IntPtr _data;
      uint _bufferLength;
      Native.ThrowIfError(Native.JsGetDataViewStorage(dataView, out _data, out _bufferLength));
      data = _data;
      bufferLength = _bufferLength;
    }


    public static void GetTypedArrayStorage(JavaScriptValue typedArray, out IntPtr data, out uint bufferLength, out JavaScriptTypedArrayType arrayType, out int elementSize) {
      IntPtr _data;
      uint _bufferLength;
      JavaScriptTypedArrayType _arrayType;
      int _elementSize;
      Native.ThrowIfError(Native.JsGetTypedArrayStorage(
        typedArray,
        out _data,
        out _bufferLength,
        out _arrayType,
        out _elementSize
      ));
      data = _data;
      bufferLength = _bufferLength;
      arrayType = _arrayType;
      elementSize = _elementSize;
    }

    /// <summary>
    ///     Gets the value of <c>null</c> in the current script context.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public static JavaScriptValue Null {
      get {
        JavaScriptValue value;
        Native.ThrowIfError(Native.JsGetNullValue(out value));
        return value;
      }
    }

    /// <summary>
    ///     Gets the value of <c>true</c> in the current script context.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public static JavaScriptValue True {
      get {
        JavaScriptValue value;
        Native.ThrowIfError(Native.JsGetTrueValue(out value));
        return value;
      }
    }

    /// <summary>
    ///     Gets the value of <c>false</c> in the current script context.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public static JavaScriptValue False {
      get {
        JavaScriptValue value;
        Native.ThrowIfError(Native.JsGetFalseValue(out value));
        return value;
      }
    }

    /// <summary>
    ///     Gets the global object in the current script context.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public static JavaScriptValue GlobalObject {
      get {
        JavaScriptValue value;
        Native.ThrowIfError(
          Native.JsGetGlobalObject(
            out value
          )
        );
        return value;
      }
    }

    /// <summary>
    ///     Gets the JavaScript type of the value.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>The type of the value.</returns>
    public JavaScriptValueType ValueType {
      get {
        JavaScriptValueType type;
        Native.ThrowIfError(
          Native.JsGetValueType(
            this,
            out type
          )
        );
        return type;
      }
    }

    /// <summary>
    ///     Gets the length of a <c>String</c> value.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>The length of the string.</returns>
    public int StringLength {
      get {
        int length;
        Native.ThrowIfError(
          Native.JsGetStringLength(
            this,
            out length
          )
        );
        return length;
      }
    }

    /// <summary>
    ///     Gets or sets the prototype of an object.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public JavaScriptValue Prototype {
      get {
        JavaScriptValue prototypeReference;
        Native.ThrowIfError(
          Native.JsGetPrototype(
            this,
            out prototypeReference
          )
        );
        return prototypeReference;
      }

      set {
        Native.ThrowIfError(
          Native.JsSetPrototype(
            this,
            value
          )
        );
      }
    }

    /// <summary>
    ///     Gets a value indicating whether an object is extensible or not.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public bool IsExtensionAllowed {
      get {
        bool allowed;
        Native.ThrowIfError(Native.JsGetExtensionAllowed(this, out allowed));
        return allowed;
      }
    }

    /// <summary>
    ///     Gets a value indicating whether an object is an external object.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public bool HasExternalData {
      get {
        bool hasExternalData;
        Native.ThrowIfError(Native.JsHasExternalData(this, out hasExternalData));
        return hasExternalData;
      }
    }

    /// <summary>
    ///     Gets or sets the data in an external object.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public IntPtr ExternalData {
      get {
        IntPtr data;
        Native.ThrowIfError(Native.JsGetExternalData(this, out data));
        return data;
      }

      set {
        Native.ThrowIfError(Native.JsSetExternalData(this, value));
      }
    }

    /// <summary>
    ///     Creates a <c>Boolean</c> value from a <c>bool</c> value.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="value">The value to be converted.</param>
    /// <returns>The converted value.</returns>
    public static JavaScriptValue FromBoolean(bool value) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsBoolToBoolean(value, out reference));
      return reference;
    }

    /// <summary>
    ///     Creates a <c>Number</c> value from a <c>double</c> value.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="value">The value to be converted.</param>
    /// <returns>The new <c>Number</c> value.</returns>
    public static JavaScriptValue FromDouble(double value) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsDoubleToNumber(value, out reference));
      return reference;
    }

    /// <summary>
    ///     Creates a <c>Number</c> value from a <c>int</c> value.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="value">The value to be converted.</param>
    /// <returns>The new <c>Number</c> value.</returns>
    public static JavaScriptValue FromInt32(int value) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsIntToNumber(value, out reference));
      return reference;
    }

    /// <summary>
    ///     Creates a <c>String</c> value from a string pointer.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="value">The string  to convert to a <c>String</c> value.</param>
    /// <returns>The new <c>String</c> value.</returns>
    public static JavaScriptValue FromString(string value) {
      JavaScriptValue reference;
      Native.ThrowIfError(
        Native.JsCreateStringUtf16(
          value,
          (UIntPtr) value.Length,
          out reference
        )
      );
      return reference;
    }

    /// <summary>
    ///     Creates a new <c>Object</c>.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>The new <c>Object</c>.</returns>
    public static JavaScriptValue CreateObject() {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsCreateObject(out reference));
      return reference;
    }

    /// <summary>
    ///     Creates a new <c>Object</c> that stores some external data.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="data">External data that the object will represent. May be null.</param>
    /// <param name="finalizer">
    ///     A callback for when the object is finalized. May be null.
    /// </param>
    /// <returns>The new <c>Object</c>.</returns>
    public static JavaScriptValue CreateExternalObject(IntPtr data, JavaScriptFinalizeCallback finalizer) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsCreateExternalObject(data, finalizer, out reference));
      return reference;
    }

    /// <summary>
    ///     Creates a new object (with prototype) that stores some external data.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="data">External data that the object will represent. May be null.</param>
    /// <param name="finalizeCallback">
    ///     A callback for when the object is finalized. May be null.
    /// </param>
    /// <param name="prototype">Prototype object or nullptr.</param>
    /// <returns>
    ///     The new object.
    /// </returns>
    public static JavaScriptValue CreateExternalObjectWithPrototype(
      IntPtr data,
      JavaScriptFinalizeCallback finalizeCallback,
      JavaScriptValue prototype
    ) {
      JavaScriptValue obj;
      Native.ThrowIfError(
        Native.JsCreateExternalObjectWithPrototype(data, finalizeCallback, prototype, out obj)
      );
      return obj;
    }

    /// <summary>
    ///     Creates a new JavaScript function.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="function">The method to call when the function is invoked.</param>
    /// <returns>The new function object.</returns>
    public static JavaScriptValue CreateFunction(JavaScriptNativeFunction function) {
      JavaScriptValue reference;
      Native.ThrowIfError(
        Native.JsCreateFunction(
          function,
          IntPtr.Zero,
          out reference
        )
      );
      return reference;
    }

    /// <summary>
    ///     Creates a new JavaScript function.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="function">The method to call when the function is invoked.</param>
    /// <param name="name">The name of the function.</param>
    /// <returns>The new function object.</returns>
    public static JavaScriptValue CreateFunction(string name, JavaScriptNativeFunction function) {
      JavaScriptValue reference;
      Native.ThrowIfError(
        Native.JsCreateNamedFunction(
          JavaScriptValue.FromString(name),
          function,
          IntPtr.Zero,
          out reference
        )
      );
      return reference;
    }

    /// <summary>
    ///     Creates a new JavaScript function.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="function">The method to call when the function is invoked.</param>
    /// <param name="callbackData">Data to be provided to all function callbacks.</param>
    /// <returns>The new function object.</returns>
    public static JavaScriptValue CreateFunction(JavaScriptNativeFunction function, IntPtr callbackData) {
      JavaScriptValue reference;
      Native.ThrowIfError(
        Native.JsCreateFunction(
          function,
          callbackData,
          out reference
        )
      );
      return reference;
    }

    /// <summary>
    ///     Creates a new JavaScript function.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="function">The method to call when the function is invoked.</param>
    /// <param name="callbackData">Data to be provided to all function callbacks.</param>
    /// <returns>The new function object.</returns>
    public static JavaScriptValue CreateFunction(string name, JavaScriptNativeFunction function, IntPtr callbackData) {
      JavaScriptValue reference;
      Native.ThrowIfError(
        Native.JsCreateNamedFunction(
          JavaScriptValue.FromString(name),
          function,
          callbackData,
          out reference
        )
      );
      return reference;
    }

    /// <summary>
    ///     Creates a JavaScript array object.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="length">The initial length of the array.</param>
    /// <returns>The new array object.</returns>
    public static JavaScriptValue CreateArray(uint length) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsCreateArray(length, out reference));
      return reference;
    }

    /// <summary>
    ///     Creates a new JavaScript error object
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="message">Message for the error object.</param>
    /// <returns>The new error object.</returns>
    public static JavaScriptValue CreateError(JavaScriptValue message) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsCreateError(message, out reference));
      return reference;
    }

    public static JavaScriptValue CreateError(string message) {
      return CreateError(JavaScriptValue.FromString(message));
    }

    /// <summary>
    ///     Creates a new JavaScript RangeError error object
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="message">Message for the error object.</param>
    /// <returns>The new error object.</returns>
    public static JavaScriptValue CreateRangeError(JavaScriptValue message) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsCreateRangeError(message, out reference));
      return reference;
    }

    public static JavaScriptValue CreateRangeError(string message) {
      return CreateRangeError(JavaScriptValue.FromString(message));
    }

    /// <summary>
    ///     Creates a new JavaScript ReferenceError error object
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="message">Message for the error object.</param>
    /// <returns>The new error object.</returns>
    public static JavaScriptValue CreateReferenceError(JavaScriptValue message) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsCreateReferenceError(message, out reference));
      return reference;
    }

    public static JavaScriptValue CreateReferenceError(string message) {
      return CreateReferenceError(JavaScriptValue.FromString(message));
    }

    /// <summary>
    ///     Creates a new JavaScript SyntaxError error object
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="message">Message for the error object.</param>
    /// <returns>The new error object.</returns>
    public static JavaScriptValue CreateSyntaxError(JavaScriptValue message) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsCreateSyntaxError(message, out reference));
      return reference;
    }

    public static JavaScriptValue CreateSyntaxError(string message) {
      return CreateSyntaxError(JavaScriptValue.FromString(message));
    }

    /// <summary>
    ///     Creates a new JavaScript TypeError error object
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="message">Message for the error object.</param>
    /// <returns>The new error object.</returns>
    public static JavaScriptValue CreateTypeError(JavaScriptValue message) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsCreateTypeError(message, out reference));
      return reference;
    }

    public static JavaScriptValue CreateTypeError(string message) {
      return CreateTypeError(JavaScriptValue.FromString(message));
    }

    /// <summary>
    ///     Creates a new JavaScript URIError error object
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="message">Message for the error object.</param>
    /// <returns>The new error object.</returns>
    public static JavaScriptValue CreateUriError(JavaScriptValue message) {
      JavaScriptValue reference;
      Native.ThrowIfError(Native.JsCreateURIError(message, out reference));
      return reference;
    }

    public static JavaScriptValue CreateUriError(string message) {
      return CreateUriError(JavaScriptValue.FromString(message));
    }

    /// <summary>
    ///     Adds a reference to the object.
    /// </summary>
    /// <remarks>
    ///     This only needs to be called on objects that are not going to be stored somewhere on
    ///     the stack. Calling AddRef ensures that the JavaScript object the value refers to will not be freed
    ///     until Release is called
    /// </remarks>
    /// <returns>The object's new reference count.</returns>
    public uint AddRef() {
      uint count;
      Native.ThrowIfError(Native.JsAddRef(this, out count));
      return count;
    }

    /// <summary>
    ///     Releases a reference to the object.
    /// </summary>
    /// <remarks>
    ///     Removes a reference that was created by AddRef.
    /// </remarks>
    /// <returns>The object's new reference count.</returns>
    public uint Release() {
      uint count;
      Native.ThrowIfError(Native.JsRelease(this, out count));
      return count;
    }

    /// <summary>
    ///     Retrieves the <c>bool</c> value of a <c>Boolean</c> value.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>The converted value.</returns>
    public bool ToBoolean() {
      bool value;
      Native.ThrowIfError(Native.JsBooleanToBool(this, out value));
      return value;
    }

    /// <summary>
    ///     Retrieves the <c>double</c> value of a <c>Number</c> value.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     This function retrieves the value of a Number value. It will fail with
    ///     <c>InvalidArgument</c> if the type of the value is not <c>Number</c>.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <returns>The <c>double</c> value.</returns>
    public double ToDouble() {
      double value;
      Native.ThrowIfError(Native.JsNumberToDouble(this, out value));
      return value;
    }

    /// <summary>
    ///     Retrieves the <c>int</c> value of a <c>Number</c> value.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     This function retrieves the value of a Number value. It will fail with
    ///     <c>InvalidArgument</c> if the type of the value is not <c>Number</c>.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <returns>The <c>int</c> value.</returns>
    public int ToInt32() {
      int value;
      Native.ThrowIfError(Native.JsNumberToInt(this, out value));
      return value;
    }

    /// <summary>
    ///     Retrieves the string pointer of a <c>String</c> value.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     This function retrieves the string pointer of a <c>String</c> value. It will fail with
    ///     <c>InvalidArgument</c> if the type of the value is not <c>String</c>.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <returns>The string.</returns>
    public new string ToString() {
      int bufferLength;
      Native.ThrowIfError(
        Native.JsGetStringLength(
          this,
          out bufferLength
        )
      ); // using JsCopyStringUtf16 to get needed size needs a max length number

      StringBuilder sb = new StringBuilder(bufferLength);
      UIntPtr written;
      Native.ThrowIfError(
        Native.JsCopyStringUtf16(
          this,
          0,
          bufferLength,
          sb,
          out written
        )
      );

      return sb.ToString(0, (int) written); // works because written is "characters written"
    }

    /// <summary>
    ///     Converts the value to <c>Boolean</c> using regular JavaScript semantics.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>The converted value.</returns>
    public JavaScriptValue ConvertToBoolean() {
      JavaScriptValue booleanReference;
      Native.ThrowIfError(Native.JsConvertValueToBoolean(this, out booleanReference));
      return booleanReference;
    }

    /// <summary>
    ///     Converts the value to <c>Number</c> using regular JavaScript semantics.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>The converted value.</returns>
    public JavaScriptValue ConvertToNumber() {
      JavaScriptValue numberReference;
      Native.ThrowIfError(Native.JsConvertValueToNumber(this, out numberReference));
      return numberReference;
    }

    /// <summary>
    ///     Converts the value to <c>String</c> using regular JavaScript semantics.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>The converted value.</returns>
    public JavaScriptValue ConvertToString() {
      JavaScriptValue stringReference;
      Native.ThrowIfError(Native.JsConvertValueToString(this, out stringReference));
      return stringReference;
    }

    /// <summary>
    ///     Converts the value to <c>Object</c> using regular JavaScript semantics.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>The converted value.</returns>
    public JavaScriptValue ConvertToObject() {
      JavaScriptValue objectReference;
      Native.ThrowIfError(Native.JsConvertValueToObject(this, out objectReference));
      return objectReference;
    }

    /// <summary>
    ///     Sets an object to not be extensible.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    public void PreventExtension() {
      Native.ThrowIfError(Native.JsPreventExtension(this));
    }

    /// <summary>
    ///     Gets a property descriptor for an object's own property.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="propertyId">The ID of the property.</param>
    /// <returns>The property descriptor.</returns>
    public JavaScriptValue GetOwnPropertyDescriptor(JavaScriptPropertyId propertyId) {
      JavaScriptValue descriptorReference;
      Native.ThrowIfError(Native.JsGetOwnPropertyDescriptor(this, propertyId, out descriptorReference));
      return descriptorReference;
    }

    /// <summary>
    ///     Gets the list of all properties on the object.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>An array of property names.</returns>
    public JavaScriptValue GetOwnPropertyNames() {
      JavaScriptValue propertyNamesReference;
      Native.ThrowIfError(Native.JsGetOwnPropertyNames(this, out propertyNamesReference));
      return propertyNamesReference;
    }

    /// <summary>
    ///     Determines whether an object has a property.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="propertyId">The ID of the property.</param>
    /// <returns>Whether the object (or a prototype) has the property.</returns>
    public bool HasProperty(JavaScriptPropertyId propertyId) {
      bool hasProperty;
      Native.ThrowIfError(Native.JsHasProperty(this, propertyId, out hasProperty));
      return hasProperty;
    }

    /// <summary>
    ///     Gets an object's property.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="id">The ID of the property.</param>
    /// <returns>The value of the property.</returns>
    public JavaScriptValue GetProperty(JavaScriptPropertyId id) {
      JavaScriptValue propertyReference;
      Native.ThrowIfError(Native.JsGetProperty(this, id, out propertyReference));
      return propertyReference;
    }
    public JavaScriptValue GetProperty(string id) {
      return this.GetProperty(JavaScriptPropertyId.FromString(id));
    }

    /// <summary>
    ///     Sets an object's property.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="id">The ID of the property.</param>
    /// <param name="value">The new value of the property.</param>
    /// <param name="useStrictRules">The property set should follow strict mode rules.</param>
    public void SetProperty(JavaScriptPropertyId id, JavaScriptValue value, bool useStrictRules = true) {
      Native.ThrowIfError(Native.JsSetProperty(this, id, value, useStrictRules));
    }

    public void SetProperty(string id, JavaScriptValue value, bool useStrictRules = true) {
      this.SetProperty(JavaScriptPropertyId.FromString(id), value, useStrictRules);
    }

    /// <summary>
    ///     Deletes an object's property.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="propertyId">The ID of the property.</param>
    /// <param name="useStrictRules">The property set should follow strict mode rules.</param>
    /// <returns>Whether the property was deleted.</returns>
    public JavaScriptValue DeleteProperty(JavaScriptPropertyId propertyId, bool useStrictRules) {
      JavaScriptValue returnReference;
      Native.ThrowIfError(Native.JsDeleteProperty(this, propertyId, useStrictRules, out returnReference));
      return returnReference;
    }

    /// <summary>
    ///     Defines a new object's own property from a property descriptor.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="propertyId">The ID of the property.</param>
    /// <param name="propertyDescriptor">The property descriptor.</param>
    /// <returns>Whether the property was defined.</returns>
    public bool DefineProperty(JavaScriptPropertyId propertyId, JavaScriptValue propertyDescriptor) {
      bool result;
      Native.ThrowIfError(
        Native.JsDefineProperty(
          this,
          propertyId,
          propertyDescriptor,
          out result
        )
      );
      return result;
    }
    public bool DefineProperty(string id, JavaScriptValue propertyDescriptor) {
      bool result;
      Native.ThrowIfError(
        Native.JsDefineProperty(
          this,
          JavaScriptPropertyId.FromString(id),
          propertyDescriptor,
          out result
        )
      );
      return result;
    }

    /// <summary>
    ///     Test if an object has a value at the specified index.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="index">The index to test.</param>
    /// <returns>Whether the object has an value at the specified index.</returns>
    public bool HasIndexedProperty(JavaScriptValue index) {
      bool hasProperty;
      Native.ThrowIfError(Native.JsHasIndexedProperty(this, index, out hasProperty));
      return hasProperty;
    }

    /// <summary>
    ///     Retrieve the value at the specified index of an object.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="index">The index to retrieve.</param>
    /// <returns>The retrieved value.</returns>
    public JavaScriptValue GetIndexedProperty(JavaScriptValue index) {
      JavaScriptValue propertyReference;
      Native.ThrowIfError(Native.JsGetIndexedProperty(this, index, out propertyReference));
      return propertyReference;
    }

    /// <summary>
    ///     Set the value at the specified index of an object.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="index">The index to set.</param>
    /// <param name="value">The value to set.</param>
    public void SetIndexedProperty(JavaScriptValue index, JavaScriptValue value) {
      Native.ThrowIfError(Native.JsSetIndexedProperty(this, index, value));
    }

    /// <summary>
    ///     Delete the value at the specified index of an object.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="index">The index to delete.</param>
    public void DeleteIndexedProperty(JavaScriptValue index) {
      Native.ThrowIfError(Native.JsDeleteIndexedProperty(this, index));
    }

    /// <summary>
    ///     Compare two JavaScript values for equality.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     This function is equivalent to the "==" operator in JavaScript.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <param name="other">The object to compare.</param>
    /// <returns>Whether the values are equal.</returns>
    public bool Equals(JavaScriptValue other) {
      bool equals;
      Native.ThrowIfError(Native.JsEquals(this, other, out equals));
      return equals;
    }

    /// <summary>
    ///     Compare two JavaScript values for strict equality.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     This function is equivalent to the "===" operator in JavaScript.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <param name="other">The object to compare.</param>
    /// <returns>Whether the values are strictly equal.</returns>
    public bool StrictEquals(JavaScriptValue other) {
      bool equals;
      Native.ThrowIfError(Native.JsStrictEquals(this, other, out equals));
      return equals;
    }

    /// <summary>
    ///     Invokes a function.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="arguments">The arguments to the call.</param>
    /// <returns>The <c>Value</c> returned from the function invocation, if any.</returns>
    public JavaScriptValue CallFunction(params JavaScriptValue[] arguments) {
      if (arguments.Length > ushort.MaxValue) {
        throw new ArgumentOutOfRangeException("arguments");
      }

      JavaScriptValue returnReference;
      Native.ThrowIfError(Native.JsCallFunction(this, arguments, (ushort) arguments.Length, out returnReference));
      return returnReference;
    }

    /// <summary>
    ///     Invokes a function as a constructor.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="arguments">The arguments to the call.</param>
    /// <returns>The <c>Value</c> returned from the function invocation.</returns>
    public JavaScriptValue ConstructObject(params JavaScriptValue[] arguments) {
      if (arguments.Length > ushort.MaxValue) {
        throw new ArgumentOutOfRangeException("arguments");
      }

      JavaScriptValue returnReference;
      Native.ThrowIfError(Native.JsConstructObject(this, arguments, (ushort) arguments.Length, out returnReference));
      return returnReference;
    }

    public static void CreatePromise(out JavaScriptValue promise, out JavaScriptValue resolve, out JavaScriptValue reject) {
      Native.ThrowIfError(Native.JsCreatePromise(out promise, out resolve, out reject));
    }

    public string ToJsonString() {
      JavaScriptPropertyId jsonId = JavaScriptPropertyId.FromString("JSON");
      JavaScriptPropertyId stringifyId = JavaScriptPropertyId.FromString("stringify");
      JavaScriptValue json = JavaScriptValue.GlobalObject.GetProperty(jsonId);
      JavaScriptValue stringify = json.GetProperty(stringifyId);
      JavaScriptValue result = stringify.CallFunction(json, this);
      return result.ToString();
    }

    /// <summary>
    ///     Gets the symbol associated with the property ID.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <param name="propertyId">The property ID to get the symbol of.</param>
    /// <returns>
    ///     The symbol associated with the property ID.
    /// </returns>
    public static JavaScriptValue GetSymbolFromPropertyId(JavaScriptPropertyId propertyId) {
      JavaScriptValue symbol;
      Native.ThrowIfError(
        Native.JsGetSymbolFromPropertyId(
          propertyId,
          out symbol
        )
      );
      return symbol;
    }

    /// <summary>
    ///     Gets the property ID associated with the symbol.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Property IDs are specific to a context and cannot be used across contexts.
    ///     </para>
    ///     <para>
    ///     Requires an active script context.
    ///     </para>
    /// </remarks>
    /// <param name="symbol">
    ///     The symbol whose property ID is being retrieved.
    /// </param>
    /// <returns>
    ///     The property ID for the given symbol.
    /// </returns>
    public JavaScriptPropertyId GetPropertyIdFromSymbol() {
      JavaScriptPropertyId propertyId;
      Native.ThrowIfError(
        Native.JsGetPropertyIdFromSymbol(
          this,
          out propertyId
        )
      );
      return propertyId;
    }


    /// <summary>
    ///     Creates a Javascript symbol.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <param name="description">The string description of the symbol. Can be null.</param>
    /// <returns>
    ///     The new symbol.
    /// </returns>
    public static JavaScriptValue CreateSymbol(JavaScriptValue description) {
      JavaScriptValue symbol;
      Native.ThrowIfError(
        Native.JsCreateSymbol(
          description,
          out symbol
        )
      );
      return symbol;
    }
    public static JavaScriptValue CreateSymbol(string description) {
      return CreateSymbol(JavaScriptValue.FromString(description));
    }

    /// <summary>
    ///     Gets the list of all symbol properties on the object.
    /// </summary>
    /// <remarks>
    ///     Requires an active script context.
    /// </remarks>
    /// <returns>
    ///     An array of property symbols.
    /// </returns>
    public JavaScriptValue GetOwnPropertySymbols() {
      JavaScriptValue propertySymbols;
      Native.ThrowIfError(
        Native.JsGetOwnPropertySymbols(
          this,
          out propertySymbols
        )
      );
      return propertySymbols;
    }


  }
}
