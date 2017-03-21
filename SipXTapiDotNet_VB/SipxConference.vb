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
Public Class SipxConference
  Implements IDisposable

#Region "DllImports"
  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConferenceCreate")> _
  Protected Shared Function SipxConferenceCreate(ByVal instanceHandle As IntPtr, ByRef conferenceHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConferenceJoin")> _
  Protected Shared Function SipxConferenceJoin(ByVal conferenceHandle As IntPtr, ByVal callHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConferenceJoin")> _
  Protected Shared Function SipxConferenceRemove(ByVal conferenceHandle As IntPtr, ByVal callHandle As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxConferenceDestroy")> _
  Protected Shared Function SipxConferenceDestroy(ByVal conferenceHandle As IntPtr) As SipxResult
  End Function
#End Region

  Private m_Instance As SipxInstance
  Private m_ConferenceHandle As IntPtr

  Public ReadOnly Property Instance() As SipxInstance
    Get
      Return m_Instance
    End Get
  End Property

  Public Sub AddCall(ByVal [call] As SipxCall)
    Dim MySipxResult As SipxResult
    [call].Hold(True)
    MySipxResult = SipxConferenceJoin(m_ConferenceHandle, [call].CallHandle)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
    [call].Unhold()
  End Sub

  Public Sub RemoveCall(ByVal [call] As SipxCall)
    Dim MySipxResult As SipxResult
    MySipxResult = SipxConferenceRemove(m_ConferenceHandle, [call].CallHandle)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If
  End Sub

  Public Shared Function Create(ByVal instance As SipxInstance) As SipxConference
    Dim rv As New SipxConference
    Dim MySipxResult As SipxResult
    Dim ConferenceHandle As IntPtr

    rv.m_Instance = instance

    MySipxResult = SipxConferenceCreate(instance.InstanceHandle, ConferenceHandle)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    End If

    rv.m_ConferenceHandle = ConferenceHandle
    Return rv
  End Function

#Region " IDisposable Support "
  Private m_IsDisposed As Boolean = False    ' To detect redundant calls

  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    If Not m_IsDisposed Then
      If disposing Then
        ' TODO: free unmanaged resources when explicitly called
        Dim MySipxResult As SipxResult
        MySipxResult = SipxConferenceDestroy(m_ConferenceHandle)
        If MySipxResult <> SipxResult.Success Then
          Throw New SipxException(MySipxResult)
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
