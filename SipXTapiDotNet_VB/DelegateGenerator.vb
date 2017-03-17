'Copyright (c) 2006 ProfitFuel, Inc 
'
'This file is part of SipXTapiDotNet.

'    SipXTapiDotNet is free software; you can redistribute it and/or modify
'    it under the terms of the GNU Lesser General Public License as published by
'    the Free Software Foundation; either version 2.1 of the License, or
'    (at your option) any later version.
'
'    SipXTapiDotNet is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU Lesser General Public License for more details.
'
'    You should have received a copy of the GNU Lesser General Public License
'    along with SipXTapiDotNet; if not, write to the Free Software
'    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
'
Imports System
Imports System.Collections
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Runtime.InteropServices

''' <summary>
''' Emits custom descendants of the <see cref="[Delegate]"/> type and
''' creates instances of them. The generated invoke method of the delegate
''' is configured (by default) for <see cref="System.Runtime.InteropServices.CallingConvention.Cdecl">Cdecl calling convention</see>.
''' </summary>
Public Class DelegateGenerator
#Region "Private fields"

  Private Shared _syncRoot As Object = New Object()

  Private Shared _assembly As AssemblyBuilder

  Private Shared _module As ModuleBuilder

  Private Shared _types As Hashtable = Hashtable.Synchronized(New Hashtable())

#End Region

#Region "Private methods"

  ''' <summary>
  ''' Emits <see cref="MulticastDelegate"/> descendant suitable for specified method and specified <see cref="CallingConvention"/>.
  ''' </summary>
  ''' <param name="method">Method to emit delegate class for.</param>
  ''' <param name="typeName">The name of the emitted class.</param>
  ''' <param name="callingConvention">Calling convention.</param>
  ''' <returns>The emitted type.</returns>
  Private Shared Function Generate(ByVal method As MethodInfo, ByVal typeName As String, ByVal callingConvention As CallingConvention) As Type
    Dim callingConventionType As Type

    Select Case callingConvention
      Case callingConvention.Cdecl
        callingConventionType = System.Type.GetType("System.Runtime.CompilerServices.CallConvCdecl", True, True)

      Case callingConvention.FastCall
        callingConventionType = System.Type.GetType("System.Runtime.CompilerServices.CallConvFastcall", True, True)

      Case callingConvention.StdCall
        callingConventionType = System.Type.GetType("System.Runtime.CompilerServices.CallConvStdcall", True, True)

      Case callingConvention.ThisCall
        callingConventionType = System.Type.GetType("System.Runtime.CompilerServices.CallConvThiscall", True, True)

      Case Else
        Throw New ArgumentOutOfRangeException("callingConvention", callingConvention, "Unknown calling convention.")
    End Select

    SyncLock _syncRoot
      Dim resultType As Type = _types(typeName)

      If (resultType IsNot Nothing) Then
        Return resultType
      End If

      If (_assembly Is Nothing) Then
        Dim name As AssemblyName = New AssemblyName()

        name.Name = "CustomDelegatesAssembly"

        _assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave)

        _module = _assembly.DefineDynamicModule("CustomDelegatesAssembly.dll", "CustomDelegatesAssembly.dll", True)
      End If

      Dim type As TypeBuilder = _module.DefineType( _
        typeName, _
        TypeAttributes.Class Or TypeAttributes.Public Or TypeAttributes.Sealed Or TypeAttributes.AutoClass Or TypeAttributes.AnsiClass, _
        System.Type.GetType("System.MulticastDelegate", True, True))

      ' Constructor
      Dim ConstructorParameterTypes() As Type = {System.Type.GetType("System.Object"), System.Type.GetType("System.IntPtr")}
      Dim c As ConstructorBuilder = type.DefineConstructor(MethodAttributes.HideBySig Or MethodAttributes.Public, CallingConventions.Standard, ConstructorParameterTypes)

      c.SetImplementationFlags(MethodImplAttributes.Managed Or MethodImplAttributes.Runtime)

      ' "Invoke" method

      Dim parameters() As ParameterInfo = method.GetParameters()

      Dim parameterTypes(parameters.Length - 1) As Type

      For i As Integer = 0 To parameterTypes.Length - 1
        parameterTypes(i) = parameters(i).ParameterType
      Next

      Dim TypeBuilder As Type = type.GetType()
      Dim m As MethodBuilder

      Try
        'Reflector.InvokeMethod(TypeBuilder, "Enter", type)

        Reflector.InvokeMethod(TypeBuilder, "ThrowIfCreated", type)

        Dim x As System.Reflection.Emit.MethodBuilder

        m = Reflector.CreateInstance( _
          System.Type.GetType("System.Reflection.Emit.MethodBuilder", True, True), _
          "Invoke", _
          MethodAttributes.HideBySig Or MethodAttributes.Public Or MethodAttributes.Virtual, _
          CallingConventions.Standard, _
          method.ReturnType, _
          parameterTypes, _
          _module, _
          type, _
          False)

        ' Get signature
        Dim o As Object = Reflector.InvokeMethod(m.GetType(), "GetMethodSignature", m)

        Reflector.SetField(o.GetType(), "bSigDone", o, True)

        Reflector.InvokeMethod(o.GetType(), "SetNumberOfSignatureElements", o, False)

        Dim b() As Byte = Reflector.GetField(o.GetType(), "m_signature", o)

        Dim size As Integer = Reflector.GetField(o.GetType(), "m_currSig", o)

        ' Hack signature
        Try
          Dim token As TypeToken = _module.GetTypeToken(callingConventionType)

          ' Compress
          Dim typeRefToken As Byte = ((token.Token >> 24) Or ((token.Token And &HFF) << 2))

          Dim a(size + 1) As Byte

          a(0) = b(0)
          a(1) = b(1)
          a(2) = &H20 ' modopt (0x1f is modreq)
          a(3) = typeRefToken ' type def or type ref token

          Array.Copy(b, 2, a, 4, size - 2)

          b = a

          size += 1
          size += 1
        Finally
        End Try

        CType(Reflector.GetField(TypeBuilder, "m_listMethods", type), ArrayList).Add(m)

        o = Reflector.InvokeStaticMethod( _
          TypeBuilder, _
          "InternalDefineMethod", _
          Reflector.GetField(TypeBuilder, "m_tdType", type), _
          "Invoke", _
          b, _
          size, _
          m.Attributes, _
          _module)

        Reflector.InvokeMethod(m.GetType(), "SetToken", m, o)
      Finally
        'Reflector.InvokeMethod(TypeBuilder, "Exit", type)
      End Try

      m.SetImplementationFlags(MethodImplAttributes.Managed Or MethodImplAttributes.Runtime)

      For Each parameter As ParameterInfo In parameters
        Dim p As ParameterBuilder = m.DefineParameter(parameter.Position + 1, parameter.Attributes, parameter.Name)

        If ((parameter.Attributes And ParameterAttributes.HasDefault) <> 0) Then
          p.SetConstant(parameter.DefaultValue)
        End If

        ' Marshaling: not implemeted

        ' Custom attributes: should not be implemented
      Next

      ' "BeginInvoke" method

      Dim asyncParameterTypes(parameterTypes.Length + 1) As Type

      Array.Copy(parameterTypes, asyncParameterTypes, parameterTypes.Length)

      asyncParameterTypes(parameterTypes.Length) = System.Type.GetType("System.AsyncCallback", True, True)

      asyncParameterTypes(parameterTypes.Length + 1) = System.Type.GetType("System.Object", True, True)

      m = type.DefineMethod("BeginInvoke", MethodAttributes.HideBySig Or MethodAttributes.Public Or MethodAttributes.Virtual Or MethodAttributes.NewSlot, CallingConventions.Standard, System.Type.GetType("System.IAsyncResult", True, True), asyncParameterTypes)

      m.SetImplementationFlags(MethodImplAttributes.Managed Or MethodImplAttributes.Runtime)

      For Each parameter As ParameterInfo In parameters
        Dim p As ParameterBuilder = m.DefineParameter(parameter.Position + 1, parameter.Attributes, parameter.Name)

        If ((parameter.Attributes And ParameterAttributes.HasDefault) <> 0) Then
          p.SetConstant(parameter.DefaultValue)
        End If

        ' Marshaling: not implemeted

        ' Custom attributes: should not be implemented
      Next

      ' "EndInvoke" method
      Dim MethodParameterTypes() As Type = {System.Type.GetType("System.IAsyncResult")}
      m = type.DefineMethod("EndInvoke", MethodAttributes.HideBySig Or MethodAttributes.Public Or MethodAttributes.Virtual Or MethodAttributes.NewSlot, CallingConventions.Standard, method.ReturnType, MethodParameterTypes)

      m.SetImplementationFlags(MethodImplAttributes.Managed Or MethodImplAttributes.Runtime)

      '/* Save the emitted type */

      resultType = type.CreateType()

      _types(typeName) = resultType

      ' Debug
      ' _assembly.Save( _module.Name )

      Return resultType
    End SyncLock
  End Function

  ''' <summary>
  ''' Suggest name for delegate type by specified <see cref="MethodInfo"/> and <see cref="CallingConvention"/>.
  ''' </summary>
  ''' <param name="method"></param>
  ''' <param name="callingConvention"></param>
  ''' <returns></returns>
  Private Shared Function GetTypeFullName(ByVal method As MethodInfo, ByVal callingConvention As CallingConvention) As String
    Return "CustomDelegates" & Type.Delimiter.ToString() & method.DeclaringType.FullName & "_" & method.Name & "_" & callingConvention.ToString()
  End Function

