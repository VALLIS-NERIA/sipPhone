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
Public Class SipxMedia
  Implements IDisposable

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioSetGain")> _
  Protected Shared Function SipxAudioSetGain(ByVal instanceHandle As IntPtr, ByVal gainLevel As Integer) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioGetGain")> _
  Protected Shared Function SipxAudioGetGain(ByVal instanceHandle As IntPtr, ByRef gainLevel As Integer) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioMute")> _
  Protected Shared Function SipxAudioMute(ByVal instanceHandle As IntPtr, ByVal muteAudio As Boolean) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioIsMuted")> _
  Protected Shared Function SipxAudioIsMuted(ByVal instanceHandle As IntPtr, ByRef isAudioMuted As Boolean) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioEnableSpeaker")> _
  Protected Shared Function SipxAudioEnableSpeaker(ByVal instanceHandle As IntPtr, ByVal speakerType As SipxSpeakerType) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioGetEnabledSpeaker")> _
  Protected Shared Function SipxAudioGetEnabledSpeaker(ByVal instanceHandle As IntPtr, ByRef speakerType As SipxSpeakerType) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioSetVolume")> _
  Protected Shared Function SipxAudioSetVolume(ByVal instanceHandle As IntPtr, ByVal speakerType As SipxSpeakerType, ByVal volumeLevel As Integer) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioGetVolume")> _
  Protected Shared Function SipxAudioGetVolume(ByVal instanceHandle As IntPtr, ByVal speakerType As SipxSpeakerType, ByRef volumeLevel As Integer) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioSetAECMode")> _
  Protected Shared Function SipxAudioSetAecMode(ByVal instanceHandle As IntPtr, ByVal aecMode As SipxAecMode) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioGetAECMode")> _
  Protected Shared Function SipxAudioGetAecMode(ByVal instanceHandle As IntPtr, ByRef aecMode As SipxAecMode) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioSetAGCMode")> _
  Protected Shared Function SipxAudioSetAgcMode(ByVal instanceHandle As IntPtr, ByVal enable As Boolean) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioGetAGCMode")> _
  Protected Shared Function SipxAudioGetAgcMode(ByVal instanceHandle As IntPtr, ByRef isEnabled As Boolean) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioSetNoiseReductionMode")> _
  Protected Shared Function SipxAudioSetNoiseReductionMode(ByVal hInst As IntPtr, ByVal mode As SipxNoiseReductionMode) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioGetNumInputDevices")> _
  Protected Shared Function SipxAudioGetNumInputDevices(ByVal instanceHandle As IntPtr, ByRef numberOfDevices As Integer) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioGetInputDevice")> _
  Protected Shared Function SipxAudioGetInputDevice(ByVal instanceHandle As IntPtr, ByVal deviceNumber As Integer, ByRef deviceName As String) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioGetNumOutputDevices")> _
  Protected Shared Function SipxAudioGetNumOutputDevices(ByVal instanceHandle As IntPtr, ByRef numberOfDevices As Integer) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioGetOutputDevice")> _
  Protected Shared Function SipxAudioGetOutputDevice(ByVal instanceHandle As IntPtr, ByVal deviceNumber As Integer, ByRef deviceName As String) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioSetCallInputDevice")> _
  Protected Shared Function SipxAudioSetCallInputDevice(ByVal instanceHandle As IntPtr, ByVal deviceName As String) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioSetRingerOutputDevice")> _
  Protected Shared Function SipxAudioSetRingerOutputDevice(ByVal instanceHandle As IntPtr, ByVal deviceName As String) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxAudioSetCallOutputDevice")> _
  Protected Shared Function SipxAudioSetCallOutputDevice(ByVal instanceHandle As IntPtr, ByVal deviceName As String) As SipxResult
  End Function

  Private m_Instance As SipxInstance
  Private WithEvents m_EventQueueMonitor As EventQueueMonitor
  Public Event AudioEvent As EventHandler(Of SipxEventArgs)

  Public ReadOnly Property Instance() As SipxInstance
    Get
      Return m_Instance
    End Get
  End Property

  Public ReadOnly Property MicIsMuted() As Boolean
    Get
      Dim MySipxResult As SipxResult
      Dim isMuted As Boolean
      MySipxResult = SipxAudioIsMuted(Instance.InstanceHandle, isMuted)
      If MySipxResult <> SipxResult.Success Then
        Throw New SipxException(MySipxResult)
      End If
      Return isMuted
    End Get
  End Property

  Public Sub SetMicMute(ByVal mute As Boolean)
    Dim MySipxResult As SipxResult
    MySipxResult = SipxAudioMute(Instance.InstanceHandle, mute)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub SetInputDevice(ByVal deviceName As String)
    Dim MySipxResult As SipxResult

    MySipxResult = SipxAudioSetCallInputDevice(m_Instance.InstanceHandle, deviceName)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub SetOutputDevice(ByVal deviceName As String)
    Dim MySipxResult As SipxResult

    MySipxResult = SipxAudioSetCallOutputDevice(m_Instance.InstanceHandle, deviceName)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub SetRingerOutputDevice(ByVal deviceName As String)
    Dim MySipxResult As SipxResult

    MySipxResult = SipxAudioSetRingerOutputDevice(m_Instance.InstanceHandle, deviceName)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub SetAecMode(ByVal aecMode As SipxAecMode)
    Dim MySipxResult As SipxResult
    MySipxResult = SipxAudioSetAecMode(m_Instance.InstanceHandle, aecMode)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub SetAGC(ByVal enable As Boolean)
    Dim MySipxResult As SipxResult
    MySipxResult = SipxAudioSetAgcMode(m_Instance.InstanceHandle, enable)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub SetNoiseReductionMode(ByVal mode As SipxNoiseReductionMode)
    Dim MySipxResult As SipxResult
    MySipxResult = SipxAudioSetNoiseReductionMode(m_Instance.InstanceHandle, mode)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Function GetVolume(ByVal type As SipxSpeakerType) As Integer
    Dim volume As Integer
    Dim MySipxResult As SipxResult
    MySipxResult = SipxAudioGetVolume(m_Instance.InstanceHandle, type, volume)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
    Return volume
  End Function

  Public Sub SetVolume(ByVal type As SipxSpeakerType, ByVal volume As Integer)
    Dim MySipxResult As SipxResult
    MySipxResult = SipxAudioSetVolume(m_Instance.InstanceHandle, type, volume)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Function GetGain() As Integer
    Dim gain As Integer
    Dim MySipxResult As SipxResult
    MySipxResult = SipxAudioGetGain(m_Instance.InstanceHandle, gain)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
    Return gain
  End Function

  Public Sub Setgain(ByVal gain)
    Dim MySipxResult As SipxResult
    MySipxResult = SipxAudioSetGain(m_Instance.InstanceHandle, gain)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Sub DumpDevices(ByVal type As SipxMediaDeviceType)
    Dim deviceList As Generic.List(Of String) = GetDevices(type)
    Console.WriteLine(type.ToString & " Devices: " & deviceList.Count)
    For i As Integer = 0 To deviceList.Count - 1
      Console.WriteLine(vbTab & "#" & i & ": '" & deviceList.Item(i) & "'")
    Next
  End Sub

  Public Function GetDevices(ByVal type As SipxMediaDeviceType) As Generic.List(Of String)
    Dim rv As New Generic.List(Of String)
    ' Display the list of input devices
    Dim NumberOfDevices As Integer
    Dim DeviceName As String = ""
    Dim MySipxResult As SipxResult

    If type = SipxMediaDeviceType.Input Then
      MySipxResult = SipxAudioGetNumInputDevices(m_Instance.InstanceHandle, NumberOfDevices)
    Else
      MySipxResult = SipxAudioGetNumOutputDevices(m_Instance.InstanceHandle, NumberOfDevices)
    End If
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    Else
      For i As Integer = 0 To NumberOfDevices - 1
        If type = SipxMediaDeviceType.Input Then
          MySipxResult = SipxAudioGetInputDevice(m_Instance.InstanceHandle, i, DeviceName)
        Else
          MySipxResult = SipxAudioGetOutputDevice(m_Instance.InstanceHandle, i, DeviceName)
        End If
        If MySipxResult <> SipxResult.Success Then
          Throw New SipxException(MySipxResult)
        Else
          rv.Add(DeviceName)
        End If
      Next
    End If
    Return rv
  End Function

  Public Shared Function Create(ByVal instance As SipxInstance) As SipxMedia
    Dim rv As New SipxMedia

    rv.m_Instance = instance
    rv.m_EventQueueMonitor = EventQueueMonitor.GetMonitor(instance)

    Return rv
  End Function
  Private Sub New()
  End Sub

  Private Sub m_AudioEvent(ByVal sender As Object, ByVal e As SipxEventArgs) Handles m_EventQueueMonitor.SipxMediaEvent
    RaiseEvent AudioEvent(Me, e)
  End Sub

#Region " IDisposable Support "
  Private m_IsDisposed As Boolean = False    ' To detect redundant calls

  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    If Not m_IsDisposed Then
      If disposing Then
        ' TODO: free unmanaged resources when explicitly called
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
