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
Public Class SipxInstance
  Implements IDisposable

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxInitialize")> _
  Protected Shared Function SipxInitialize(ByRef instanceHandle As IntPtr, Optional ByVal tcpPort As Integer = 5060, Optional ByVal udpPort As Integer = 5060, Optional ByVal tlsPort As Integer = 5061, Optional ByVal rtpPortStart As Integer = 9000, Optional ByVal maxConnections As Integer = 32, Optional ByVal fromIdentity As String = "sipx", Optional ByVal bindToAddress As String = "0.0.0.0", Optional ByVal useSequentialPorts As Boolean = False, Optional ByVal tlsCertificateNickname As String = Nothing, Optional ByVal tlsCertificatePassword As String = Nothing, Optional ByVal dbLocation As String = Nothing) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxUnInitialize")> _
  Protected Shared Function SipxUnInitialize(ByVal instanceHandle As IntPtr, ByVal forceShutdown As Boolean) As SipxResult
  End Function

  Private m_InstanceHandle As IntPtr
  Private m_EventQueueMonitor As EventQueueMonitor
  Private m_Event As SipxEvent
  Private m_EventDelegate As New SipxEvent.SipxInstanceEvent(AddressOf EventDelegate)
  Private m_Lines As Dictionary(Of Integer, SipxLine)
  Private m_Config As SipxConfig
  Private m_Audio As SipxMedia

  Private m_TcpPort As Integer
  Private m_UdpPort As Integer
  Private m_TlsPort As Integer
  Private m_RtpPortStart As Integer
  Private m_MaxConnections As Integer
  Private m_Identity As String = ""
  Private m_BindToAddress As String = ""
  Private m_UseSequentialPorts As Boolean
  Friend Event SipxInstanceEvent As EventHandler(Of EventArgs)

  Public Event LineEvent As EventHandler(Of SipxLineEventArgs)
  Public Event CallEvent As EventHandler(Of SipxCallEventArgs)

  Public ReadOnly Property Config() As SipxConfig
    Get
      Return (m_Config)
    End Get
  End Property

  Public ReadOnly Property Lines() As Dictionary(Of Integer, SipxLine)
    Get
      Return m_Lines
    End Get
  End Property

  Public ReadOnly Property Audio() As SipxMedia
    Get
      If m_Audio Is Nothing Then
        Throw New SipXTapiDotNet.SipxException("Audio is unavailable prior to calling connect()!")
      End If
      Return m_Audio
    End Get
  End Property

  Public Function LineCreate() As Integer
    Dim i As Integer = 0
    Do While m_Lines.ContainsKey(i) AndAlso m_Lines(i) IsNot Nothing
      i = i + 1
    Loop
    Lines.Add(i, SipxLine.Create(Me))
    AddHandler m_Lines(i).LineEvent, AddressOf m_LineEvent
    AddHandler m_Lines(i).CallEvent, AddressOf m_CallEvent
    Lines(i).Provision()
    Return i
  End Function
  Public Sub LineDispose(ByVal lineNumber As Integer)
    Lines(lineNumber).Dispose()
    Lines.Remove(lineNumber)
  End Sub
  Public Function FindCall(ByVal callhandle As IntPtr, ByRef linenumber As Integer, ByRef callnumber As Integer) As Boolean
    For Each lineKey As Integer In Me.Lines.Keys
      If Me.Lines(lineKey) IsNot Nothing Then
        For Each CallKey As Integer In Me.Lines(lineKey).Calls.Keys
          If Me.Lines(lineKey).Calls(CallKey) IsNot Nothing AndAlso Me.Lines(lineKey).Calls(CallKey).CallHandle = callhandle Then
            linenumber = lineKey
            callnumber = CallKey
            Exit For
          End If
        Next
      End If
      If linenumber > -1 Then
        Exit For
      End If
    Next
  End Function

  Public Property Identity() As String
    Get
      Return m_Identity
    End Get
    Set(ByVal value As String)
      m_Identity = value
    End Set
  End Property

  Friend ReadOnly Property InstanceHandle() As IntPtr
    Get
      Return m_InstanceHandle
    End Get
  End Property

  Public ReadOnly Property IsConnected() As Boolean
    Get
      Return m_InstanceHandle <> 0
    End Get
  End Property

  Public Sub Connect(ByVal enableRport As Boolean)
    Dim MySipxResult As SipxResult

    If m_InstanceHandle = 0 Then
      MySipxResult = SipxInitialize(m_InstanceHandle, m_TcpPort, m_UdpPort, m_TlsPort, m_RtpPortStart, m_MaxConnections, m_Identity, m_BindToAddress, m_UseSequentialPorts)
      If MySipxResult <> SipxResult.Success Then
        Throw New SipxException(MySipxResult)
      End If
      m_InstanceHandle = InstanceHandle

      m_Event = SipXTapiDotNet.SipxEvent.Create(Me)
      m_Event.AddListener(m_EventDelegate)
      m_Config.Rport = enableRport
      m_Audio = SipxMedia.Create(Me)

    Else
      Throw New SipxException("Disconnect() must be called before Connect() can be called again!")
    End If
  End Sub

  Public Sub Disconnect(ByVal forceShutdown As Boolean)
    Dim MySipxResult As SipxResult

    If m_Lines.Count > 0 Then
      Dim LineKeys As New Generic.List(Of Integer)
      For Each key As Integer In Lines.Keys
        LineKeys.Add(key)
      Next
      For Each key As Integer In LineKeys
        LineDispose(key)
      Next
    End If

    If m_InstanceHandle <> 0 Then
      MySipxResult = SipxUnInitialize(m_InstanceHandle, forceShutdown)
      If MySipxResult <> SipxResult.Success Then
        Throw New SipxException(MySipxResult)
      Else
        m_InstanceHandle = 0
      End If
    Else
      Throw New SipxException("Connect() must be called prior to Disconnect()!")
    End If
  End Sub

  Private Sub m_LineEvent(ByVal sender As Object, ByVal myEventArgs As SipxLineEventArgs)
    RaiseEvent LineEvent(sender, myEventArgs)
  End Sub

  Private Sub m_CallEvent(ByVal sender As Object, ByVal myEventArgs As SipxCallEventArgs)
    RaiseEvent CallEvent(sender, myEventArgs)
  End Sub

  ''' <summary>
  ''' Signature for event callback/observer.  Application developers should
  ''' not block this event callback thread -- doing so will cause deadlocks 
  ''' and will slow down call processing.  You should re-post these events
  ''' to your own thread context for handling.  
  ''' 
  ''' The application developer must look at the SipxEventCategory and then 
  ''' cast the pInfo method to the appropriate structure:
  '''
  ''' <pre>
  ''' CallState:   CallInfo   = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxCallStateInfo)), SipxCallStateInfo)
  ''' LineState:   LineInfo   = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxLineStateInfo)), SipxLineStateInfo)
  ''' InfoStatus:  InfoStatus = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxInfoStatusInfo)), SipxInfoStatusInfo)
  ''' Info:        InfoInfo   = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxInfoInfo)), SipxInfoInfo)
  ''' SubStatus:   SubInfo    = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxSubStatusInfo)), SipxSubStatusInfo)
  ''' Notify:      NotifyInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxNotifyInfo)), SipxNotifyInfo)
  ''' Config:      ConfigInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxConfigInfo)), SipxConfigInfo)
  ''' Security:    SecInfo    = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxSecurityInfo)), SipxSecurityInfo)
  ''' Media:       MediaInfo  = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxMediaInfo)), SipxMediaInfo)
  ''' </pre>
  ''' 
  ''' Please see the SipxEventCategory and structure definitions for details.
  ''' </summary>
  ''' <param name="eventCategory">The category of the event (call, line, 
  '''        subscription, notify, etc.).
  ''' </param>
  ''' <param name="eventStateHandle">Pointer to the event info structure.  
  '''        Depending on the event type, the application layer needs to 
  '''        cast this parameter to the appropriate structure.
  ''' </param>
  ''' <param name="userData">User data provided when listener was added</param>
  ''' <remarks></remarks>
    Private Sub EventDelegate(ByVal eventCategory As SipxEventCategory, ByVal eventStateHandle As IntPtr, ByVal userData As IntPtr)
    Select Case eventCategory
      Case SipxEventCategory.CallState
        Dim CallEventArgs As SipxCallEventArgs
        Dim CallStateInfo As SipxCallStateInfo
        CallStateInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxCallStateInfo)), SipxCallStateInfo)
        CallEventArgs = New SipxCallEventArgs(CallStateInfo)
        RaiseEvent SipxInstanceEvent(Me, CallEventArgs)

      Case SipxEventCategory.LineState
        Dim LineEventArgs As SipxLineEventArgs
        Dim LineStateInfo As SipxLineStateInfo
        LineStateInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxLineStateInfo)), SipxLineStateInfo)
        LineEventArgs = New SipxLineEventArgs(LineStateInfo)
        RaiseEvent SipxInstanceEvent(Me, LineEventArgs)

      Case SipxEventCategory.InfoStatus
        Dim InfoStatus As SipxInfoStatusInfo
        InfoStatus = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxInfoStatusInfo)), SipxInfoStatusInfo)
        Console.Write("InfoStatusEvent='" & InfoStatus.InfoStatusEvent.ToString & "', ")
        Console.Write("ResponseCode='" & InfoStatus.ResponseCode & "', ")
        Console.WriteLine("ResponseText='" & InfoStatus.ResponseText & "'")


      Case SipxEventCategory.Info
        Dim InfoInfo As SipxInfoInfo
        InfoInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxInfoInfo)), SipxInfoInfo)
        Console.Write("FromUrl='" & InfoInfo.FromUrl & "', ")
        Console.WriteLine("UserAgent='" & InfoInfo.UserAgent & "'")

      Case SipxEventCategory.SubStatus
        Dim SubInfo As SipxSubStatusInfo
        SubInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxSubStatusInfo)), SipxSubStatusInfo)
        Console.Write("SubState='" & SubInfo.SubState.ToString & "', ")
        Console.Write("SubCause='" & SubInfo.SubCause.ToString & "', ")
        Console.WriteLine("SubServerUserAgent='" & SubInfo.SubServerUserAgent & "'")

      Case SipxEventCategory.Notify
        Dim NotifyInfo As SipxNotifyInfo
        NotifyInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxNotifyInfo)), SipxNotifyInfo)
        Console.Write("NotifierUserAgent='" & NotifyInfo.NotifierUserAgent & "', ")
        Console.WriteLine("NotifierUserAgent='" & NotifyInfo.NotifierUserAgent & "'")

      Case SipxEventCategory.Config
        Dim ConfigInfo As SipxConfigInfo
        ConfigInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxConfigInfo)), SipxConfigInfo)
        Console.WriteLine("ConfigEvent='" & ConfigInfo.ConfigEvent.ToString & "'")

      Case SipxEventCategory.Security
        Dim SecInfo As SipxSecurityInfo
        SecInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxSecurityInfo)), SipxSecurityInfo)
        Console.Write("SecurityEvent='" & SecInfo.SecurityEvent.ToString & "', ")
        Console.Write("SecurityCause='" & SecInfo.SecurityCause.ToString & "', ")
        Console.WriteLine("RemoteAddress='" & SecInfo.RemoteAddress & "'")

      Case SipxEventCategory.Media
        'Dim MediaInfo As sipxMediaInfo
        'MediaInfo = CType(Marshal.PtrToStructure(eventStateHandle, GetType(SipxMediaInfo)), SipxMediaInfo)
    End Select
  End Sub

  Public Shared Function Create(ByVal identity As String, Optional ByVal tcpPort As Integer = 5060, Optional ByVal udpPort As Integer = 5060, Optional ByVal tlsPort As Integer = 5061, Optional ByVal rtpPortStart As Integer = 10000, Optional ByVal maxConnections As Integer = 32, Optional ByVal bindToAddress As String = "0.0.0.0", Optional ByVal useSequentialPorts As Boolean = False) As SipxInstance
    Dim rv As New SipxInstance

    rv.m_Identity = identity
    rv.m_TcpPort = tcpPort
    rv.m_UdpPort = udpPort
    rv.m_TlsPort = tlsPort
    rv.m_RtpPortStart = rtpPortStart
    rv.m_MaxConnections = maxConnections
    rv.m_BindToAddress = bindToAddress
    rv.m_UseSequentialPorts = useSequentialPorts
    Return rv
  End Function

  Private Sub New()
    m_EventQueueMonitor = EventQueueMonitor.GetMonitor(Me)
    m_Config = SipxConfig.Create(Me)
    m_Lines = New Dictionary(Of Integer, SipxLine)
  End Sub

#Region " IDisposable Support "
  ' IDisposable
  Private m_IsDisposed As Boolean = False    ' To detect redundant calls

  Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    Dim MySipxResult As SipxResult

    If Not m_IsDisposed Then
      If disposing Then
        ' TODO: free unmanaged resources when explicitly called
        If m_InstanceHandle <> 0 Then
          m_Event.Dispose()

          MySipxResult = SipxUnInitialize(m_InstanceHandle, True)
          If MySipxResult <> SipxResult.Success Then
            Throw New SipxException(MySipxResult)
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
    m_EventQueueMonitor.Shutdown()
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
#End Region

End Class