#End Region

#Region "Public methods"

  ''' <summary>
  ''' Creates <see cref="MulticastDelegate"/> with <see cref="CallingConvention.Cdecl"/> calling convention.
  ''' </summary>
  ''' <param name="obj">The object on which method is defined (Nothing for shared methods).</param>
  ''' <param name="methodName">The name of the method.</param>
  ''' <returns>Instance of <see cref="MulticastDelegate"/> descendant.</returns>
  ''' <remarks>This method emits <see cref="MulticastDelegate"/> descending class (if not already emitted) suitable
  ''' for the specified <see cref="MethodInfo"/>.</remarks>
  Public Shared Function CreateDelegate(ByVal obj As Object, ByVal methodName As String) As [Delegate]
    Return CreateDelegate(obj, methodName, CallingConvention.Cdecl)
  End Function

  ''' <summary>
  ''' Creates <see cref="MulticastDelegate"/>.
  ''' </summary>
  ''' <param name="obj">The object on which method is defined (Nothing for shared methods).</param>
  ''' <param name="methodName">The name of the method.</param>
  ''' <param name="callingConvention"><see cref="CallingConvention"/> of the delegate's "Invoke" method.</param>
  ''' <returns>Instance of <see cref="MulticastDelegate"/> descendant.</returns>
  ''' <remarks>This method emits <see cref="MulticastDelegate"/> descending class (if not already emitted) suitable
  ''' for the specified <see cref="MethodInfo"/>.</remarks>
  Public Shared Function CreateDelegate(ByVal obj As Object, ByVal methodName As String, ByVal callingConvention As CallingConvention) As [Delegate]
    If (obj Is Nothing) Then
      Throw New ArgumentNullException("obj")
    End If

    Dim method As MethodInfo = obj.GetType().GetMethod(methodName, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Static)

    If (method Is Nothing) Then
      Throw New ArgumentOutOfRangeException("method", method, "The specified method was not found.")
    End If

    Return CreateDelegate(obj, method, callingConvention)
  End Function

  ''' <summary>
  ''' Creates <see cref="MulticastDelegate"/> with <see cref="CallingConvention.Cdecl"/> calling convention.
  ''' </summary>
  ''' <param name="type">The type of object on which method is defined.</param>
  ''' <param name="methodName">The name of the method.</param>
  ''' <returns>Instance of <see cref="MulticastDelegate"/> descendant.</returns>
  ''' <remarks>This method emits <see cref="MulticastDelegate"/> descending class (if not already emitted) suitable
  ''' for the specified <see cref="MethodInfo"/>.</remarks>
  Public Shared Function CreateDelegate(ByVal type As Type, ByVal methodName As String) As [Delegate]
    Return CreateDelegate(type, methodName, CallingConvention.Cdecl)
  End Function

  ''' <summary>
  ''' Creates <see cref="MulticastDelegate"/>.
  ''' </summary>
  ''' <param name="type">The type of object on which method is defined.</param>
  ''' <param name="methodName">The name of the method.</param>
  ''' <param name="callingConvention"><see cref="CallingConvention"/> of the delegate's "Invoke" method.</param>
  ''' <returns>Instance of <see cref="MulticastDelegate"/> descendant.</returns>
  ''' <remarks>This method emits <see cref="MulticastDelegate"/> descending class (if not already emitted) suitable
  ''' for the specified <see cref="MethodInfo"/>.</remarks>
  Public Shared Function CreateDelegate(ByVal type As Type, ByVal methodName As String, ByVal callingConvention As CallingConvention) As [Delegate]
    If (type Is Nothing) Then
      Throw New ArgumentNullException("type")
    End If

    Dim method As MethodInfo = type.GetMethod(methodName, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Static)

    If (method Is Nothing) Then
      Throw New ArgumentOutOfRangeException("method", method, "The specified method was not found.")
    End If

    Return CreateDelegate(Nothing, method, callingConvention)
  End Function

  ''' <summary>
  ''' Creates <see cref="MulticastDelegate"/> with <see cref="CallingConvention.Cdecl"/> calling convention.
  ''' </summary>
  ''' <param name="del">Delegate to obtain target object and <see cref="MethodInfo"/> from.</param>
  ''' <returns>Instance of <see cref="MulticastDelegate"/> descendant.</returns>
  ''' <remarks>This method emits <see cref="MulticastDelegate"/> descending class (if not already emitted) suitable
  ''' for the specified <see cref="MethodInfo"/>.</remarks>
  Public Shared Function CreateDelegate(ByVal del As [Delegate]) As [Delegate]
    Return CreateDelegate(del, CallingConvention.Cdecl)
  End Function

  ''' <summary>
  ''' Creates <see cref="MulticastDelegate"/>.
  ''' </summary>
  ''' <param name="del">Delegate to obtain target object and <see cref="MethodInfo"/> from.</param>
  ''' <param name="callingConvention"><see cref="CallingConvention"/> of the delegate's "Invoke" method.</param>
  ''' <returns>Instance of <see cref="MulticastDelegate"/> descendant.</returns>
  ''' <remarks>This method emits <see cref="MulticastDelegate"/> descending class (if not already emitted) suitable
  ''' for the specified <see cref="MethodInfo"/>.</remarks>
  Public Shared Function CreateDelegate(ByVal del As [Delegate], ByVal callingConvention As CallingConvention) As [Delegate]
    If (del Is Nothing) Then
      Throw New ArgumentNullException("del")
    End If

    Return CreateDelegate(del.Target, del.Method, callingConvention)
  End Function

  ''' <summary>
  ''' Creates <see cref="MulticastDelegate"/> with <see cref="CallingConvention.Cdecl"/> calling convention.
  ''' </summary>
  ''' <param name="obj">The object on which method is defined (Nothing for shared methods).</param>
  ''' <param name="method">The <see cref="MethodInfo"/> of the method for which a delegate is created.</param>
  ''' <returns>Instance of <see cref="MulticastDelegate"/> descendant.</returns>
  ''' <remarks>This method emits <see cref="MulticastDelegate"/> descending class (if not already emitted) suitable
  ''' for the specified <see cref="MethodInfo"/>.</remarks>
  Public Shared Function CreateDelegate(ByVal obj As Object, ByVal method As MethodInfo) As [Delegate]
    Return CreateDelegate(obj, method, CallingConvention.Cdecl)
  End Function

  ''' <summary>
  ''' Creates <see cref="MulticastDelegate"/>.
  ''' </summary>
  ''' <param name="obj">The object on which method is defined (Nothing for shared methods).</param>
  ''' <param name="method">The <see cref="MethodInfo"/> of the method for which a delegate is created.</param>
  ''' <param name="callingConvention"><see cref="CallingConvention"/> of the delegate's "Invoke" method.</param>
  ''' <returns>Instance of <see cref="MulticastDelegate"/> descendant.</returns>
  ''' <remarks>This method emits <see cref="MulticastDelegate"/> descending class (if not already emitted) suitable
  ''' for the specified <see cref="MethodInfo"/>.</remarks>
  Public Shared Function CreateDelegate(ByVal obj As Object, ByVal method As MethodInfo, ByVal callingConvention As CallingConvention) As [Delegate]
    If (method Is Nothing) Then
      Throw New ArgumentNullException("method")
    End If

    Dim typeName As String = GetTypeFullName(method, callingConvention)

    Dim type As Type = _types(typeName)

    If (type Is Nothing) Then
      type = Generate(method, typeName, callingConvention)
    End If

    Return Activator.CreateInstance(type, New Object() {obj, method.MethodHandle.GetFunctionPointer()})
  End Function

