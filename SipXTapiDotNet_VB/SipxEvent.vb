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
Public Class SipxEvent
  Implements IDisposable

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxListenerAdd")> _
  Protected Shared Function SipxListenerAdd(ByVal instanceHandle As IntPtr, ByVal eventDelegate As SipxInstanceEvent, ByVal pUserData As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxEventListenerRemove")> _
  Protected Shared Function SipxEventListenerRemove(ByVal instanceHandle As IntPtr, ByVal eventDelegate As SipxInstanceEvent, ByVal pUserData As IntPtr) As SipxResult
  End Function

  <DllImport(SipxConfig.SIPXTAPI_DLL, SetLastError:=True, CallingConvention:=CallingConvention.Cdecl, EntryPoint:="sipxEventListenerAdd")> _
  Protected Shared Function SipxEventListenerAdd(ByVal instanceHandle As IntPtr, ByVal eventDelegate As SipxInstanceEvent, ByVal userData As IntPtr) As SipxResult
  End Function

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
  <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
  Public Delegate Sub SipxInstanceEvent(ByVal eventCategory As SipxEventCategory, ByVal eventStateHandle As IntPtr, ByVal userData As IntPtr)

  Private m_Instance As SipxInstance
  Private m_Delegates As New Generic.List(Of SipxInstanceEvent)

  Public ReadOnly Property Instance() As SipxInstance
    Get
      Return m_Instance
    End Get
  End Property

  Public Sub AddListener(ByVal eventDelegate As SipxInstanceEvent)
    Dim MySipxResult As SipxResult

    MySipxResult = SipxEvent.SipxEventListenerAdd(m_Instance.InstanceHandle, eventDelegate, Nothing)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    Else
      m_Delegates.Add(eventDelegate)
    End If
  End Sub

  Public Sub RemoveListener(ByVal eventDelegate As SipxInstanceEvent)
    Dim MySipxResult As SipxResult

    MySipxResult = SipxEvent.SipxEventListenerRemove(m_Instance.InstanceHandle, eventDelegate, Nothing)
    If MySipxResult <> SipxResult.Success Then
      Throw New SipxException(MySipxResult)
    Else
      m_Delegates.Remove(eventDelegate)
    End If
  End Sub


  Public Shared Function Create(ByVal instance As SipxInstance) As SipxEvent
    Dim rv As New SipxEvent

    rv.m_Instance = instance

    Return rv
  End Function

#Region " IDisposable Support "
  Private m_IsDisposed As Boolean = False    ' To detect redundant calls

  ' IDisposable
  Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    If Not m_IsDisposed Then
      If disposing Then
        For Each MyDelegate As SipxInstanceEvent In m_Delegates
          Me.RemoveListener(MyDelegate)
        Next
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
