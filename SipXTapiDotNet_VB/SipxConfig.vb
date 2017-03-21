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
Imports System.Runtime.InteropServices
Public Class SipxConfig
  Implements IDisposable

#If DEBUG Then
  '  Friend Const SIPXTAPI_DLL = "C:\devrootsipx\sipxcalllib\examples\bin\sipXtapi.dll"
  Friend Const SIPXTAPI_DLL = "sipXtapid.dll"
#Else  
  Friend Const SIPXTAPI_DLL = "sipXtapi.dll"
#End If

#Region "Dll Imports"
  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigSetLogLevel")> _
  Protected Shared Function SipxConfigSetLogLevel(ByVal logLevel As SipxLogLevel) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigSetLogFile")> _
  Protected Shared Function SipxConfigSetLogFile(ByVal fileName As String) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigSetLogCallback")> _
  Protected Shared Function SipxConfigSetLogCallback(ByVal eventDelegate As SipxLogEvent) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigEnableRport")> _
  Protected Shared Function SipxConfigEnableRport(ByVal instanceHandle As IntPtr, ByVal enable As Boolean) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigEnableIce")> _
  Protected Shared Function SipxConfigEnableIce(ByVal instanceHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigDisableIce")> _
  Protected Shared Function SipxConfigDisableIce(ByVal instanceHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigEnableOutOfBandDTMF")> _
  Protected Shared Function SipxConfigEnableOutOfBandDtmf(ByVal instanceHandle As IntPtr, ByVal enable As Boolean) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigIsOutOfBandDTMFEnabled")> _
  Protected Shared Function SipxConfigIsOutOfBandDTMFEnabled(ByVal instanceHandle As IntPtr, ByRef isEnabled As Boolean) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigGetLocalContacts")> _
  Protected Shared Function SipxConfigGetLocalContacts(ByVal instanceHandle As IntPtr, ByVal addressesPtr As IntPtr, ByVal maxAddresses As UInteger, ByRef actualAddresses As UInteger) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigSetOutboundProxy")> _
  Protected Shared Function SipxConfigSetOutboundProxy(ByVal instanceHandle As IntPtr, ByVal outboundProxy As String) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConfigSetUserAgentName")> _
  Protected Shared Function sipxConfigSetUserAgentName(ByVal instanceHandle As IntPtr, ByVal userAgentName As String, ByVal includePlatformName As Boolean) As SipxResult
  End Function

  <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
  Public Delegate Sub SipxLogEvent(ByVal priority As String, ByVal sourceId As String, ByVal message As String)
#End Region

  Private m_Instance As SipxInstance
  Private Shared m_LogLevel As SipxLogLevel
  Private Shared m_LogFile As String
  Private m_OutboundProxy As String
  Private m_Rport As Boolean

  Friend ReadOnly Property Instance() As SipxInstance
    Get
      Return m_Instance
    End Get
  End Property

  Public Property EnableOutOfBandDtmf() As Boolean
    Get
      Dim MySipxResult As SipxResult
      Dim rv As Boolean
      MySipxResult = SipxConfigIsOutOfBandDTMFEnabled(m_Instance.InstanceHandle, rv)
      Return rv
    End Get
    Set(ByVal value As Boolean)
      Dim MySipxResult As SipxResult
      MySipxResult = SipxConfigEnableOutOfBandDtmf(m_Instance.InstanceHandle, value)
    End Set
  End Property

  Public Shared Property LogLevel() As SipxLogLevel
    Get
      Return m_LogLevel
    End Get
    Set(ByVal value As SipxLogLevel)
      Dim MySipxResult As SipxResult

      MySipxResult = SipxConfigSetLogLevel(value)
      If MySipxResult <> SipxResult.Success Then
        Throw New SipxException(MySipxResult)
      Else
        m_LogLevel = value
      End If
    End Set
  End Property

  Public Shared Property LogFile() As String
    Get
      Return m_LogFile
    End Get
    Set(ByVal value As String)
      Dim MySipxResult As SipxResult

      MySipxResult = SipxConfigSetLogFile(value)
      If MySipxResult <> SipxResult.Success Then
        Throw New SipxException(MySipxResult)
      Else
        m_LogFile = value
      End If
    End Set
  End Property

  Public Property OutboundProxy() As String
    Get
      Return m_OutboundProxy
    End Get
    Set(ByVal value As String)
      Dim MySipxResult As SipxResult

      MySipxResult = SipxConfigSetOutboundProxy(m_Instance.InstanceHandle, value)
      If MySipxResult <> SipxResult.Success Then
        Throw New SipxException(MySipxResult)
      Else
        m_OutboundProxy = value
      End If
    End Set
  End Property

  Public Sub SetUserAgentName(ByVal userAgentName As String, ByVal includePlatformName As Boolean)
    Dim MySipxResult As SipxResult

    MySipxResult = sipxConfigSetUserAgentName(m_Instance.InstanceHandle, userAgentName, includePlatformName)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Property Rport() As Boolean
    Get
      Return m_Rport
    End Get
    Set(ByVal value As Boolean)
      Dim MySipxResult As SipxResult

      MySipxResult = SipxConfig.SipxConfigEnableRport(m_Instance.InstanceHandle, value)
      If MySipxResult <> SipxResult.Success Then
        Throw New SipxException(MySipxResult)
      Else
        m_Rport = value
      End If
    End Set
  End Property

  'This code was broken on a recent sipxtapi update
  'Public Function GetLocalContacts() As Generic.IList(Of SipxContactAddress)
  '  Dim MySipxResult As SipxResult
  '  Dim rv As Generic.List(Of SipxContactAddress)
  '  Dim ContactPtr As IntPtr
  '  Dim Contacts() As SipxContactAddress = SipxContactAddress.Create(32)
  '  Dim Count As Integer
  '  Contacts(0).ContactId = 100
  '  Contacts(0).InterfaceName = "foo0"
  '  Contacts(1).ContactId = 101
  '  Contacts(1).InterfaceName = "foo1"
  '  Contacts(2).ContactId = 102
  '  Contacts(2).InterfaceName = "foo2"
  '  ContactPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(Contacts(0)) * 32)
  '  SipxContactAddress.ToPtr(Contacts, ContactPtr, 32)
  '  MySipxResult = SipxConfigGetLocalContacts(m_Instance.InstanceHandle, ContactPtr, 32, Count)
  '  'MySipxResult = SipxConfigGetLocalContacts(m_Instance.InstanceHandle, Contacts, 32, Count)
  '  If MySipxResult <> SipxResult.Success Then
  '    Marshal.FreeCoTaskMem(ContactPtr)
  '    Throw New SipxException(MySipxResult)
  '  Else
  '    Contacts = SipxContactAddress.FromPtr(ContactPtr, Count)
  '    Marshal.FreeCoTaskMem(ContactPtr)
  '  End If
  '  rv = New Generic.List(Of SipxContactAddress)(Count)
  '  For i As Integer = 0 To Count - 1
  '    rv.Add(Contacts(i))
  '  Next
  '  Return rv
  'End Function

  Public Sub DumpLocalContacts()
    'Dim contacts As Generic.IList(Of SipxContactAddress) =s GetLocalContacts()
    Dim contacts As New Generic.List(Of SipxContactAddress)
    For Each contact As SipxContactAddress In contacts
      Console.WriteLine("Contact: Id " & contact.ContactId & " Type, " & contact.ContactType.ToString & "TransportType " & contact.TransportType & ", Interface: " & contact.InterfaceName & ", Ip " & contact.IpAddress & ", Port " & contact.Port)
    Next
  End Sub


  Public Shared Function Create(ByVal instance As SipxInstance) As SipxConfig
    Dim rv As New SipxConfig

    rv.m_Instance = instance

    Return rv
  End Function

  Private Sub New()
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