#End Region


  ''' <summary>
  ''' Provides reflection helper methods.
  ''' </summary>
  Private NotInheritable Class Reflector
    ''' <summary>
    ''' No new, Shareds only
    ''' </summary>
    Private Sub New()
    End Sub

    Public Shared Function CreateInstance(ByVal assembly As Assembly, ByVal name As String, ByVal ParamArray parameters() As Object) As Object
      If (assembly Is Nothing) Then
        Throw New ArgumentNullException("assembly")
      End If

      Return CreateInstance(assembly.GetType(name), parameters)
    End Function

    Public Shared Function CreateInstance(ByVal type As Type, ByVal ParamArray parameters() As Object) As Object
      Return Activator.CreateInstance( _
        type, _
        BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance, _
        Nothing, _
        parameters, _
        Nothing)
    End Function

    Public Shared Function InvokeMethod(ByVal type As Type, ByVal name As String, ByVal instance As Object, ByVal ParamArray parameters() As Object) As Object
      Return InvokeMember(type, name, instance, BindingFlags.InvokeMethod Or BindingFlags.Instance, parameters)
    End Function

    Public Shared Function GetProperty(ByVal type As Type, ByVal name As String, ByVal instance As Object) As Object
      Return InvokeMember(type, name, instance, BindingFlags.GetProperty Or BindingFlags.Instance)
    End Function

    Public Shared Function SetProperty(ByVal type As Type, ByVal name As String, ByVal instance As Object, ByVal value As Object) As Object
      Return InvokeMember(type, name, instance, BindingFlags.SetProperty Or BindingFlags.Instance, value)
    End Function

    Public Shared Function GetField(ByVal type As Type, ByVal name As String, ByVal instance As Object) As Object
      Return InvokeMember(type, name, instance, BindingFlags.GetField Or BindingFlags.Instance)
    End Function

    Public Shared Function SetField(ByVal type As Type, ByVal name As String, ByVal instance As Object, ByVal value As Object) As Object
      Return InvokeMember(type, name, instance, BindingFlags.SetField Or BindingFlags.Instance, value)
    End Function

    Public Shared Function InvokeStaticMethod(ByVal type As Type, ByVal name As String, ByVal ParamArray parameters() As Object) As Object
      Return InvokeMember(type, name, Nothing, BindingFlags.InvokeMethod Or BindingFlags.Static, parameters)
    End Function

    Public Shared Function InvokeMember(ByVal type As Type, ByVal name As String, ByVal instance As Object, ByVal flags As BindingFlags, ByVal ParamArray parameters() As Object) As Object
      Return type.InvokeMember( _
        name, _
        BindingFlags.Public Or BindingFlags.NonPublic Or flags, _
        Nothing, _
        instance, _
        parameters)
    End Function
  End Class
End Class


