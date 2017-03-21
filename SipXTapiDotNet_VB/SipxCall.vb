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
Public Class SipxCall
  Implements IDisposable

#Region "Dll Imports"
  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallCreate")> _
  Protected Shared Function SipxCallCreate(ByVal instanceHandle As IntPtr, ByVal lineHandle As IntPtr, ByRef callHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallConnect")> _
  Protected Shared Function SipxCallConnect(ByVal callHandle As IntPtr, ByVal address As String, Optional ByVal contactId As Integer = 0, Optional ByVal display As Object = Nothing, Optional ByVal security As Object = Nothing, Optional ByVal takeFocus As Boolean = True, Optional ByVal options As Object = Nothing, Optional ByVal callId As String = Nothing) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallAccept")> _
  Protected Shared Function SipxCallAccept(ByVal callHandle As IntPtr, Optional ByVal display As Object = Nothing, Optional ByVal security As Object = Nothing, Optional ByVal options As Object = Nothing) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallReject")> _
  Protected Shared Function SipxCallReject(ByVal callHandle As IntPtr, Optional ByVal errorCode As Integer = 400, Optional ByVal errorText As String = "Bad Request") As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallRedirect")> _
  Protected Shared Function SipxCallRedirect(ByVal callHandle As IntPtr, ByVal forwardUrl As String) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallAnswer")> _
  Protected Shared Function SipxCallAnswer(ByVal callHandle As IntPtr, Optional ByVal takeFocus As Boolean = True) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallHold")> _
  Protected Shared Function SipxCallHold(ByVal callHandle As IntPtr, Optional ByVal stopRemoteAudio As Boolean = True) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallUnhold")> _
  Protected Shared Function SipxCallUnhold(ByVal callHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallDestroy")> _
  Protected Shared Function SipxCallDestroy(ByRef callHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallStartTone")> _
  Protected Shared Function SipxCallStartTone(ByVal callHandle As IntPtr, ByVal toneId As SipxToneId, ByVal playLocal As Boolean, ByVal playRemote As Boolean) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxCallStopTone")> _
  Protected Shared Function SipxCallStopTone(ByVal callHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, Entrypoint:="sipxCallAudioPlayFileStart")> _
  Protected Shared Function SipxCallAudioPlayFileStart(ByVal callHandle As IntPtr, ByVal file As String, ByVal repeat As Boolean, ByVal playLocal As Boolean, ByVal playRemote As Boolean, Optional ByVal mixWithMircophone As Boolean = False, Optional ByVal VolumeScaling As Single = 1.0) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, Entrypoint:="sipxCallAudioPlayFileStop")> _
  Protected Shared Function SipxCallAudioPlayFileStop(ByVal callHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, Entrypoint:="sipxCallAudioRecordFileStart")> _
  Protected Shared Function SipxCallAudioRecordFileStart(ByVal callHandle As IntPtr, ByVal file As String) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, Entrypoint:="sipxCallAudioRecordFileSetMask")> _
    Protected Shared Function SipxCallAudioRecordFileSetMask(ByVal RecorderMask As Integer) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, Entrypoint:="sipxCallAudioRecordFileStop")> _
    Protected Shared Function SipxCallAudioRecordFileStop(ByVal callHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, Entrypoint:="sipxCallGetRemoteID")> _
  Protected Shared Function SipxCallGetRemoteID(ByVal callHandle As IntPtr, ByVal id As IntPtr, ByVal maxLength As IntPtr) As SipxResult
  End Function

