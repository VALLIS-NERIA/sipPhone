'Copyright (c) 2006 ProfitFuel, Inc (Authors: Charlie Hedlin & Joshua Garvin) 
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
Public Class SipxLine
  Implements IDisposable

#Region "DllImports"
  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxLineAdd")> _
  Protected Shared Function SipxLineAdd(ByVal instanceHandle As IntPtr, ByVal fromIdentity As String, ByRef lineHandle As IntPtr, Optional ByVal contactid As Integer = 0) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxLineRegister")> _
  Protected Shared Function SipxLineRegister(ByVal lineHandle As IntPtr, ByVal register As Boolean) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxLineRemove")> _
  Protected Shared Function SipxLineRemove(ByVal lineHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxLineAddCredential")> _
  Protected Shared Function SipxLineAddCredential(ByVal lineHandle As IntPtr, ByVal username As String, ByVal password As String, ByVal realm As String) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigSubscribe")> _
  Protected Shared Function SipxConfigSubscribe(ByVal instanceHandle As IntPtr, ByVal lineHandle As IntPtr, ByVal targetUrl As String, ByVal eventType As String, ByVal acceptType As String, ByVal contactId As Integer, ByRef subHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigUnsubscribe")> _
  Protected Shared Function sipxConfigUnsubscribe(ByVal subHandle As IntPtr) As SipxResult
  End Function

#End Region

  'Private m_Pool As New Threading.Semaphore(0, 1)
  Private m_LineRegisterWaitHandle As Threading.ManualResetEvent
  Private m_LineUnRegisterWaitHandle As Threading.ManualResetEvent
  Private m_LineProvisionWaitHandle As Threading.ManualResetEvent
  Private m_LineHandle As IntPtr
  Private m_Registered As Boolean
  Private m_Instance As SipxInstance
  Private WithEvents m_EventQueue As EventQueueMonitor
  Private m_Calls As Dictionary(Of Integer, SipxCall)
  Private m_MwiHandle As IntPtr

  Public Event CallEvent As EventHandler(Of SipxCallEventArgs)
  Public Event LineEvent As EventHandler(Of SipxLineEventArgs)

  Public ReadOnly Property Calls() As Dictionary(Of Integer, SipxCall)
    Get
      Return m_Calls
    End Get
  End Property

  Public ReadOnly Property CallExists(ByVal callnumber As Integer) As Boolean
    Get
      Return Calls.ContainsKey(callnumber) AndAlso Calls(callnumber) IsNot Nothing
    End Get
  End Property

  Public ReadOnly Property Instance() As SipxInstance
    Get
      Return m_Instance
    End Get
  End Property

  Friend ReadOnly Property LineHandle() As IntPtr
    Get
      Return m_LineHandle
    End Get
  End Property

  Public Function CallCreate() As Integer
    Return CallCreate(True)
  End Function

  Private Function CallCreate(ByVal init As Boolean) As Integer
    Dim i As Integer = 1
    Do While (m_Calls.ContainsKey(i) AndAlso m_Calls(i) IsNot Nothing)
      i = i + 1
    Loop
    Calls.Add(i, SipxCall.Create(Me, i))
    AddHandler m_Calls(i).CallEvent, AddressOf m_CallEvent
    If init Then
      Calls(i).Init()
    End If
    Return i
  End Function

  Public Sub AddCreditial(ByVal username As String, ByVal password As String, ByVal realm As String)
    Dim MySipxResult As SipxResult

    MySipxResult = SipxLineAddCredential(m_LineHandle, username, password, realm)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub Register()
    Dim MySipxResult As SipxResult

    m_LineRegisterWaitHandle.Reset()
    MySipxResult = SipxLineRegister(m_LineHandle, True)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    ElseIf m_LineRegisterWaitHandle.WaitOne(3000, False) Then
      Console.WriteLine("Success!")
      m_Registered = True
    End If
  End Sub

  Public Sub Unregister()
    Dim MySipxResult As SipxResult
    UnsubscribeMwi()
    m_LineUnRegisterWaitHandle.Reset()
    MySipxResult = SipxLineRegister(m_LineHandle, False)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    ElseIf m_LineUnRegisterWaitHandle.WaitOne(3000, False) Then
      Console.WriteLine("Success!")
      m_Registered = False
    End If
  End Sub

  Public Sub SubscribeMwi()
    SipxConfigSubscribe(m_Instance.InstanceHandle, m_LineHandle, "sip:6409@sip.profitfuel.net", "message-summary", "application/simple-message-summary", 1, m_MwiHandle)
  End Sub

  Public Sub UnsubscribeMwi()
    If m_MwiHandle <> 0 Then
      sipxConfigUnsubscribe(m_MwiHandle)
    End If
  End Sub

  Private Sub m_CallEvent(ByVal sender As Object, ByVal e As SipxCallEventArgs)
    RaiseEvent CallEvent(sender, e)
  End Sub

  Private Sub m_NewCallEvent(ByVal sender As Object, ByVal e As SipxCallEventArgs) Handles m_EventQueue.SipxCallEvent
    If e.CallStateinfo.StateEvent = SipxCallStateEvent.NewCall Then
      Dim i As Integer = 1 + 1
    End If
    If e.CallStateinfo.StateEvent = SipxCallStateEvent.NewCall And (e.CallStateinfo.LineHandle = m_LineHandle Or e.CallStateinfo.LineHandle = 0 And m_LineHandle = 1) Then
      Dim myCallnumber As Integer = CallCreate(False)
      Calls(myCallnumber).AttachIncoming(e.CallStateinfo.CallHandle)
      RaiseEvent CallEvent(Calls(myCallnumber), e)
    End If
  End Sub

  Private Sub m_LineEvent(ByVal sender As Object, ByVal e As SipxLineEventArgs) Handles m_EventQueue.SipxLineEvent
    If e.LineStateinfo.LineHandle = m_LineHandle Then
      Dim MyEventArgs As SipxLineEventArgs = e
      Console.WriteLine("Event: " & MyEventArgs.EventCategory.ToString & ", " & MyEventArgs.LineStateinfo.StateEvent.ToString & ", " & MyEventArgs.LineStateinfo.StateCause.ToString)
      Select Case MyEventArgs.LineStateinfo.StateEvent
        Case SipxLineStateEvent.Provisioned
          m_LineProvisionWaitHandle.Set()
        Case SipxLineStateEvent.Registered
          m_LineRegisterWaitHandle.Set()
        Case SipxLineStateEvent.Unregistered
          m_LineUnRegisterWaitHandle.Set()
        Case Else
          Dim i = 1 + 1
      End Select
      RaiseEvent LineEvent(Me, MyEventArgs)
    End If
  End Sub

  Public Sub Provision()
    Dim MySipxResult As SipxResult
    Dim LineHandle As IntPtr

    m_LineProvisionWaitHandle.Reset()
    MySipxResult = SipxLineAdd(Instance.InstanceHandle, Instance.Identity, LineHandle)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    ElseIf m_LineProvisionWaitHandle.WaitOne(3000, False) Then
      Console.WriteLine("Success!")
    End If

    m_LineHandle = LineHandle
  End Sub

  Public Shared Function Create(ByVal instance As SipxInstance) As SipxLine
    Dim rv As New SipxLine
    rv.m_Instance = instance
    rv.m_EventQueue = EventQueueMonitor.GetMonitor(instance)
    Return rv
  End Function

  Private Sub New()
    m_LineRegisterWaitHandle = New Threading.ManualResetEvent(True)
    m_LineUnRegisterWaitHandle = New Threading.ManualResetEvent(True)
    m_LineProvisionWaitHandle = New Threading.ManualResetEvent(True)
    m_Calls = New Dictionary(Of Integer, SipxCall)()
  End Sub

#Region " IDisposable Support "
  Private m_IsDisposed As Boolean = False    ' To detect redundant calls
  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    If Not m_IsDisposed Then
      If disposing Then
        ' TODO: free unmanaged resources when explicitly called

        If Calls.Count > 0 Then
          Dim CallKeys As New Generic.List(Of Integer)
          For Each Callkey As Integer In Calls.Keys
            CallKeys.Add(Callkey)
          Next
          For Each CallKey As Integer In CallKeys
            Calls(CallKey).Dispose()
          Next
        End If

        If m_LineHandle <> 0 Then
          If m_Registered Then
            Unregister()
          End If

          m_LineUnRegisterWaitHandle.Reset()
          Dim MySipxResult As SipxResult
          MySipxResult = SipxLineRemove(m_LineHandle)
          If MySipxResult <> SipxResult.Success Then
            Throw New SipxException(MySipxResult)
          ElseIf m_LineUnRegisterWaitHandle.WaitOne(3000, False) Then
            Console.WriteLine("Success!")
          End If
        End If
      End If

      ' TODO: free shared unmanaged resources
    End If
    m_IsDisposed = True
  End Sub

  ' This code added by Visual Basic to correctly implement the disposable pattern.
  Public Sub Dispose() Implements IDisposable.Dispose
    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
#End Region

End Class