#End Region

  Private m_CallConnectedEvent As Threading.ManualResetEvent
  Private m_CallHeldEvent As Threading.ManualResetEvent
  Private m_CallDialtoneEvent As Threading.ManualResetEvent
  Private m_CallDestroyedEvent As Threading.ManualResetEvent
  Private m_CallDisposeLock As New Object
  Private m_CallRecordLock As New Object
  Private m_CallHandle As IntPtr
  Private m_CallNumber As Integer
  Private m_Line As SipxLine
  Private m_Instance As SipxInstance
  Private m_IsRecording As Boolean
  Private m_EarlyMedia As Boolean
  Private WithEvents m_EventQueueMonitor As EventQueueMonitor
  Public Event CallEvent As EventHandler(Of SipxCallEventArgs)

  Friend ReadOnly Property CallHandle() As IntPtr
    Get
      Return m_CallHandle
    End Get
  End Property
  Public ReadOnly Property CallNubmer() As Integer
    Get
      Return m_CallNumber
    End Get
  End Property
  Public ReadOnly Property IsRecording()
    Get
      Return m_IsRecording
    End Get
  End Property
  Public ReadOnly Property RemoteAddress(Optional ByVal fullUrl As Boolean = False) As String
    Get
      Dim rv As String
      Dim x As IntPtr
      x = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(255)
      If SipxCallGetRemoteID(CallHandle, x, 255) = SipxResult.Success Then
        rv = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(x)
        System.Runtime.InteropServices.Marshal.FreeCoTaskMem(x)
        If rv.Contains("sip:") Then
          If Not fullUrl Then
            rv = rv.Remove(0, rv.IndexOf(":", 0))
            rv = rv.Substring(0, rv.IndexOf("@", 0))
          End If
        Else
          rv = ""
        End If
      Else
        rv = ""
      End If
      Return rv
    End Get
  End Property

  Public Sub Connect(ByVal address As String)
    Dim MySipxResult As SipxResult

    m_EarlyMedia = False
    MySipxResult = SipxCallConnect(m_CallHandle, address)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    Else
      Console.WriteLine("Success!")
    End If
  End Sub

  Public Sub StartTone(ByVal tone As SipxToneId, ByVal playLocal As Boolean, ByVal playRemote As Boolean)
    Dim MySipxResult As SipxResult
    If tone <> SipxToneId.Tone_Ringback Or Not m_EarlyMedia Then
      MySipxResult = SipxCallStartTone(m_CallHandle, tone, playLocal, playRemote)
      If MySipxResult <> SipxResult.Success Then
        Throw New SipxException(MySipxResult)
      End If
    End If
  End Sub
  Public Sub StopTone()
    Dim MySipxResult As SipxResult

    MySipxResult = SipxCallStopTone(m_CallHandle)
    'If MySipxResult <> SipxResult.Success Then
    '  Throw New SipxException(MySipxResult)
    'End If
  End Sub

  Public Sub Reject(ByVal myErrorCode As Integer, ByVal myErrorText As String)
    Dim MySipxResult As SipxResult
    MySipxResult = SipxCallReject(m_CallHandle, myErrorCode, myErrorText)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub Answer()
    Dim MySipxResult As SipxResult

    m_CallConnectedEvent.Reset()
    MySipxResult = SipxCallAnswer(m_CallHandle, True)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    ElseIf m_CallConnectedEvent.WaitOne(3000, False) Then
      Console.WriteLine("Success!")
    Else
      Console.WriteLine("CallConnect Timed out")
    End If
  End Sub

  Public Sub Accept()
    Dim MySipxResult As SipxResult
    'TODO: Pass Display, Security and Options parameters to SipxCallAccept()
    MySipxResult = SipxCallAccept(m_CallHandle, Nothing, Nothing, Nothing)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub Hold(ByVal stopRemoteAudio As Boolean)
    Dim MySipxResult As SipxResult

    MySipxResult = SipxCallHold(m_CallHandle, stopRemoteAudio)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    ElseIf m_CallHeldEvent.WaitOne(3000, False) Then
      Console.WriteLine("Hold Success!")
    End If
  End Sub

  Public Sub Unhold()
    Dim MySipxResult As SipxResult

    MySipxResult = SipxCallUnhold(m_CallHandle)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Private Sub m_CallEvent(ByVal sender As Object, ByVal e As SipxCallEventArgs) Handles m_EventQueueMonitor.SipxCallEvent
    If e.CallStateinfo.CallHandle = Me.m_CallHandle Then
      Dim MyEventArgs As SipxCallEventArgs = e
      Console.WriteLine("SipxCall_Event: " & MyEventArgs.EventCategory.ToString & ", " & MyEventArgs.CallStateinfo.StateEvent.ToString & ", " & MyEventArgs.CallStateinfo.StateCause.ToString)

      Select Case MyEventArgs.CallStateinfo.StateEvent
        Case SipxCallStateEvent.Connected
          m_CallConnectedEvent.Set()
        Case SipxCallStateEvent.Held
          m_CallHeldEvent.Set()
        Case SipxCallStateEvent.RemoteHeld
          m_CallHeldEvent.Set()
        Case SipxCallStateEvent.RemoteOffering
        Case SipxCallStateEvent.RemoteAlerting
          If MyEventArgs.CallStateinfo.StateCause = SipxCallStateCause.EarlyMedia Then
            m_EarlyMedia = True
          End If
        Case SipxCallStateEvent.Dialtone
          m_CallDialtoneEvent.Set()
        Case SipxCallStateEvent.Destroyed
          m_CallDestroyedEvent.Set()
        Case SipxCallStateEvent.Disconnected
          m_CallConnectedEvent.Set()
          m_CallHeldEvent.Set()
      End Select
      RaiseEvent CallEvent(Me, MyEventArgs)
    End If
  End Sub

  Public Sub Init()
    Dim MySipxResult As SipxResult

    m_CallDialtoneEvent.Reset()
    MySipxResult = SipxCallCreate(m_Instance.InstanceHandle, m_Line.LineHandle, m_CallHandle)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    ElseIf m_CallDialtoneEvent.WaitOne(3000, False) Then
      Console.WriteLine("Call Create Success!")
    End If
  End Sub

  Friend Sub AttachIncoming(ByVal callHandle As IntPtr)
    m_CallHandle = callHandle
  End Sub

  Public Shared Function Create(ByVal line As SipxLine, ByVal callNumber As Integer) As SipxCall
    Dim rv As New SipxCall

    rv.m_Line = line
    rv.m_Instance = line.Instance
    rv.m_EventQueueMonitor = EventQueueMonitor.GetMonitor(line.Instance)
    rv.m_CallNumber = callNumber
    Return rv
  End Function

  Private Sub New()
    m_CallConnectedEvent = New Threading.ManualResetEvent(True)
    m_CallHeldEvent = New Threading.ManualResetEvent(True)
    m_CallDialtoneEvent = New Threading.ManualResetEvent(True)
    m_CallDestroyedEvent = New Threading.ManualResetEvent(True)
  End Sub

  Public Sub RecordingStart(ByVal filename As String, ByVal mic As Boolean, ByVal spkr As Boolean)
    Dim recorderMask As Integer = 0
    Dim mySipxResult As SipxResult
    SyncLock m_CallRecordLock
      If mic Then recorderMask = recorderMask Or 1
      If spkr Then recorderMask = recorderMask Or 4
      mySipxResult = SipxCallAudioRecordFileSetMask(recorderMask)
      If mySipxResult <> SipxResult.Success Then
        Throw New SipxException(mySipxResult)
      End If
      If Not m_IsRecording Then
        mySipxResult = SipxCallAudioRecordFileStart(m_CallHandle, filename)
        If mySipxResult <> SipxResult.Success Then
          Throw New SipxException(mySipxResult)
        Else
          m_IsRecording = True
        End If
      End If
    End SyncLock
  End Sub

  Public Sub RecordingStop()
    SyncLock m_CallRecordLock
      If m_IsRecording Then
        Dim mySipxResult As SipxResult
        Try
          mySipxResult = SipxCallAudioRecordFileStop(m_CallHandle)
          'We don't want to error if this fails, so we will always assume success.
          'If mySipxResult <> SipxResult.Success Then
          'Throw New SipxException(mySipxResult)
          'Else
        Catch ex As Exception
          'Not sure if this will save us from the error or not...
        End Try
        m_IsRecording = False
        'End If
      End If
    End SyncLock
  End Sub

#Region " IDisposable Support "
  Private m_IsDisposed As Boolean = False    ' To detect redundant calls
  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    Dim MySipxResult As SipxResult
    Dim callHandle As IntPtr = m_CallHandle
    SyncLock m_CallDisposeLock
      If Not m_IsDisposed Then
        If disposing Then
          ' TODO: free unmanaged resources when explicitly called
          m_CallDestroyedEvent.Reset()
          RecordingStop()
          MySipxResult = SipxCallDestroy(callHandle)
          If MySipxResult <> SipxResult.Success Then
            Throw New SipxException(MySipxResult)
          ElseIf m_CallDestroyedEvent.WaitOne(10000, False) Then
            Console.WriteLine("Success!")
          End If
          If m_Line.Calls.ContainsKey(m_CallNumber) AndAlso m_Line.Calls(m_CallNumber) Is Me Then
            m_Line.Calls.Remove(m_CallNumber)
          End If
          m_CallNumber = 0
        End If
        ' TODO: free shared unmanaged resources
      End If
      m_IsDisposed = True
    End SyncLock
  End Sub

  ' This code added by Visual Basic to correctly implement the disposable pattern.
  Public Sub Dispose() Implements IDisposable.Dispose
    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
#End Region

End Class
